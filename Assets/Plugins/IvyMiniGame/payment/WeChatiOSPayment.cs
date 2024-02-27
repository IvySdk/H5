using System.Collections;
using System.Collections.Generic;
using Ivy.MiniGame;
using UnityEngine;

namespace Ivy.Pay
{
    [System.Serializable]
    internal struct ServiceMessage
    {
        public string client_ip;
        public string platform;
        public string tradeNo;
        public string payId;
    }

#if IVYSDK_WX
    public class WeChatiOSPayment : WeChatPaymentInterface
    {
        void WeChatPaymentInterface.RequestPayment(string payId, string desc, int amount, string payload)
        {
            var pConfig = MiniGame.PaymentManager.GetPayConfig(payId);
            if (pConfig == null)
            {
                Utils.Log.Print($"pConfig is null");
                return;
            }
            
            //不同的小游戏要查询对应服务器的路由，来分散访问压力
            Utils.HttpUtil.I.HttpPostWithoutTimeout("https://wxapi.winwingplane.cn:8081/queryip", "", (ret, ip) =>
            {
                Utils.Log.Print($"resp code::{ret}; ip:{ip}");
                if (ret != Utils.SdkErrorCode.Ok) return;
                var outTradeNo = MiniGame.PaymentManager.GenerateOrderNo();
                MiniGame.MiniGameConfig.clientIp = ip;
                
                //这部分是需要返回给客户端的参数
                QueryPaymentResp resp = new QueryPaymentResp();
                resp.code = 1;
                int.TryParse(payId, out var id);
                resp.data = new PayData()
                {
                    orderId = outTradeNo,
                    billId = id,
                    payload = payload
                };
                var respContent = JsonUtility.ToJson(resp);
                
                WeChatWASM.WX.OpenCustomerServiceConversation(new WeChatWASM.OpenCustomerServiceConversationOption
                {
                    showMessageCard = true,
                    sendMessageTitle = "秘境消除故事",
                    sendMessageImg = "https://mergetales-1253615945.cos.ap-nanjing.myqcloud.com/WChat_Resource/wchat_pay_guide.png",
                    sessionFrom = JsonUtility.ToJson(new ServiceMessage
                    {
                        client_ip = MiniGame.MiniGameConfig.clientIp,
                        platform = MiniGame.MiniGameConfig.platform,
                        tradeNo = outTradeNo,
                        payId = payId
                    }),
                    complete = ret =>
                    {
                        Utils.Log.Print($"complete:{ret.errMsg}");
                        Cloud.CloudStorage.CallFunctionToSelf("pay_queryOrderLocal", 
                            new Dictionary<string, object>
                            {
                                ["outTradeNo"] = outTradeNo,
                                ["platform"] = "ios",
                            },
                            (action, content) =>
                            {
                                MiniGame.PaymentManager.ParsePayResponse(action, respContent);
                            }, 
                            msg => {
                                Utils.OperatorDispatcher.I.StartCoroutine(QueryWechatPayment(payId, outTradeNo, 1 ,respContent));
                            });
                    }
                });
            });

        }

        private static IEnumerator QueryWechatPayment(string payId, string tradeNo, int count, string respContent)
        {
            if (count > 6)
            {
                MiniGame.PaymentManager.failPayCall?.Invoke(payId);
                yield break;
            }
            yield return new WaitForSeconds(4f);;
            count += 1;
            Cloud.CloudStorage.CallFunctionToSelf("pay_queryOrderLocal", 
                new Dictionary<string, object>
                {
                    ["outTradeNo"] = tradeNo,
                    ["platform"] = "ios",
                },
                (action, content) =>
                {
                    MiniGame.PaymentManager.ParsePayResponse(action, respContent);
                }, 
                msg => {
                    Utils.OperatorDispatcher.I.StartCoroutine(QueryWechatPayment(payId, tradeNo, count, respContent));
                });
        }
    }
#endif
}

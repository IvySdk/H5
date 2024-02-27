using System.Collections;
using System.Collections.Generic;
using Ivy.MiniGame;
using UnityEngine;

namespace Ivy.Pay
{
#if IVYSDK_WX
    public class WeChatAndroidPayment : WeChatPaymentInterface
    {
        void WeChatPaymentInterface.RequestPayment(string payId, string desc, int amount, string payload)
        {
            var outTradeNo = MiniGame.PaymentManager.GenerateOrderNo();

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

            int isSandBox = MiniGame.MiniGameConfig.IsSandBox ? 1 : 0;
            WeChatWASM.WX.RequestMidasPayment(new WeChatWASM.RequestMidasPaymentOption
            {
                mode = "game",
                env = isSandBox,
                currencyType = "CNY",
                offerId = MiniGame.MiniGameConfig.offerId,
                buyQuantity = amount,
                outTradeNo = outTradeNo,
                success = resp =>
                {
                    Utils.Log.Print($"[pay_queryOrderLocal] start -- {outTradeNo}");
                    //支付成功回调，向服务器查询当次订单的付费情况
                    Cloud.CloudStorage.CallFunctionToSelf("pay_queryOrderLocal",
                        new Dictionary<string, object>()
                        {
                            ["outTradeNo"] = outTradeNo,
                            ["is_dev"] = isSandBox, 
                            ["platform"] = "android",
                        },
                        (action, content) =>
                        {
                            Utils.Log.Print($" ----[RequestWeChatPayment] {payId}  {action} --- {content}");
                            MiniGame.PaymentManager.ParsePayResponse(action, respContent);
                        },
                        msg =>
                        {
                            MiniGame.PaymentManager.failPayCall?.Invoke(payId);
                        });
                },
                fail = resp =>
                {
                    Utils.Log.Error($"支付失败 -- {resp.errMsg}");
                    MiniGame.PaymentManager.failPayCall?.Invoke(payId);
                },
            });
        }
    }
#endif
}
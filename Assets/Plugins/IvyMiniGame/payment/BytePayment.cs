using System;
using System.Collections;
using System.Collections.Generic;
using Ivy.MiniGame;
using UnityEngine;

namespace Ivy.Pay
{
#if IVYSDK_DY
    public class BytePayment : PaymentInterface
    {
        void PaymentInterface.RequestPayment(string payId, string desc, int amount, string payload)
        {
            if (!StarkSDKSpace.CanIUse.StarkPayService.RequestGamePayment) return;
            //支付前检查抖音的登录状态
            StarkSDKSpace.StarkSDK.API.GetAccountManager().CheckSession(
                () => { RequestBytePayment(payId, desc, amount, payload); },
                msg => { PaymentManager.failPayCall?.Invoke(payId); });
        }

        #region 内部函数

        private void RequestBytePayment(string payId, string desc, int amount, string payload)
        {
            var outTradeNo = PaymentManager.GenerateOrderNo();
            var orderInfoParams = new Dictionary<string, object>
            {
                ["mode"] = "game", //支付的类型, 目前仅为"game"
                ["env"] = "0", //环境配置，目前合法值仅为"0"
                ["currencyType"] = "CNY", // 固定值: CNY。币种
                ["platform"] = "android", //申请接入时的平台，目前仅为"android"
                ["buyQuantity"] = amount,
                //金币购买数量，金币数量必须满足：金币数量*金币单价 = 限定价格等级（详见下方 buyQuantity 限制说明。开发者可以在抖音小游戏平台的“支付”tab 设置游戏币单价）
                ["customId"] = outTradeNo,
                //todo 待测试 尝试重写支付通知地址
                ["notify_url"] = MiniGameConfig.payNotifyUrl,
                ["extraInfo"] = JsonUtility.ToJson(new ExtraAttach
                {
                    uuid = MiniGameConfig.openId,
                    pkg = MiniGameConfig.packageName,
                    pid = payId,
                    desc = desc,
                    code = MiniGameConfig.versionCode
                })
            };
            //游戏开发者自定义的唯一订单号，订单支付成功后通过服务端支付结果回调回传
            StarkSDKSpace.StarkSDK.API.GetStarkPayService().RequestGamePayment(
                orderInfoParams,
                () =>
                {
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

                    //支付成功回调，向服务器查询当次订单的付费情况
                    Cloud.CloudStorage.CallFunctionToSelf("pay_queryOrderDouYin",
                        new Dictionary<string, object>()
                        {
                            ["outTradeNo"] = outTradeNo,
                        },
                        (action, content) => { PaymentManager.ParsePayResponse(action, respContent); },
                        msg => { PaymentManager.failPayCall?.Invoke(payId); });
                },
                (errCode, errMsg) => { PaymentManager.failPayCall?.Invoke(payId); }
            );
        }

        #endregion
    }
#endif
}
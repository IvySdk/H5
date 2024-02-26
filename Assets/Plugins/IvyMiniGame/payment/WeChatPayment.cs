using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ivy.Pay
{
#if CN_WX
    internal interface WeChatPaymentInterface
    {
        internal void RequestPayment(string payId, string desc, int amount, string payload);
    }

    public class WeChatPayment : MiniGame.PaymentInterface
    {
        private readonly WeChatPaymentInterface _interface;
        public WeChatPayment()
        {
            switch (MiniGame.MiniGameConfig.platform)
            {
                case "ios":
                {
                    _interface = new WeChatiOSPayment();
                    break;
                }
                case "android":
                {
                    _interface = new WeChatAndroidPayment();
                    break;
                }
                default:
                {
                    _interface = null;
                    break;
                }
            }
        }

        void MiniGame.PaymentInterface.RequestPayment(string payId, string desc, int amount, string payload)
        {
            _interface?.RequestPayment(payId, desc, amount, payload);
        }
    }
#endif
}

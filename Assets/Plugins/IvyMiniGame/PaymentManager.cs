using System;
using System.Collections.Generic;
using Ivy.Pay;
using UnityEngine;

namespace Ivy.MiniGame
{
    #region 数据定义

    internal interface PaymentInterface
    {
        internal void RequestPayment(string payId, string desc, int amount, string payload);
    }

    //附加数据
    [Serializable]
    public class ExtraAttach
    {
        [SerializeField] public string uuid;
        [SerializeField] public string pid;
        [SerializeField] public string pkg;
        [SerializeField] public string desc;
        [SerializeField] public string code;
    }

    [Serializable]
    internal class PayData
    {
        [SerializeField] public int billId; // 计费点
        [SerializeField] public string orderId; //订单号
        [SerializeField] public string payload; // 订单追加信息
    }

    [Serializable]
    internal class QueryPaymentResp
    {
        [SerializeField] public int code;
        [SerializeField] public string message;
        [SerializeField] public PayData data;
    }

    #endregion

    public static class PaymentManager
    {
        private static PaymentInterface _instance;

        private static readonly Dictionary<string, PayIdConfig> _payDict = new Dictionary<string, PayIdConfig>();

        //初始化
        internal static void InitPaymentManager()
        {
#if IVYSDK_DY
            _instance = new BytePayment();
#elif IVYSDK_WX
            _instance = new WeChatPayment();
#endif
            //计费点重新写入字典
            foreach (var pConfig in MiniGameConfig.payConfigArray)
            {
                _payDict[pConfig.payId] = pConfig;
            }
        }

        private static void GamePayment(string payId, string desc, int amount, string payload)
        {
            _instance?.RequestPayment(payId, desc, amount, payload);
        }

        //付费
        public static void GamePayment(string payId)
        {
            var pConfig = GetPayConfig(payId);
            if (pConfig == null)
            {
                return;
            }

            GamePayment(payId, pConfig.desc, pConfig.price, "");
        }

        /// <summary>
        /// 付费
        /// </summary>
        /// <param name="payId"></param>
        /// <param name="payload"></param>
        public static void GamePaymentWithPayload(string payId, string payload)
        {
            var pConfig = GetPayConfig(payId);
            if (pConfig == null)
            {
                return;
            }
            GamePayment(payId, pConfig.desc, pConfig.price, payload);
        }

        //通过付费id查询付费配置
        public static PayIdConfig GetPayConfig(string payId)
        {
            return _payDict.TryGetValue(payId, out var pConfig) ? pConfig : null;
        }

        /// <summary>
        /// 字符串md5计算
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string MD5(this string s)
        {
            using var provider = System.Security.Cryptography.MD5.Create();
            var builder = new System.Text.StringBuilder();

            foreach (var b in provider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(s)))
                builder.Append(b.ToString("x2").ToLower());

            return builder.ToString();
        }

        private static readonly System.Random random = new System.Random((int)Utils.TimeManager.GetTimeStamp());

        //随机字符串
        private static string RandomString(int length = 8)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var charsArr = new char[length];
            for (var i = 0; i < charsArr.Length; i++)
            {
                charsArr[i] = characters[random.Next(characters.Length)];
            }

            return new string(charsArr);
        }

        //获取时间戳，单位 毫秒
        private static long GetTimestamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        }

        //随机md5产生订单号
        internal static string GenerateOrderNo()
        {
            var ch = "WX";
            switch (MiniGameConfig.channel)
            {
                case "douyin":
                    {
                        ch = "DY";
                        break;
                    }
            }

            var length = MiniGameConfig.appid.Length;
            length += ch.Length;
            var timestamp = GetTimestamp().ToString();
            length += timestamp.Length;
            var randomStr = "";
            if (length < 32)
            {
                randomStr = RandomString(32 - length);
            }
            return $"{MiniGameConfig.appid}{ch}{randomStr}{GetTimestamp()}";
        }

        internal static Action<string, string> successPayCall;
        internal static Action<string> failPayCall;

        internal static void ParsePayResponse(string action, string content)
        {
            switch (action)
            {
                //todo 增加其他查询结果
                case "pay_queryOrderDouYin":
                case "pay_queryOrderLocal":
                    {
                        Utils.OperatorDispatcher.I.AddActionToQueue(() =>
                        {
                            var data = JsonUtility.FromJson<QueryPaymentResp>(content);
                            var res = JsonUtility.ToJson(data.data);
                            Utils.Log.Print($" ----[ParsePayResponse]  {action} --- {res}");
                            successPayCall?.Invoke(action, res);
                        });
                        break;
                    }
            }
        }

    }
}
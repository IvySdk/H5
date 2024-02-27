using System;
using System.Collections.Generic;
using Ivy.Cloud;





#if IVYSDK_WX

using Ivy.MiniGame;
using Ivy.Event;


namespace com.Ivy
{
    public sealed class IvySdk
    {

        public class TencentCloudFuncData
        {
            public string funcKey = ""; // 云函数的请求 key 
            public int count; // 当前数据分段的 数量
            public int index; // 当前数据分段 索引，用于数据拼接排序
            public string data = ""; // 当前数据分段的 数据
        }

        public class PaymentData
        {
            public int billId; // 计费点
            public string orderId = ""; //订单号
            public string payload = ""; // 订单追加信息
        }

        public enum PaymentResult : int
        {
            Success = 1,
            Failed,
            Cancel,
            PaymentSystemError,
            PaymentSystemValid
        }

        public enum AdEventType : int
        {
            FullAdLoadCompleted = 1,
            FullAdLoadFailed,
            RewardAdLoadFailed,
            RewardAdLoadCompleted,
            RewardAdShown,
            RewardAdShowFinished,
            RewardAdShowFailed,
            RewardAdClosed,
            RewardAdClicked,
            FullAdClosed, // 大屏广告被关闭
            FullAdShown, // 大屏广告展示成功
            FullAdClicked, // 大屏广告被点击
            BannerAdClicked, // bannner广告被点击
            CrossAdClicked, // 交叉推广广告被点击
            AdLoadCompleted, //广告加载成功(only ios)
            AdLoadFailed, //广告加载失败(only ios)
            AdShown, // 广告展示成功(大屏或bannner)(only ios)
            AdClosed, //广告被关闭(only ios)
            AdClicked, //广告被点击(only ios)
            IconAdClicked,
            NativeAdClicked
        }

        /// <summary>
        /// 获取配置的AppId参数常量
        /// </summary>
        public const int CONFIG_KEY_APP_ID = 1;
        /// <summary>
        /// 获取应用的版本号参数常量
        /// </summary>
        public const int CONFIG_KEY_VERSION_CODE = 8;
        /// <summary>
        /// 获取应用的版本号名称参数常量
        /// </summary>
        public const int CONFIG_KEY_VERSION_NAME = 9;
        /// <summary>
        /// 获取应用的包名参数常量
        /// </summary>
        public const int CONFIG_KEY_PACKAGE_NAME = 10;
        //视频广告类型
        public const int ADTYPE_VIDEO = 2;

        private static IvySdk _instance;

        public static IvySdk Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new IvySdk();
                }
                return _instance;
            }
        }

        private IvySdk()
        {
            // TODO: Add constructor logic here
        }

        #region 广告

        public void ShowInterstitialAd(string tag, Action<InterstitialAdResult> rlt = null)
        {
            MiniSdk.ShowInter(tag, rlt);
        }

        public void ShowRewardedVideoAd(string tag, Action onRewarded = null, Action<RewardedVideoAdResult> rlt = null)
        {
            MiniSdk.ShowVideo(tag, onRewarded, rlt);
        }

        public void ShowBannerAd(string tag, BannerPosition bp, Action<BannerAdResult> rlt = null)
        {
            MiniSdk.ShowBanner(tag, bp, rlt);
        }

        public void ShowBannerAd(string tag, int left, int top, int width, Action<BannerAdResult> rlt = null)
        {
            MiniSdk.ShowBanner(tag, left, top, width, rlt);
        }

        public void HideBanner()
        {
            MiniSdk.HideBanner();
        }

        #endregion

        #region 登陆

        public void Login(Action<string> loginSuccess, Action<string> loginFail)
        {
            MiniSdk.Login(loginSuccess, loginFail);
        }

        public void IsLogin(Action<bool> onIsLogin)
        {
            MiniSdk.IsLogin(onIsLogin);
        }

        public string LoggedUser()
        {
            return MiniSdk.Me();
        }

        #endregion

        #region 支付

        public string GetPaymentData(int billingId)
        {
            return MiniSdk.GetPaymentData(billingId);
        }

        public void Pay(int billingId, string payload = null)
        {
            MiniSdk.GamePayment(billingId, payload);
        }

        #endregion


        #region 本地存储

        public bool HasLocalKey(string key)
        {
            return MiniSdk.HasLocalKey(key);
        }

        public void DeleteLocalKey(string key)
        {
            MiniSdk.DeleteLocalKey(key);
        }

        public void DeleteAll()
        {
            MiniSdk.DeleteAll();
        }

        public void SetLocalString(string key, string value = "")
        {
            MiniSdk.SetLocalString(key, value);
        }

        public string GetLocalStringSync(string key, string defaultValue)
        {
            return MiniSdk.GetLocalStringSync(key, defaultValue);
        }

        public void SetLocalInt(string key, int value)
        {
            MiniSdk.SetLocalInt(key, value);
        }

        public int GetLocalIntSync(string key, int defaultValue)
        {
            return MiniSdk.GetLocalIntSync(key, defaultValue);
        }

        public void SetLocalFloat(string key, float value)
        {
            MiniSdk.SetLocalFloat(key, value);
        }

        public float GetLocalFloatSync(string key, float defaultValue)
        {
            return MiniSdk.GetLocalFloatSync(key, defaultValue);
        }

        #endregion

        #region 小游戏客服

        public void ShowHelp()
        {
            MiniSdk.OpenCustomerServicePage();
        }

        #endregion

        #region  事件

        public void LogEvent(string eventKey, Dictionary<string, object> paramMap)
        {
            EventTracker.TrackEvent(eventKey, paramMap);
        }

        public void LogEvent(string eventKey, string paramContent)
        {
            EventTracker.TrackEvent(eventKey, paramContent);
        }

        #endregion

        #region 支持性

        public bool IsIOSSystem()
        {
            return MiniSdk.IsIOSSystem();
        }

        public Dictionary<string, object> GetSystemInfo()
        {
            return MiniSdk.GetSystemInfo();
        }

        public void SetClipboardData(string text, Action<bool> OnSetClipbardData = null)
        {
            MiniSdk.SetClipboardData(text, OnSetClipbardData);
        }

        #endregion

        #region 腾讯云函数

        public void CallTencentCloudFunction(string api, string content)
        {
            CloudStorage.CallFunction(api, content);
        }

        #endregion


    }
}
#endif
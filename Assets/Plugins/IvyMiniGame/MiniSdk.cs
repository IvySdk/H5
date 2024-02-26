using System;
using System.Collections.Generic;
using Ivy.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Ivy.MiniGame
{
    public static partial class MiniSdk
    {
        private static MiniSdkInterface _instance;

        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeInit()
        {
#if UNITY_EDITOR
            //editor环境下初始化editor实例
            _instance = new MiniSdkEditor();
#elif CN_WX
            //实例初始化为wechat实例
            _instance = new WeChatGame();
#elif CN_DY
            //实例初始化为抖音实例
            _instance = new ByteGame();
#endif
            InitMiniSDK();

            //加载游戏配置
            MiniGameConfig.LoadMiniGameConfig();
        }

        private static void InitMiniSDK()
        {
            _instance?.InitSdk(ret => { Utils.Log.Print($"OnRuntimeInit InitSDK:{ret}"); });
            LinkToRiseSdk();
        }

        ///报告场景启动
        public static void MustReportGameStart()
        {
            _instance?.ReportGameStart();
        }

        #region 登录

        public static string Me()
        {
            return _instance?.Me();
        }

        public static bool IsPreLogin;

        public static void IsLogin()
        {
            _instance?.IsLogin(ret =>
            {
                // result "0"：已经登陆 "1"：需要登陆
                RiseSdkListener.Instance.onWXPreLogState(ret ? "0" : "1");
            });
        }

        //回调从CloudStorage类的回调 onCloudFunctionXxxxx 的user_login action给出
        public static void Login()
        {
            _instance?.Login(result => { RiseSdkListener.Instance.onWXLogSuccess(result); },
                result => { RiseSdkListener.Instance.onWXLogFailed(result); });
        }

        #endregion

        #region 本地存储

        public static bool HasLocalKey(string key)
        {
            return _instance.HasLocalKey(key);
        }

        public static void DeleteLocalKey(string key)
        {
            _instance.DeleteLocalKey(key);
        }

        public static void DeleteAll()
        {
            _instance.DeleteAll();
        }

        public static void SetLocalString(string key, string value = "")
        {
            _instance.SetLocalString(key, value);
        }

        public static string GetLocalStringSync(string key, string defaultValue)
        {
            return _instance.GetLocalStringSync(key, defaultValue);
        }

        public static void SetLocalInt(string key, int value)
        {
            _instance.SetLocalInt(key, value);
        }

        public static int GetLocalIntSync(string key, int defaultValue)
        {
            return _instance.GetLocalIntSync(key, defaultValue);
        }

        public static void SetLocalFloat(string key, float value)
        {
            _instance.SetLocalFloat(key, value);
        }

        public static float GetLocalFloatSync(string key, float defaultValue)
        {
            return _instance.GetLocalFloatSync(key, defaultValue);
        }

        #endregion

        #region 广告

        ///显示banner广告
        public static void ShowBanner(string tag, BannerPosition bp, Action<BannerAdResult> rlt = null)
        {
            AdManager.ShowBanner(tag, bp, rlt);
        }

        ///显示banner广告
        public static void ShowBanner(string tag, int left, int top, int width, Action<BannerAdResult> rlt = null)
        {
            AdManager.ShowBanner(tag, left, top, width, rlt);
        }

        ///隐藏banner广告
        public static void HideBanner()
        {
            AdManager.HideBanner();
        }

        ///显示激励视频广告   
        public static void ShowVideo(string tag, Action onSuccess = null, Action<RewardedVideoAdResult> rlt = null)
        {
            AdManager.ShowRewardedVideo(tag, onSuccess, rlt);
        }

        ///显示插屏广告
        public static void ShowInter(string tag, Action<InterstitialAdResult> rlt = null)
        {
            AdManager.ShowInterstitial(tag, rlt);
        }

        #endregion


        #region 内部函数

        //通过这个函数链接RiseSdk，尽量防止耦合
        private static void LinkToRiseSdk()
        {
#if CN_DY || CN_WX
            //连接RiseSdk的支付回调
            PaymentManager.successPayCall = (act, content) => { RiseSdkListener.Instance.onPaymentSuccess(content); };
            PaymentManager.failPayCall = err => { RiseSdkListener.Instance.onPaymentFail(err); };
#endif
        }

        #endregion

        // 设置剪切板
        public static void SetClipboardData(string text)
        {
            _instance?.SetClipboardData(text, (result) => { RiseSdkListener.Instance.OnClipboardComplete(result); });
        }

        public static Dictionary<string, object> GetSystemInfo()
        {
            return _instance?.GetSystemInfo();
        }

        public static bool IsIOSSystem()
        {
            return _instance?.IsIOSSystem() ?? false;
        }

        public static string GetPaymentData(int billingId)
        {
            var payIdConfig = PaymentManager.GetPayConfig(billingId.ToString());
            Dictionary<string, object> payData = new Dictionary<string, object>();
            payData["type"] = "inapp";
            payData["payid"] = payIdConfig.payid;
            payData["currency"] = payIdConfig.currency;
            payData["price"] = payIdConfig.price;
            payData["rmb"] = payIdConfig.rmb;
            payData["amount"] = payIdConfig.amount;
            payData["desc"] = payIdConfig.desc;
            return JsonConvert.SerializeObject(payData);
        }

        public static Vector3 GetHorizonSafeAreaOffsetAndMenuOffset()
        {
            if (_instance != null)
            {
                return _instance.GetHorizonSafeAreaOffsetAndMenuOffset();
            }
            return Vector3.zero;
        }
    }

#if CN_DY
    public static partial class MiniSdk
    {
        // 开始录屏
        public static void StartRecordScreen(bool isRecordAudio, int maxRecordTime)
        {
            _instance?.StartRecordScreen(isRecordAudio, maxRecordTime, () => { RiseSdkListener.Instance.OnStartRecordScreenSuccess(); },
                (code, msg) => { RiseSdkListener.Instance.OnStartRecordScreenError(code, msg); },
                (msg) => { RiseSdkListener.Instance.OnStartRecordScreenTimeOut(msg); });
        }

        // 结束录屏
        public static void StopRecordScreen()
        {
            _instance?.StopRecordScreen(() => RiseSdkListener.Instance.OnStopRecordScreenComplete(),
                (code, msg) => { RiseSdkListener.Instance.OnStopRecordScreenError(code, msg); });
        }

        // 结束录屏
        public static int GetRecordDuration()
        {
            return _instance?.GetRecordDuration() ?? 0;
        }

        // 分享
        public static void ShareVideo(StarkSDKSpace.UNBridgeLib.LitJson.JsonData shareJson = null)
        {
            _instance?.ShareVideo((result) => { RiseSdkListener.Instance.OnShareVideoSuccess(result); },
                (msg) => { RiseSdkListener.Instance.OnShareVideoFailed(msg); },
                () => { RiseSdkListener.Instance.OnShareVideoCancel(); }, shareJson);
        }

        // 侧边栏
        public static void CheckScene()
        {
            _instance?.CheckScene((result) => { RiseSdkListener.Instance.OnCheckSceneSuccess(result); },
                () => { RiseSdkListener.Instance.OnCheckSceneComplete(); },
                (code, msg) => { RiseSdkListener.Instance.OnCheckSceneError(code, msg); });
        }

        public static void NavigateToScene()
        {
            _instance?.NavigateToScene(() => { RiseSdkListener.Instance.OnNavigateToSceneSuccess(); },
                () => { RiseSdkListener.Instance.OnNavigateToSceneComplete(); },
                (code, msg) => { RiseSdkListener.Instance.OnNavigateToSceneError(code, msg); });
        }

        public static bool IsFromSidebarCard()
        {
            return _instance?.IsFromSidebarCard() ?? false;
        }

        #region 小游戏客服

        public static void OpenCustomerServicePage()
        {
            _instance?.OpenCustomerServicePage((flag) => { Utils.Log.Print($"OpenCustomerServicePage flag:{flag}"); });
        }

        public static void OpenCustomerToPay(int billingId)
        {
            var payIdConfig = PaymentManager.GetPayConfig(billingId.ToString());
            var outTradeNo = PaymentManager.GenerateOrderNo();

            var extraInfo = new StarkSDKSpace.UNBridgeLib.LitJson.JsonData
            {
                ["uuid"] = MiniGameConfig.openId,
                ["pkg"] = MiniGameConfig.packageName,
                ["pid"] = payIdConfig.payid,
                ["desc"] = payIdConfig.desc,
                ["code"] = MiniGameConfig.versionCode
            };

            var options = new StarkSDKSpace.UNBridgeLib.LitJson.JsonData
            {
                ["buyQuantity"] = payIdConfig.amount,
                ["customId"] = outTradeNo,
                ["currencyType"] = "CNY",
                ["notify_url"] = MiniGameConfig.payNotifyUrl,
                ["extraInfo"] = extraInfo.ToString(),
            };
            _instance?.OpenAwemeCustomerService(options,
                () => { Utils.Log.Print("OpenCustomerToPay: OnAwemeCustomerServiceSuccess"); },
                (code, msg) => { Utils.Log.Print($"OpenCustomerToPay: OnAwemeCustomerServiceFail,code:{code},msg:{msg}"); });
        }

        #endregion

        // 屏蔽字
        /// int: 返回状态码，0为成功，否则为失败
        /// string: 错误信息
        /// JsonData：
        /// audit_result    int     1: 有敏感内容；2: 无敏感内容
        /// audit_content   string  被替换的内容（无敏感内容则返回值与传入的word相同）
        public static void ReplaceSensitiveWords(string words)
        {
            _instance?.ReplaceSensitiveWords(words, (code, msg, data) =>
            {
                if (code == 0)
                {
                    // 成功
                    var result = data.OptGetInt("audit_result");
                    var content = data.OptGetString("audit_content");
                    RiseSdkListener.Instance.OnReplaceSensitiveWords(code, result, content);
                }
                else
                {
                    // 失败
                    RiseSdkListener.Instance.OnReplaceSensitiveWords(code, 0, words);
                }
            });
        }

        // 键盘
        public static void ShowKeyboard()
        {
            RegisterKeyboardInput();
            _instance?.ShowKeyboard(s => { RiseSdkListener.Instance.OnKeyboardConfirm(s); });
        }

        public static void HideKeyboard()
        {
            _instance?.HideKeyboard(() => { RiseSdkListener.Instance.OnKeyboardComplete(); });
        }

        private static void RegisterKeyboardInput()
        {
            _instance?.OnKeyboardInput(s => { RiseSdkListener.Instance.OnKeyboardInput(s); });
        }
    }
#endif

#if CN_WX
    public static partial class MiniSdk
    {
        // 键盘
        public static void ShowKeyboard(string defaultValue, int maxLength)
        {
            _instance?.ShowKeyboard(
                defaultValue, 
                maxLength, 
                (sInput) => { RiseSdkListener.Instance.OnKeyboardInput(sInput); },
                (sConfirm) => { RiseSdkListener.Instance.OnKeyboardConfirm(sConfirm); }, 
                () => { RiseSdkListener.Instance.OnKeyboardComplete(); });
        }

        public static void HideKeyboard()
        {
            _instance?.HideKeyboard();
        }
        
        public static void OpenCustomerServicePage()
        {
            _instance?.OpenCustomerServicePage((flag) => { Utils.Log.Print($"OpenCustomerServicePage flag:{flag}"); });
        }
        public static void RegisterWxShowFunc(Action result)
        {
            _instance?.RegisterWxShowFunc(result);
        }
        public static void UnregisterWxShowFunc(Action result)
        {
            _instance?.UnregisterWxShowFunc(result);
        }
        public static void RegisterWxHideFunc(Action result)
        {
            _instance?.RegisterWxHideFunc(result);
        }
        public static void UnregisterWxHideFunc(Action result)
        {
            _instance?.UnregisterWxHideFunc(result);
        }

        public static void CleanAllFileCache()
        {
            _instance?.CleanAllFileCache();
        }
    }
#endif
}
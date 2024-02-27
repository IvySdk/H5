// #region Using

// using System;
// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using System.Text;
// using System.Security.Cryptography;
// using System.IO;
// using System.Runtime.InteropServices;
// using Ivy.Cloud;
// using Ivy.MiniGame;
// using Newtonsoft.Json;
// using UnityEngine.Networking;

// #if UNITY_EDITOR
// using UnityEditor;
// #endif

// #endregion

// /// <summary>
// /// SDK接口类
// /// </summary>
// public sealed partial class RiseSdk
// {
// #if !UNITY_IOS

//     #region 安卓部分，用于折叠

//     private static RiseSdk _instance = null;

//     private AndroidJavaClass _class = null;
//     private bool _wxInit = false;
//     private string BACK_HOME_ADPOS = M_CUSTOM;
//     private bool BACK_HOME_AD_ENABLE = false;
//     private double BACK_HOME_AD_TIME = 0;
//     private bool canShowBackHomeAd = false;
//     private int homeAdMinPauseMillisecond = 1000;
//     private int homeAdMinDeltaMillisecond = 1000;
//     private static bool DebugMono = true;

// #if UNITY_EDITOR
//     public static bool hasRewardAd = true;
//     public static int remoteConfigValue = 1;
// #endif

//     public FullAdType lastFullAdType = FullAdType.none;

//     /// <summary>
//     /// 配置计费系统的可用状态，SDK自动调用。
//     /// </summary>
//     /// <param name="valid">要配置的状态</param>
//     public void SetPaymentSystemValid(bool valid)
//     {
//         //paymentSystemValid = valid;
//     }

//     /// <summary>
//     /// 单例对象。
//     /// </summary>
//     public static RiseSdk Instance
//     {
//         get { return _instance ??= new RiseSdk(); }
//     }

//     private bool IsSDKValid => DebugMono && (_class != null || _wxInit);

//     /// <summary>
//     /// 初始化SDK，最好在第一个场景加载时初始化。
//     /// </summary>
//     public void Init()
//     {
//         RiseEditorAd.hasInit = true;
//         if (IsSDKValid)
//         {
//             return;
//         }
// #if CN_Android_Normal
//         try
//         {
//             RiseSdkListener.Instance.enabled = true;
//             _class = new AndroidJavaClass("com.android.client.Unity");
//             if (_class != null && DebugMono)
//             {
//                 using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
//                 {
//                     using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
//                     {
//                         _class.CallStatic("onCreate", context);
//                         DelayInvoke.DelayDo(() => { _class.CallStatic("showGameProtocolDialog"); }, 1f);
//                     }
//                 }
//             }
//         }
//         catch (Exception e)
//         {
// #if !UNITY_EDITOR
//             Debug.LogError ("RiseSdk Init Error:::\n" + e.StackTrace + "\n" + e.Message);
// #endif
//             _class = null;
//         }
// #elif IVYSDK_WX || IVYSDK_DY
//         _wxInit = true;
// #endif
//     }

//     private static bool showBarDisplay = false;

//     // 展示系统loading界面
//     public void ShowProgressBar()
//     {
//         if (_class != null && DebugMono)
//         {
//             showBarDisplay = true;
//             _class.CallStatic("showProgressBar");
//         }
//     }

//     // 隐藏系统loading界面
//     public void HideProgressBar()
//     {
//         if (_class != null && DebugMono)
//         {
//             if (showBarDisplay)
//             {
//                 showBarDisplay = false;
//                 _class.CallStatic("hideProgressBar");
//             }
//         }
//     }

//     #region 广告

//     #region Banner广告

//     /// <summary>
//     /// 显示bannner广告。
//     /// 如果有需要可以添加bannner广告被点击的回调：
//     /// RiseSdkListener.OnAdEvent += 
//     /// (
//     /// RiseSdk.AdEventType type//广告事件类型，需要判断是否为RiseSdk.AdEventType.BannerAdClicked
//     /// ) 
//     /// => {to do something};
//     /// </summary>
//     /// <param name="tag">bannner广告tag</param>
//     /// <param name="pos">bannner显示的位置，如：POS_BANNER_MIDDLE_BOTTOM为在底部居中显示</param>
//     public void ShowBanner(string tag, int pos)
//     {
// #if UNITY_EDITOR && EditorEnableAdTest
//         RiseEditorAd.EditorAdInstance.ShowBanner(tag, pos);
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("showBanner", tag, pos);
//             Debug.LogWarning("showBanner");
//         }
//     }

//     /// <summary>
//     /// 显示bannner广告。
//     /// 如果有需要可以添加bannner广告被点击的回调：
//     /// RiseSdkListener.OnAdEvent += 
//     /// (
//     /// RiseSdk.AdEventType type//广告事件类型，需要判断是否为RiseSdk.AdEventType.BannerAdClicked
//     /// ) 
//     /// => {to do something};
//     /// </summary>
//     /// <param name="pos">bannner显示的位置，如：POS_BANNER_MIDDLE_BOTTOM为在底部居中显示</param>
//     public void ShowBanner(int pos)
//     {
// #if UNITY_EDITOR && EditorEnableAdTest
//         RiseEditorAd.EditorAdInstance.ShowBanner(pos);
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("showBanner", pos);
//             Debug.LogWarning("showBanner");
//         }
//     }

//     public void ShowBanner(string tag, int pos, int animate)
//     {
// #if UNITY_EDITOR && EditorEnableAdTest
//         RiseEditorAd.EditorAdInstance.ShowBanner(tag, pos);
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("showBanner", tag, pos, animate);
//             Debug.LogWarning("showBanner");
//         }
//     }

//     public bool HasBanner(string tag)
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("hasBanner", tag);
//         }

//         return false;
//     }

//     /// <summary>
//     /// 关闭banner广告。
//     /// </summary>
//     public void CloseBanner()
//     {
// #if UNITY_EDITOR && EditorEnableAdTest
//         RiseEditorAd.EditorAdInstance.CloseBanner();
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("closeBanner");
//         }
//     }

//     #endregion

//     #region Native广告

//     /// <summary>
//     /// 显示Native广告。
//     /// </summary>
//     /// <param name="tag"></param>
//     /// <param name="yPercent"></param>
//     public void ShowNativeAd(string tag, int yPercent)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("ShowNativeAd");
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("showNative", tag, yPercent);
//         }
//     }

//     public bool ShowNativeAd(string tag, int xPixel, int yPixel, string configJson)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("ShowNativeAd");
// #endif
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("showNativeBanner", tag, xPixel, yPixel, configJson);
//         }

//         return false;
//     }

//     public bool ShowNativeAdWithFrame(string tag, float xPixel, float yPixel, float width, float height,
//         string configJson)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("ShowNativeAdWithFrame");
// #endif
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("showNativeBanner", tag, xPixel, yPixel, width, height, configJson);
//         }

//         return false;
//     }

//     public void CloseNativeAd(string tag)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("closeNativeBanner", tag);
//         }
//     }

//     /// <summary>
//     /// 隐藏Native广告。
//     /// </summary>
//     /// <param name="tag">广告tag</param>
//     public void HideNativeAd(string tag)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("hideNative", tag);
//         }
//     }

//     /// <summary>
//     /// 检测Native广告是否加载成功。
//     /// </summary>
//     /// <param name="tag">广告tag</param>
//     /// <returns>true成功， false失败</returns>
//     public bool HasNativeAd(string tag)
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("hasNative", tag);
//         }

//         return false;
//     }

//     #endregion

//     #region 插屏、激励广告

//     /// <summary>
//     /// 显示大屏广告。
//     /// 如果有需要可以添加大屏广告被点击和被关闭的回调：
//     /// RiseSdkListener.OnAdEvent += 
//     /// (
//     /// RiseSdk.AdEventType type//广告事件类型，需要判断是否为
//     /// //RiseSdk.AdEventType.FullAdClosed（大屏广告被关闭）
//     /// //或RiseSdk.AdEventType.FullAdClicked（大屏广告被点击）
//     /// ) 
//     /// => {to do something};
//     /// </summary>
//     /// <param name="tag">大屏广告弹出时机，如：M_PAUSE为游戏暂停时弹出</param>
//     public void ShowAd(string tag)
//     {
//         BACK_HOME_AD_TIME = GetCurrentTimeInMills();
// #if UNITY_EDITOR && EditorEnableAdTest
//         RiseEditorAd.EditorAdInstance.ShowAd(tag);
// #endif
// #if IVYSDK_DY || IVYSDK_WX
//         Ivy.MiniGame.AdManager.ShowInterstitial();
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("showFullAd", tag);
//         }
//     }

//     public void ShowAd(string tag, FullAdType adType)
//     {
//         BACK_HOME_AD_TIME = GetCurrentTimeInMills();
// #if UNITY_EDITOR && EditorEnableAdTest
//         RiseEditorAd.EditorAdInstance.ShowAd(tag);
// #endif
// #if IVYSDK_DY || IVYSDK_WX
//         Ivy.MiniGame.AdManager.ShowInterstitial();
// #endif
//         this.lastFullAdType = adType;
//         if (adType == FullAdType.back)
//             this.BackHomeAdTimeDelta(2000);
//         else
//             this.BackHomeAdTimeDelta(60000);
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("showFullAd", tag);
//         }
//     }

//     public bool HasInterstitial(string tag)
//     {
// #if UNITY_EDITOR && EditorEnableAdTest
//         return true;
// #endif
// #if IVYSDK_DY || IVYSDK_WX
//         return Ivy.MiniGame.AdManager.HasInterstitial();
// #endif
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("hasFull", tag);
//         }

//         return false;
//     }


//     /// <summary>
//     /// 检测视频广告是否加载完成。
//     /// </summary>
//     /// <returns>true完成，false失败</returns>
//     public bool HasRewardAd()
//     {
// #if UNITY_EDITOR && EditorEnableAdTest
//         return hasRewardAd;
// #endif
// #if IVYSDK_DY || IVYSDK_WX
//         return Ivy.MiniGame.AdManager.HasRewardAd();
// #endif
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("hasRewardAd");
//         }

//         return false;
//     }

//     public bool HasRewardedInterstitial()
//     {
// #if UNITY_EDITOR && EditorEnableAdTest
//         return true;
// #else
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("hasRewardedInterstitial");
//         }

//         return false;
// #endif
//     }

//     /// <summary>
//     /// 检测视频广告是否加载完成。
//     /// </summary>
//     /// <param name="tag">视频广告tag</param>
//     /// <returns>true完成，false失败</returns>
//     public bool HasRewardAd(string tag)
//     {
// #if UNITY_EDITOR && EditorEnableAdTest
//         return hasRewardAd;
// #endif
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("hasRewardAd", tag);
//         }

//         return false;
//     }

//     /// <summary>
//     /// 显示视频广告。
//     /// 该接口的回调方法为：
//     /// RiseSdkListener.OnRewardAdEvent += 
//     /// (
//     /// RiseSdk.AdEventType result,//是否成功显示视频广告，成功返回RiseSdk.AdEventType.RewardAdShowFinished，失败返回RiseSdk.AdEventType.RewardAdShowFailed
//     /// int rewardId//视频广告调用时机
//     /// ) 
//     /// => {to do something};
//     /// 如果有需要可以添加视频广告被关闭的回调：
//     /// RiseSdkListener.OnAdEvent += 
//     /// (
//     /// RiseSdk.AdEventType type//广告事件类型，需要判断是否为RiseSdk.AdEventType.VideoAdClosed
//     /// ) 
//     /// => {to do something};
//     /// </summary>
//     /// <param name="rewardId">客户端自己配置的视频广告调用时机</param>
//     public void ShowRewardAd(int rewardId)
//     {
//         BACK_HOME_AD_TIME = GetCurrentTimeInMills();
// #if UNITY_EDITOR && EditorEnableAdTest
//         RiseEditorAd.EditorAdInstance.ShowRewardAd(rewardId);
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("showRewardAd", rewardId);
//         }
//     }

//     public void ShowRewardedInterstitial(int rewardId)
//     {
//         BACK_HOME_AD_TIME = GetCurrentTimeInMills();
// #if UNITY_EDITOR && EditorEnableAdTest
//         RiseEditorAd.EditorAdInstance.ShowRewardAd(rewardId);
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("showRewardedInterstitial", rewardId);
//         }
//     }

//     /// <summary>
//     /// 显示视频广告。
//     /// 该接口的回调方法为：
//     /// RiseSdkListener.OnRewardAdEvent += 
//     /// (
//     /// RiseSdk.AdEventType result,//是否成功显示视频广告，成功返回RiseSdk.AdEventType.RewardAdShowFinished，失败返回RiseSdk.AdEventType.RewardAdShowFailed
//     /// int rewardId，//视频广告调用时机
//     /// string tag//后台配置的视频广告位置tag
//     /// ) 
//     /// => {to do something};
//     /// </summary>
//     /// <param name="tag">后台配置的视频广告位置tag</param>
//     /// <param name="rewardId">客户端自己配置的视频广告调用时机</param>
//     public void ShowRewardAd(string tag, int rewardId)
//     {
//         BACK_HOME_AD_TIME = GetCurrentTimeInMills();
// #if UNITY_EDITOR && EditorEnableAdTest
//         RiseEditorAd.EditorAdInstance.ShowRewardAd(tag, rewardId);
// #endif
// #if IVYSDK_DY || IVYSDK_WX
//         Ivy.MiniGame.AdManager.ShowRewardedVideo(rewardId);
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("showRewardAd", tag, rewardId);
//         }
//     }


//     public void ShowRewardedInterstitial(string tag, int rewardId)
//     {
// #if UNITY_EDITOR && EditorEnableAdTest
//         RiseEditorAd.EditorAdInstance.ShowRewardAd(tag, rewardId);
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("showRewardedInterstitial", tag, rewardId);
//         }
//     }

//     #endregion

//     public void InitBackHomeAd(string adPos, int minPauseMillisecond = 20000)
//     {
//         BACK_HOME_ADPOS = adPos;
//         homeAdMinPauseMillisecond = minPauseMillisecond;
//     }

//     public void enableBackHomeAd(bool enabled)
//     {
//         BACK_HOME_AD_ENABLE = enabled;
//     }

//     public void BackHomeAdTimeDelta(int minAdTimeDelta = 2000)
//     {
//         homeAdMinDeltaMillisecond = minAdTimeDelta;
//     }

//     #endregion


//     /// <summary>
//     /// 更多游戏接口，跳到推广的游戏列表界面。
//     /// </summary>
//     public void ShowMore()
//     {
//         BACK_HOME_AD_TIME = GetCurrentTimeInMills();
// #if UNITY_EDITOR && EditorEnableAdTest
//         RiseEditorAd.EditorAdInstance.Toast("ShowMore");
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("moreGame");
//         }
//     }

//     //追踪用户打开窗体的时间
//     public void TrackScreen(string screenClass, string screenName)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[TrackScreen] screenClass:" + screenClass + " screenName:" + screenName);
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("trackScreen", screenClass, screenName);
//         }
//     }

//     public void CopyText(string str)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[CopyText] str:" + str);
// #endif
// #if CN_Android_Normal
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("copyText", str);
//         }
// #endif
// #if IVYSDK_DY || IVYSDK_WX
//         MiniSdk.SetClipboardData(str);
// #endif
//     }

//     #region 本地存储(存到安卓工程目录下)

//     public void MMKVWithId(string collectionName)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("mmkvWithId", collectionName);
//         }
//     }

//     public void MMRemoveKey(string key)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("mmRemoveKey", key);
//         }
//     }

//     public void MMRemoveKey2(string collectionName, string key)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("mmRemoveKey2", collectionName, key);
//         }
//     }

//     public bool MMContainsKey(string key)
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("mmContainsKey", key);
//         }

//         return false;
//     }

//     public bool MMContainsKey2(string collectionName, string key)
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("mmContainsKey2", collectionName, key);
//         }

//         return false;
//     }

//     public void MMSetStringValue(string key, string value)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("mmSetStringValue", key, value);
//         }
//     }

//     public void MMSetStringValue2(string collectionName, string key, string value)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("mmSetStringValue2", collectionName, key, value);
//         }
//     }

//     public string MMGetStringValue(string key, string defaultValue)
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<string>("mmGetStringValue", key, defaultValue);
//         }

//         return defaultValue;
//     }

//     public string MMGetStringValue2(string collectionName, string key, string defaultValue)
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<string>("mmGetStringValue2", collectionName, key, defaultValue);
//         }

//         return defaultValue;
//     }

//     public void MMSetIntValue(string key, int value)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("mmSetIntValue", key, value);
//         }
//     }

//     public void MMSetIntValue2(string collectionName, string key, int value)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("mmSetIntValue2", collectionName, key, value);
//         }
//     }

//     public int MMGetIntValue(string key, int defaultValue)
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<int>("mmGetIntValue", key, defaultValue);
//         }

//         return defaultValue;
//     }

//     public int MMGetIntValue2(string collectionName, string key, int defaultValue)
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<int>("mmGetIntValue2", collectionName, key, defaultValue);
//         }

//         return defaultValue;
//     }

//     public void MMSetLongValue(string key, long value)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("mmSetLongValue", key, value);
//         }
//     }

//     public void MMSetLongValue2(string collectionName, string key, long value)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("mmSetLongValue2", collectionName, key, value);
//         }
//     }

//     public long MMGetLongValue(string key, long defaultValue)
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<long>("mmGetLongValue", key, defaultValue);
//         }

//         return defaultValue;
//     }

//     public long MMGetLongValue2(string collectionName, string key, long defaultValue)
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<long>("mmGetLongValue2", collectionName, key, defaultValue);
//         }

//         return defaultValue;
//     }

//     public void MMSetBoolValue(string key, bool value)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("mmSetBoolValue", key, value);
//         }
//     }

//     public void MMSetBoolValue2(string collectionName, string key, bool value)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("mmSetBoolValue2", collectionName, key, value);
//         }
//     }

//     public bool MMGetBoolValue(string key, bool defaultValue)
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("mmGetBoolValue", key, defaultValue);
//         }

//         return defaultValue;
//     }

//     public bool MMGetBoolValue2(string collectionName, string key, bool defaultValue)
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("mmGetBoolValue2", collectionName, key, defaultValue);
//         }

//         return defaultValue;
//     }

//     #endregion


//     public void LogError(string errorMessage)
//     {
//         return; // sdk 暂无此方法
// #if UNITY_EDITOR

// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("logError", errorMessage);
//         }
//     }

//     /// <summary>
//     /// 游戏获得焦点，SDK自动调用。
//     /// </summary>
//     public void OnResume()
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("onResume");
//         }

//         if (BACK_HOME_AD_ENABLE)
//         {
//             if (canShowBackHomeAd && BACK_HOME_AD_TIME <= 0)
//             {
//                 canShowBackHomeAd = false;
//                 if (GetCurrentTimeInMills() - pauseTime > homeAdMinPauseMillisecond)
//                 {
//                     ShowAd(BACK_HOME_ADPOS, FullAdType.back);
//                 }
//             }
//         }
//     }

//     private double pauseTime = 0;

//     /// <summary>
//     /// 游戏失去焦点，SDK自动调用。
//     /// </summary>
//     public void OnPause()
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("onPause");
//         }

//         if (BACK_HOME_AD_ENABLE)
//         {
//             double now = GetCurrentTimeInMills();
//             double delta = now - BACK_HOME_AD_TIME;
//             canShowBackHomeAd = delta > homeAdMinDeltaMillisecond;
//             if (canShowBackHomeAd)
//             {
//                 BACK_HOME_AD_TIME = 0;
//             }

//             pauseTime = now;
//         }
//     }

//     /// <summary>
//     /// 游戏打开，SDK自动调用。
//     /// </summary>
//     public void OnStart()
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("onStart");
//         }
//     }

//     /// <summary>
//     /// 游戏退出，SDK自动调用。
//     /// </summary>
//     public void OnStop()
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("onStop");
//         }
//     }

//     /// <summary>
//     /// 游戏销毁，SDK自动调用。
//     /// </summary>
//     public void OnDestroy()
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("onDestroy");
//         }
//     }

//     /// <summary>
//     /// 需要退出游戏时调用。
//     /// </summary>
//     public void OnExit()
//     {
//         BACK_HOME_AD_TIME = GetCurrentTimeInMills();
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.OnExit();
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("onQuit");
//         }
//     }

//     public void OnKill()
//     {
//         BACK_HOME_AD_TIME = GetCurrentTimeInMills();
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.OnExit();
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("onKill");
//         }
//     }

//     /// <summary>
//     /// 检测计费点是否存在。
//     /// </summary>
//     /// <param name="billingId">计费点id</param>
//     public void HasPaid(int billingId)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("query", billingId);
//         }
//     }

//     /// <summary>
//     /// 检测计费是否可用。
//     /// </summary>
//     /// <returns>true可用，false不可用</returns>
//     public bool IsPayEnabled()
//     {
//         return true;
//     }

//     /// <summary>
//     /// 支付接口，对计费点进行支付。
//     /// 该接口的回调方法为：
//     /// RiseSdkListener.OnPaymentEvent += 
//     /// (
//     /// int result,//计费结果，如：PAYMENT_RESULT_SUCCESS为计费成功
//     /// int billId//计费点id
//     /// ) 
//     /// => {to do something};
//     /// </summary>
//     /// <param name="billingId">计费点id</param>
//     public void Pay(int billingId)
//     {
//         BACK_HOME_AD_TIME = GetCurrentTimeInMills();
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Pay(billingId);
// #elif IVYSDK_DY || IVYSDK_WX
//         PaymentManager.GamePayment(billingId.ToString());
// #elif CN_Android_Normal
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("pay", billingId);
//         }
// #endif
//     }

//     public void CheckSubscriptionActive(int billingId)
//     {
//         return; //sdk无此方法
//         if (_class != null && DebugMono)
//         {
//             try
//             {
//                 Debug.Log($" CheckSubscriptionActive : {billingId}");
//                 _class.CallStatic("isSubscriptionActive", billingId);
//             }
//             catch (Exception e)
//             {
//                 Debug.Log(e);
//             }
//         }
//     }


//     public void SetPayVerifyUrl(string url)
//     {
// #if UNITY_EDITOR
// #else
//         if (_class != null && DebugMono) {
//             _class.CallStatic ("setPayVerifyUrl", url);
//         }
// #endif
//     }

//     public void PayWithPayload(int billingId, string payloadData)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.PayWithPayload(billingId, payloadData);
// #elif CN_Android_Normal
//         if (_class != null && DebugMono) {
//             _class.CallStatic ("pay", billingId, payloadData);
//         }
// #elif IVYSDK_DY || IVYSDK_WX
//         PaymentManager.GamePaymentWithPayload(billingId.ToString(), payloadData);
// #else
// #endif
//     }

//     //打开本地图片
//     public void OpenAlbum()
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("OpenAlbum success");
// #else
//         if (_class != null && DebugMono) {
//             _class.CallStatic ("openAlbum");
//         }
// #endif
//     }

//     public void OpenPhoto()
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("OpenPhoto success");
// #else
//         if (_class != null && DebugMono) {
//             _class.CallStatic ("openCamera");
//         }
// #endif
//     }


//     /// <summary>
//     /// 分享游戏。
//     /// </summary>
//     public void Share()
//     {
//         BACK_HOME_AD_TIME = GetCurrentTimeInMills();
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Share");
// #endif
//         if (_class == null)
//         {
//             return;
//         }

//         _class.CallStatic("share");
//     }

//     public const string ShareHashTag = "#MergeElves";

//     public void ShareWithURL(string shareID, string url, string quote, string hashTag = ShareHashTag)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("ShareWithURL");
// #else
//         if (_class == null)
//             return;
//         _class.CallStatic("share", shareID, url, quote, hashTag);
// #endif
//     }


//     /// <summary>
//     /// 退出游戏
//     /// </summary>
//     public void ForceQuit()
//     {
// #if UNITY_EDITOR
//         UnityEditor.EditorApplication.isPlaying = false;
// #endif
//         if (_class == null)
//         {
//             return;
//         }

//         _class.CallStatic("forceQuit");
//     }

//     /// <summary>
//     /// 获取自定义json数据，该接口最好在SDK初始化完成后3秒或以上再调用。
//     /// </summary>
//     /// <returns>返回后台配置的自定义json数据，如：{"x":"x", "x":8, "x":{x}, "x":[x]}</returns>
//     public string GetExtraData()
//     {
//         if (_class == null)
//         {
//             return null;
//         }

//         return _class.CallStatic<string>("getExtraData");
//     }

//     /// <summary>
//     /// 统计数据。
//     /// </summary>
//     /// <param name="category">需要统计的数据分类名称</param>
//     /// <param name="action">需要统计的数据属性名称</param>
//     /// <param name="label">数据的属性值</param>
//     /// <param name="value">一般传0</param>
//     public void TrackEvent(string category, string action, string label, int value)
//     {
//         if (_class == null)
//         {
//             return;
//         }

//         _class.CallStatic("trackEvent", category, action, label, value);
//     }

//     public void TrackEvent(string category, string keyValueData)
//     {
// #if CN_Android_Normal
//         if (_class == null)
//         {
//             return;
//         }
//         _class.CallStatic("trackEvent", category, keyValueData);
        
// #elif IVYSDK_WX || IVYSDK_DY
//         Ivy.Event.EventTracker.TrackEvent(category, keyValueData);
// #endif

//     }

//     /// <summary>
//     /// 获取应用商店最新版本（只有ios 使用）
//     /// </summary>
//     public string GetAppStoreVersion()
//     {
//         return "Default";
//     }

//     /// <summary>
//     /// 跳转到游戏评分界面。
//     /// </summary>
//     public void Rate()
//     {
//         BACK_HOME_AD_TIME = GetCurrentTimeInMills();
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Rate");
// #endif
//         if (_class == null)
//         {
//             return;
//         }

//         _class.CallStatic("rate");
//     }

//     public void CheckUpdate()
//     {
//         if (_class == null)
//         {
//             return;
//         }

//         _class.CallStatic("checkUpdate");
//     }

//     /// <summary>
//     /// 检测是否已经登陆。
//     /// </summary>
//     /// <returns>true已登陆， false未登陆</returns>
//     public void IsLogin()
//     {
// #if IVYSDK_DY || IVYSDK_WX
//         MiniSdk.IsLogin();
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("isLogin");
//         }
//     }

//     /// <summary>
//     /// 登出
//     /// </summary>
//     public void Logout()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Logout");
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("logout");
//         }
//     }

//     /// <summary>
//     /// 获取后台配置的全部內付计费点信息，价格单位、价格、打折信息、描述
//     /// <returns>
//     /// 返回json数据，格式为：
//     /// 22 23 等是计费点
//     /// price_amount 一般用于计算折扣，当前的价格是打折后的价格，通过折扣来计算原价，展示给用户
//     /// {
//     ///     "22": {
//     ///         "id": "com.ivy.galaxyshooting.sky22",
//     ///         "type": "inapp",
//     ///         "price": "HK$15.00",
//     ///         "price_amount": 15.0,
//     ///         "currency": "HKD",
//     ///         "title": " remove Ads +2000 coins (Galaxy sky shooting)",
//     ///         "desc": " remove Ads +2000 coins"
//     ///     },
//     ///     "23": {
//     ///         "id": "com.ivy.galaxyshooting.sky23",
//     ///         "type": "subs",
//     ///         "price": "HK$15.00",
//     ///         "price_amount": 15.0,
//     ///         "currency": "HKD",
//     ///         "title": "VIP Privilege (Galaxy sky shooting)",
//     ///         "desc": "VIP Privilege"
//     ///     }
//     /// }
//     /// </returns>
//     public string GetPaymentDatas()
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<string>("getPaymentDatas");
//         }
//         else
//         {
//             return "{}";
//         }
//     }

//     /// <summary>
//     /// 获取后台配置的某一个內付计费点信息，价格单位、价格、打折信息、描述
//     /// </summary>
//     /// <param name="billingId">计费点Id</param>
//     /// <returns>
//     /// 返回json数据，格式为：
//     /// {
//     ///     "id": "com.ivy.galaxyshooting.sky25",
//     ///     "type": "inapp",
//     ///     "price": "HK$78.00",
//     ///     "price_amount": 78.0,
//     ///     "currency": "HKD",
//     ///     "title": "unlock Hades (Galaxy sky shooting)",
//     ///     "desc": "unlock Hades"
//     /// }
//     /// </returns>
//     public string GetPaymentData(int billingId)
//     {
// #if IVYSDK_DY || IVYSDK_WX
//         return MiniSdk.GetPaymentData(billingId);
// #endif
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<string>("getPaymentData", billingId);
//         }
//         else
//         {
//             return "{}";
//         }
//     }

//     public void GetPaymentDataAsyn(int billingId)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("getPaymentDataAsyn", billingId);
//         }
//     }

//     public void GetPaymentDatasAsyn(string billingIds)
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("getPaymentDatasAsyn", billingIds);
//         }
//     }

//     public bool IsPaymentValid()
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("isPaymentValid");
//         }
//         else
//         {
//             return false;
//         }
//     }

//     /// <summary>
//     /// 获取后台配置信息和Unity无法直接获取的一些信息。
//     /// </summary>
//     /// <param name="configId">需要获取的信息类型，如：CONFIG_KEY_VERSION_CODE为获取游戏的版本号</param>
//     /// <returns>返回请求的信息</returns>
//     public string GetConfig(int configId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("GetConfig, configId: " + configId);
// #endif
// #if IVYSDK_DY || IVYSDK_WX
//         return MiniGameConfig.GetConfig(configId);
// #endif
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<string>("getConfig", configId);
//         }
//         else
//         {
//             return "";
//         }
//     }


//     public void InAppReview()
//     {
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("inAppReview");
//         }
//     }

//     /// <summary>
//     /// 获取指定app的后台配置信息和Unity无法直接获取的一些信息。
//     /// </summary>
//     /// <param name="packageName"></param>
//     /// <param name="configId">需要获取的信息类型，如：CONFIG_KEY_VERSION_CODE为获取游戏的版本号</param>
//     /// <returns>返回请求的信息</returns>
//     public string GetConfig(string packageName, int configId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("GetConfig, packageName: " + packageName + ", configId: " + configId);
// #endif
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<string>("getConfig", packageName, configId);
//         }
//         else
//         {
//             return "";
//         }
//     }

//     /// <summary>
//     /// 系统的确认对话框。
//     /// </summary>
//     /// <param name="title">对话框标题</param>
//     /// <param name="message">对话框内容</param>
//     public void Alert(string title, string message)
//     {
//         BACK_HOME_AD_TIME = GetCurrentTimeInMills();
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Alert(title, message);
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("alert", title, message);
//         }
//     }

//     /// <summary>
//     /// 系统的toast提示信息。
//     /// </summary>
//     /// <param name="message">提示内容</param>
//     public void Toast(string message)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast(message);
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("toast", message);
//         }
//     }

//     public void DisplayUrl(string title, string url)
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning($"[DisplayUrl]title:{title},url:{url}");
// #endif
//         if (_class != null)
//         {
//             _class.CallStatic("displayUrl", title, url);
//         }
//     }

//     /// <summary>
//     /// 检测网络是否可用。
//     /// </summary>
//     /// <returns>true可用， false不可用</returns>
//     public bool IsNetworkConnected()
//     {
// #if UNITY_EDITOR || CLOUD_TEST || SUBSCRIBE_LOCAL_Test
//         return true;
// #endif
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("isNetworkConnected");
//         }

//         return true;
//     }

//     /// <summary>
//     /// 判断用户刘海屏
//     /// </summary>
//     /// <returns></returns>
//     public bool HasNotch()
//     {
// #if UNITY_EDITOR
//         return true;
// #endif
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("hasNotch");
//         }

//         return true;
//     }


//     public string EncodeParams(string dataStr)
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<string>("encodeParams", dataStr);
//         }

//         return "";
//     }


//     /// <summary>
//     /// AIHelp
//     /// </summary>
//     /// <param name="entranceId">自定义入口id</param>
//     /// <param name="meta">JsonObject格式，用户数据</param>
//     /// <param name="tags">用户标签；在具体为用户配置标签时，你应该确保已经提前在 AIHelp 后台配置好了对应的标签内容</param>
//     /// <param name="welcomeMessage">自定义欢迎语</param>
//     public void ShowAIHelp(String entranceId, String meta)
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("showAIHelp 2 params");
// #endif
// #if IVYSDK_DY || IVYSDK_WX
//         // AIHelper.Instance.ShowAIHelp();
//         MiniSdk.OpenCustomerServicePage();
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("showAIHelp", entranceId, meta);
//         }
//     }

//     /// <summary>
//     /// AIHelp 是否已初始化
//     /// </summary>
//     /// <returns></returns>
//     public bool IsAIHelpInitialized()
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("init AIHelp");
//         return false;
// #endif
// #if IVYSDK_DY || IVYSDK_WX
//         return true;
// #endif
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("isAIHelpInitialized");
//         }

//         return false;
//     }

//     /// <summary>
//     /// 获取未读消息数,每5分钟轮询一次
//     /// 监听 unreadMessageCount 获取返回值，参数 int
//     /// </summary>
//     /// <param name="onlyOnce">是否只获取一次</param>
//     public void LoadAIHelpUnreadMessageCount(bool onlyOnce)
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("LoadAIHelpUnreadMessageCount");
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("loadAIHelpUnreadMessageCount", onlyOnce);
//         }
//     }


// #if Headline
//     public int Getrecordtime(int defaultValue = 30)
//     {
// #if UNITY_EDITOR
//         return defaultValue;
// #endif
//         if (_class != null && DebugMono)
//         {
//             int time = _class.CallStatic<int>("getRemoteConfigInt", "recordtime");
//             if (time <= 0)
//                 time = defaultValue;
//             return time;
//         }

//         return defaultValue;
//     }

//     #region 分享
//     /// <summary>
//     /// 抖音图片分享
//     /// DouYin the picture share.
//     /// </summary>
//     public void DyPictureShare(string pictureFileName)
//     {

//     }

//     /// <summary>
//     /// 抖音视频分享
//     /// DouYin the video share.
//     /// </summary>
//     public void DyVideoShare(string videoFileName)
//     {

//     }

//     /// <summary>
//     /// 智能视频编辑和抖音分享
//     /// AI video editor and douyin share.
//     /// </summary>
//     public void AIEditorVideoShare(string fileName)
//     {

//     }
//     #endregion

//     #region 录屏
//     bool isRecording = false;
//     public void OpenSettingPage()
//     {
// #if UNITY_EDITOR

// #endif
//         if (_class != null && DebugMono)
//             _class.CallStatic("openSettingPage");
//     }

//     public void RequestPhotoWritePermission()
//     {
// #if UNITY_EDITOR

// #endif
//         if (_class != null && DebugMono)
//             _class.CallStatic("requestPhotoWritePermission");
//     }

//     public bool HasPhotoWritePermission()
//     {
// #if UNITY_EDITOR
//         return true;
// #endif
//         if (_class != null && DebugMono)
//             return _class.CallStatic<bool>("hasPhotoWritePermission");
//         return false;
//     }
//     /**
//  * 是否支持录屏
//  * 1.系统API 小于 21
//  * 2.CPU 频率小于1700MHZ
//  * 3.运行内存小于2048MB
//  *
//  * @param context context
//  * @return 若支持 返回 {@link ScreenRecordResult#ERRNO_SUCCESS}
//  */
//     public bool CheckSupScreenRecord()
//     {
// #if UNITY_EDITOR
//         return false;
// #endif
// #if Headline
//         if (_class != null && DebugMono)
//             return _class.CallStatic<bool>("isRecordingSupported");
//         return false;
// #endif
//     }

//     public bool IsRecording()
//     {
// #if UNITY_EDITOR
//         return isRecording;
// #endif
//         if (_class != null && DebugMono)
//             return _class.CallStatic<bool>("isRecording");
//         return false;
//     }
//     /// <summary>
//     /// 开始录屏
//     /// Screen record start.
//     /// </summary>
//     public void ScreenRecordStart()
//     {
// #if UNITY_EDITOR
//         isRecording = true;
// #endif
//         if (_class != null && DebugMono)
//             _class.CallStatic("startRecording");
//     }

//     /// <summary>
//     /// 暂停录屏
//     /// Screen record pause.
//     /// </summary>
//     public void ScreenRecordPause()
//     {

//     }

//     /// <summary>
//     /// 继续录屏
//     /// Screen record resume.
//     /// </summary>
//     public void ScreenRecordResume()
//     {

//     }

//     /// <summary>
//     /// 停止录屏
//     /// Screen record stop.
//     /// </summary>
//     public void ScreenRecordStop(bool autoEdit)
//     {
// #if UNITY_EDITOR
//         isRecording = false;
// #endif
//         if (_class != null && DebugMono)
//             _class.CallStatic("stopRecording");
//     }

//     public void ShareRecentRecordVideo()
//     {
//         if (_class != null && DebugMono)
//             _class.CallStatic("shareRecentRecordVideo");
//     }
//     #endregion

//     #region 埋点
//     /// <summary>
//     /// 等级上报
//     /// </summary>
//     /// <param name="level">等级</param>
//     public void OnEventUpdateLevel(int level)
//     {
//         if (_class != null && DebugMono)
//             _class.CallStatic("UM_setPlayerLevel", level);
//     }
//     #endregion
// #endif

//     #region RemoteConfig

//     public int GetRemoteConfigInt(string remoteKey)
//     {
// #if UNITY_EDITOR
//         return remoteConfigValue;
// #endif
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<int>("getRemoteConfigInt", remoteKey);
//         }

//         return 0;
//     }

//     public long GetRemoteConfigLong(string remoteKey)
//     {
// #if UNITY_EDITOR
//         return remoteConfigValue;
// #endif
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<long>("getRemoteConfigLong", remoteKey);
//         }

//         return 0L;
//     }

//     public double GetRemoteConfigDouble(string remoteKey)
//     {
// #if UNITY_EDITOR
//         return remoteConfigValue;
// #else
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<double>("getRemoteConfigDouble", remoteKey);
//         }
//         return 0D;
// #endif
//     }

//     public bool GetRemoteConfigBoolean(string remoteKey)
//     {
// #if UNITY_EDITOR
//         return remoteConfigValue == 0 ? false : true;
// #else
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<bool>("getRemoteConfigBoolean", remoteKey);
//         }
//         return false;
// #endif
//     }


//     public static string LoadUserPackJsonFromFile()
//     {
//         if (!File.Exists(Application.dataPath + "/Data/TestUserPack.json"))
//         {
//             return null;
//         }

//         StreamReader sr = new StreamReader(Application.dataPath + "/Data/TestUserPack.json");
//         if (sr == null)
//         {
//             return null;
//         }

//         string json = sr.ReadToEnd();
//         return json;
//     }


//     public string GetRemoteConfigString(string remoteKey)
//     {
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<string>("getRemoteConfigString", remoteKey);
//         }

//         return "";
//     }

//     public void SetUserTag(string tag)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("setUserTag: " + tag);
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("setUserTag", tag);
//         }
//     }

//     public void SetUserProperty(string key, string value)
//     {
// #if UNITY_EDITOR
//         //RiseEditorAd.EditorAdInstance.Toast("key: " + key + ", value: " + value);
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("setUserProperty", key, value);
//         }
//     }

//     #endregion

//     #endregion

// #elif UNITY_IOS || PLATFORM_IOS
//     [DllImport("__Internal")]
//     private static extern void showGameProtocolDialog();

//     [DllImport("__Internal")]
//     private static extern void queryPushNotification();
//     [DllImport("__Internal")]
//     private static extern void openApplicationSetting();
//     [DllImport("__Internal")]
//     private static extern void requestIDFA();

//     [DllImport("__Internal")]
//     private static extern int getScreenWidth();
//     [DllImport("__Internal")]
//     private static extern int getScreenHeight();

//     [DllImport("__Internal")]
//     private static extern int showProgressBar();
//     [DllImport("__Internal")]
//     private static extern int hideProgressBar();
//     [DllImport("__Internal")]
//     private static extern int getScreenDesignWidth();
//     [DllImport("__Internal")]
//     private static extern int getScreenDesignHeight();
//     [DllImport("__Internal")]
//     private static extern void onCreate();
//     [DllImport("__Internal")]
//     private static extern bool isBannerAvailable();
//     [DllImport("__Internal")]
//     private static extern bool isBannerAvailableWithTag(string tag);
//     [DllImport("__Internal")]
//     private static extern void showBanner(int pos);
//     [DllImport("__Internal")]
//     private static extern void showBannerWithTag(string tag, int pos);
//     [DllImport("__Internal")]
//     private static extern void closeBanner();
//     [DllImport("__Internal")]
//     private static extern bool isVideoAvailable();
//     [DllImport("__Internal")]
//     private static extern bool isVideoAvailableWithTag(string tag);
//     [DllImport("__Internal")]
//     private static extern void showRewardVideo(int placementId);
//     [DllImport("__Internal")]
//     private static extern void showRewardVideoWithTag(string tag, int placementId);

//     [DllImport("__Internal")]
//     private static extern bool hasRewardedInterstitial();
//     // [DllImport("__Internal")]
//     // private static extern void showRewardedInterstitial(int rewardId);
//     // [DllImport("__Internal")]
//     // private static extern void showRewardedInterstitial(string tag, int rewardId);
//     [DllImport("__Internal")]
//     private static extern void sendChat(string path,string leagueID, string jsonData);
//     [DllImport("__Internal")]
//     private static extern void listen(string path, string leagueID);
//     [DllImport("__Internal")]
//     private static extern void initFirestoreAfterSignIn(string provider);
//     [DllImport("__Internal")]
//     private static extern void initializeAndLinkGoogleAfterSignInFacebook();
//     [DllImport("__Internal")]
//     private static extern void readFirestore(string collectionName);

//     [DllImport("__Internal")]
//     private static extern void loginAppleBeforeFirestore(bool isForce);
//     [DllImport("__Internal")]
//     private static extern void linkFirestoreAfterAppleSignIn();

//     [DllImport("__Internal")]
//     private static extern void readFirestore2(string collectionName, string documentId);
//     [DllImport("__Internal")]
//     private static extern void mergeFirestore(string collectionName, string jsonData);
//     [DllImport("__Internal")]
//     private static extern void setFirestore(string collectionName, string jsonData);
//     [DllImport("__Internal")]
//     private static extern void updateFirestore(string collectionName, string translationId, string jsonData);
//     [DllImport("__Internal")]
//     private static extern void deleteFirestore(string collectionName);
//     [DllImport("__Internal")]
//     private static extern void snapshotFirestore(string collectionName);
//     [DllImport("__Internal")]
//     private static extern void signOutFirestore();
//     [DllImport("__Internal")]
//     private static extern string getFirebaseUserId();    
//     [DllImport("__Internal")]
//     private static extern void cloudfunction(string funcName);
//     [DllImport("__Internal")]
//     private static extern void cloudfunction2(string funcName, string funcParamsJson);
//     [DllImport("__Internal")]
//     private static extern void trackScreen(string screenClass, string screenName);
//     [DllImport("__Internal")]
//     private static extern void copyText(string str);
//     // [DllImport("__Internal")]
//     // private static extern void playerFinder();
//     [DllImport("__Internal")]
//     private static extern void initializeAndLinkFacebookAfterSignIn();
//     [DllImport("__Internal")]
//     private static extern void logoutFacebook();
//     // [DllImport("__Internal")]
//     // private static extern string getPushToken();

//     // [DllImport("__Internal")]
//     // private static extern void mmkvWithId(string collectionName); 
//     [DllImport("__Internal")]
//     private static extern void mmRemoveKey(string key);
//     // [DllImport("__Internal")]
//     // private static extern void mmRemoveKey2(string collectionName,string key);
//     [DllImport("__Internal")]
//     private static extern bool mmContainsKey(string key);
//     // [DllImport("__Internal")]
//     // private static extern bool mmContainsKey2(string collectionName,string key);
//     [DllImport("__Internal")]
//     private static extern void mmSetStringValue(string key,string value);
//     // [DllImport("__Internal")]
//     // private static extern void mmSetStringValue2(string collectionName,string key,string value);
//     [DllImport("__Internal")]
//     private static extern string mmGetStringValue(string key, string defaultValue);
//     // [DllImport("__Internal")]
//     // private static extern string mmGetStringValue2(string collectionName,string key, string defaultValue);
//     [DllImport("__Internal")]
//     private static extern void mmSetIntValue(string key, int value);
//     // [DllImport("__Internal")]
//     // private static extern void mmSetIntValue2(string collectionName,string key, int value);
//     [DllImport("__Internal")]
//     private static extern int mmGetIntValue(string key, int defaultValue);
//     // [DllImport("__Internal")]
//     // private static extern int mmGetIntValue2(string collectionName,string key, int defaultValue);
//     [DllImport("__Internal")]
//     private static extern void mmSetLongValue(string key, long value);
//     // [DllImport("__Internal")]
//     // private static extern void mmSetLongValue2(string collectionName,string key, long value);
//     [DllImport("__Internal")]
//     private static extern long mmGetLongValue(string key, long defaultValue);
//     // [DllImport("__Internal")]
//     // private static extern long mmGetLongValue2(string collectionName,string key, long defaultValue);
//     [DllImport("__Internal")]
//     private static extern void mmSetBoolValue(string key, bool value);
//     // [DllImport("__Internal")]
//     // private static extern void mmSetBoolValue2(string collectionName,string key, bool value);
//     [DllImport("__Internal")]
//     private static extern bool mmGetBoolValue(string key, bool defaultValue);
//     // [DllImport("__Internal")]
//     // private static extern bool mmGetBoolValue2(string collectionName,string key, bool defaultValue);

//     // [DllImport("__Internal")]
//     // private static extern void appFeedback(string sectionUrl, string systemInfo);
//     // [DllImport("__Internal")]
//     // private static extern void logIvyError(string errorMessage);

//     [DllImport("__Internal")]
//     private static extern void displayHelpEngagement(string uid, string userInfoJson);
    
//     [DllImport("__Internal")]
//     private static extern void saveUserAttribute(string dataJsonString);
//     // [DllImport("__Internal")]
//     // private static extern void subscribeFCMTopic(string saveKey, string newTopic);

//     [DllImport("__Internal")]
//     private static extern bool isInterstitialAvailable(string tag);
//     [DllImport("__Internal")]
//     private static extern void showInterstitialAd(string tag);
//     [DllImport("__Internal")]
//     private static extern void showInterstitialAdWithTag(string tag, int delayShowSeconds);
//     [DllImport("__Internal")]
//     private static extern void showInterstitialAdWithTag2(string tag, int delayShowSeconds, double delayTimeInterval);
//     [DllImport("__Internal")]
//     private static extern void showIconAd(float width, float xPercent, float yPercent);
//     [DllImport("__Internal")]
//     private static extern void showPopupIconAds();
//     [DllImport("__Internal")]
//     private static extern bool isNativeAvailable(string tag);
//     [DllImport("__Internal")]
//     private static extern void showNativeAd(string tag, float xPixel, float yPixel, string configJson);
//     [DllImport("__Internal")]
//     private static extern void showNativeAdWithFrame(string tag, float x, float y, float width, float height, string configJson);
//     [DllImport("__Internal")]
//     private static extern void closeNativeAd(string tag);
//     [DllImport("__Internal")]
//     private static extern void loadInterstitialAd(string tag);
//     [DllImport("__Internal")]
//     private static extern void closeIconAd();

//     [DllImport("__Internal")]
//     private static extern bool isDeliciousAdAvailable();
//     [DllImport("__Internal")]
//     private static extern void showDeliciousInterstitialAd(string configJson);
//     [DllImport("__Internal")]
//     private static extern void showDeliciousBannerAd(float x, float y, float w, float h, string configJson);
//     [DllImport("__Internal")]
//     private static extern void closeDeliciousBannerAd();
//     [DllImport("__Internal")]
//     private static extern void showDeliciousIconAd(float x, float y, float w, float h, string configJson);
//     [DllImport("__Internal")]
//     private static extern void closeDeliciousIconAd();

//     [DllImport("__Internal")]
//     private static extern void rateUs();
//     [DllImport("__Internal")]
//     private static extern void rateInApp();
//     [DllImport("__Internal")]
//     private static extern void rateUsWithStar(float star);
//     [DllImport("__Internal")]
//     private static extern void rateInAppWithStar(float star);

//     [DllImport("__Internal")]
//     private static extern string getAppstoreVersion();
//     [DllImport("__Internal")]
//     private static extern void setPayVerifyUrl(string url);
//     [DllImport("__Internal")]
//     private static extern bool isNetworkAvailable();
//     [DllImport("__Internal")]
//     private static extern void pay(int billingId);
//     [DllImport("__Internal")]
//     private static extern void payWithPayload(int billingId, string payloadData);
//     [DllImport("__Internal")]
//     private static extern void isSubscriptionActive();
//     [DllImport("__Internal")]
//     private static extern int[] getPurchasedIds();
//     [DllImport("__Internal")]
//     private static extern void clearPurchasedIds();
//     [DllImport("__Internal")]
//     private static extern void clearPurchasedId(int billingId);
//     [DllImport("__Internal")]
//     private static extern bool isAdsEnabled();
//     [DllImport("__Internal")]
//     private static extern void setAdsEnable(bool enable);
//     [DllImport("__Internal")]
//     private static extern bool hasNotch();
//     [DllImport("__Internal")]
//     private static extern void verifyIdCard();
//     [DllImport("__Internal")]
//     private static extern bool hasGdpr();
//     [DllImport("__Internal")]
//     private static extern void resetGdpr();
//     [DllImport("__Internal")]
//     private static extern void restorePayments();
//     [DllImport("__Internal")]
//     private static extern string getPaymentDatas();
//     [DllImport("__Internal")]
//     private static extern string getPaymentData(int billingId);
//     [DllImport("__Internal")]
//     private static extern string getPopupIconAdsData();
//     [DllImport("__Internal")]
//     private static extern string getExtraData();
//     [DllImport("__Internal")]
//     private static extern string getPushData();
//     [DllImport("__Internal")]
//     private static extern string getConfig(int configId);

//     // [DllImport("__Internal")]
//     // private static extern void getPaymentDataAsyn(int billingId);
//     // [DllImport("__Internal")]
//     // private static extern void getPaymentDatasAsyn(string billingIds);

//     [DllImport("__Internal")]
//     private static extern void loginGameCenter();
//     [DllImport("__Internal")]
//     private static extern void login();
//     [DllImport("__Internal")]
//     private static extern void logout();
//     [DllImport("__Internal")]
//     private static extern void isLogin();
//     [DllImport("__Internal")]
//     private static extern string meFirstName();
//     [DllImport("__Internal")]
//     private static extern string meLastName();
//     [DllImport("__Internal")]
//     private static extern string meId();
//     [DllImport("__Internal")]
//     private static extern string meName();
//     [DllImport("__Internal")]
//     private static extern string me();
//     [DllImport("__Internal")]
//     private static extern string friends();
//     [DllImport("__Internal")]
//     private static extern string mePictureURL();
//     [DllImport("__Internal")]
//     private static extern void fetchFriends(bool invitable);
//     [DllImport("__Internal")]
//     private static extern void fetchScores();
//     [DllImport("__Internal")]
//     private static extern void invite();
//     [DllImport("__Internal")]
//     private static extern void share();
//     // [DllImport("__Internal")]
//     // private static extern void share2(string shareID, string url, string quote, string hashTag);
//     [DllImport("__Internal")]
//     private static extern void shareContent(string contentURL, string tag, string quote);
//     [DllImport("__Internal")]
//     private static extern bool isIPhoneX();
//     [DllImport("__Internal")]
//     private static extern void logPlayerLevel(int levelId);
//     [DllImport("__Internal")]
//     private static extern void logPageStart(string pageName);
//     [DllImport("__Internal")]
//     private static extern void logPageEnd(string pageName);
//     [DllImport("__Internal")]
//     private static extern void logEvent(string eventId);
//     [DllImport("__Internal")]
//     private static extern void logEventWithTag(string eventId, string tag);
//     [DllImport("__Internal")]
//     private static extern void logEventWithData(string eventId, string data);
//     [DllImport("__Internal")]
//     private static extern void logEventLikeGA(string category, string action, string label, int value);
//     [DllImport("__Internal")]
//     private static extern void logStartLevel(string level);
//     [DllImport("__Internal")]
//     private static extern void logFailLevel(string level);
//     [DllImport("__Internal")]
//     private static extern void logFinishLevel(string level);
//     [DllImport("__Internal")]
//     private static extern void logBuy(string itemName, int count, double price);
//     [DllImport("__Internal")]
//     private static extern void logUse(string itemName, int count, double price);
//     [DllImport("__Internal")]
//     private static extern void logBonus(string itemName, int count, double price, int trigger);
//     [DllImport("__Internal")]
//     private static extern void sdklog(string info);
//     [DllImport("__Internal")]
//     private static extern void toast(string info);
//     [DllImport("__Internal")]
//     private static extern void displayUrl(string title, string url);
//     [DllImport("__Internal")]
//     private static extern void logFinishAchievement(string achievement);
//     [DllImport("__Internal")]
//     private static extern void logFinishTutorial(string tutorial);
//     [DllImport("__Internal")]
//     private static extern bool justShowFullAd();

//     [DllImport("__Internal")]
//     private static extern bool isGameCenterAvailable();
//     [DllImport("__Internal")]
//     private static extern void showLeaderboards();
//     [DllImport("__Internal")]
//     private static extern void showLeaderboard(int leaderboardId);
//     [DllImport("__Internal")]
//     private static extern void showAchievements();
//     [DllImport("__Internal")]
//     private static extern void submitScore(int leaderboardId, long score);
//     [DllImport("__Internal")]
//     private static extern long myHighScore(int leaderboardId);
//     [DllImport("__Internal")]
//     private static extern void submitAchievement(int achievementId, double percent);
//     [DllImport("__Internal")]
//     private static extern double getAchievementProgress(int achievementId);
//     [DllImport("__Internal")]
//     private static extern void sendMail(string address, string subject, string content, bool isHTML);
//     [DllImport("__Internal")]
//     private static extern int getRemoteConfigIntValue(string key);
//     [DllImport("__Internal")]
//     private static extern long getRemoteConfigLongValue(string key);
//     [DllImport("__Internal")]
//     private static extern double getRemoteConfigDoubleValue(string key);
//     [DllImport("__Internal")]
//     private static extern bool getRemoteConfigBoolValue(string key);
//     [DllImport("__Internal")]
//     private static extern string getRemoteConfigStringValue(string key);
//     [DllImport("__Internal")]
//     private static extern void cancelRemoteNotification(string key);
//     [DllImport("__Internal")]
//     private static extern void cancelLocalNotification(string key);
//     [DllImport("__Internal")]
//     private static extern void cancelAllLocalNotifications();
//     [DllImport("__Internal")]
//     private static extern string getLocalNotificationDataJson();
//     [DllImport("__Internal")]
//     private static extern void pushLocalNotification(string key, string title, string msg, long pushTime, int interval, bool repeat, bool useSound, string soundName, string userInfo);
//     [DllImport("__Internal")]
//     private static extern void pushLocalNotificationWithDateStr(string key, string title, string msg, string dateStr, int interval, bool repeat, bool useSound, string soundName, string userInfo);
//     [DllImport("__Internal")]
//     private static extern void pushRemoteNotification(string key, string title, string content, long pushTime, bool useLocalTimeZone, string facebookIds, string uuids, string topic, int iosBadge, bool useSound, string soundName, string dataJson);
//     [DllImport("__Internal")]
//     private static extern void saveBase64ImageToCameraRoll(string base64Image);
//     [DllImport("__Internal")]
//     private static extern void setAdsUnderAgeMode(bool value);
//     [DllImport("__Internal")]
//     private static extern void setUserPropertyString(string key, string value);
//     [DllImport("__Internal")]
//     private static extern bool hasNewVersion();

//     [DllImport("__Internal")]
//     private static extern void inAppMessageDismissed();
//     [DllImport("__Internal")]
//     private static extern void inAppMessageClicked(int index);
//     [DllImport("__Internal")]
//     private static extern void inAppReview();
//     [DllImport("__Internal")]
//     private static extern bool showSignInWithApple(float x, float y, float w, float h, bool useBlackBackgrounds);
//     [DllImport("__Internal")]
//     private static extern void hideSignInWithApple();

//     [DllImport("__Internal")]
//     private static extern void showAIHelp(string entranceId, string meta, string tags, string welcomeMessage);
//     [DllImport("__Internal")]
//     private static extern bool isAIHelpInitialized();
//     [DllImport("__Internal")]
//     private static extern void loadAIHelpUnreadMessageCount(bool onlyOnce);


// #if Headline
//     [DllImport ("__Internal")]
//     private static extern void startRecording();
//     [DllImport ("__Internal")]
//     private static extern void pauseRecording();
//     [DllImport ("__Internal")]
//     private static extern void continueRecording();
//     [DllImport ("__Internal")]
//     private static extern void stopRecording();

//     [DllImport ("__Internal")]
//     private static extern void editVideo(string path);
//     [DllImport ("__Internal")]
//     private static extern void sharePhotos(string photoIdentifiers);
//     [DllImport ("__Internal")]
//     private static extern void shareVideos(string photoIdentifiers);
//     [DllImport ("__Internal")]
//     private static extern void shareRecentRecordVideo();
//     [DllImport ("__Internal")]
//     private static extern void showPromotion(float x, float y);

//     [DllImport("__Internal")]
//     private static extern void requestPhotoWritePermission ();
//     [DllImport("__Internal")]
//     private static extern bool hasPhotoWritePermission ();
//     [DllImport("__Internal")]
//     private static extern void openSettingPage ();
//     [DllImport("__Internal")]
//     private static extern bool isRecording();

// #endif

// #if CN_IOS_Normal
//     [DllImport("__Internal")]
//     private static extern void cloudFunc(string funcKey, string data);
    
//     [DllImport("__Internal")]
//     private static extern void cloudFuncWithoutLogin(string funcKey, string data);
    
//     [DllImport("__Internal")]
//     private static extern void adultVerify(string name,string idCard);
    
//     [DllImport("__Internal")]
//     private static extern int getNotchHeight();
    
// #endif

//     private static RiseSdk _instance = null;
//     private static bool hasInit = false;
// #if UNITY_EDITOR
//     public static bool hasRewardAd = true;
//     public static int remoteConfigValue = 1;
// #endif
//     public FullAdType lastFullAdType = FullAdType.none;
//     public static RiseSdk Instance
//     {
//         get
//         {
//             if (null == _instance)
//                 _instance = new RiseSdk();
//             return _instance;
//         }
//     }

//     //初始化时注册ATT监听回调（每次启动App的时候返回状态）
//     //private bool isOpenPermission = false;
//     public void Init()
//     {
//         RiseEditorAd.hasInit = true;
//         if (hasInit)
//         {
//             return;
//         }
//         hasInit = true;
//         RiseSdkListener.Instance.enabled = true;

// #if !UNITY_EDITOR
//         Create();
// #endif
//         //RiseSdkListener.OnAttAccepted -= OnAttAcceptedStatus;
//         //RiseSdkListener.OnAttAccepted += OnAttAcceptedStatus;
//         // lruCache = new FileLRUCache(20);
//     }
    
//     //private void OnAttAcceptedStatus(bool status)
//     //{
//     //    LogX($"hxctestIOS_SDK_OnAttAcceptedStatus:{status}");
//     //    isOpenPermission = status;
//     //}

//     //public bool GetAttAcceptedStatus()
//     //{
//     //    return isOpenPermission;
//     //}

// #if Headline
//     public  bool IsRecording()
//     {
// #if UNITY_EDITOR
// 	    return false;
// #else
//         return isRecording();
// #endif
//     }

//     public  void RequestPhotoWritePermission()
// 	{
// #if UNITY_EDITOR
// 	    return;
// #else
// 	   requestPhotoWritePermission ();
// #endif
// 	}

//     public  bool HasPhotoWritePermission ()
// 	{
// #if UNITY_EDITOR
// 	    return true;
// #else
// 	    return hasPhotoWritePermission ();
// #endif
// 	}

//     public void OpenSettingPage ()
// 	{
// #if UNITY_EDITOR
//         return;
// #else
// 		openSettingPage ();
// #endif
// 	}

//     #region 分享
//     /// <summary>
//     /// 抖音图片分享
//     /// DouYin the picture share.
//     /// </summary>
//     public void DyPictureShare(string pictureFileName)
//     {
// #if UNITY_EDITOR

// #else
//         sharePhotos (pictureFileName);
// #endif
//     }

//     /// <summary>
//     /// 抖音视频分享
//     /// DouYin the video share.
//     /// </summary>
//     public void DyVideoShare(string videoFileName)
//     {
// #if UNITY_EDITOR

// #else
//         shareVideos (videoFileName);
// #endif
//     }

//     public void ShareRecentRecordVideo()
//     {
// #if UNITY_EDITOR

// #else
//         shareRecentRecordVideo ();
// #endif
//     }
//         /// <summary>
//     /// 智能视频编辑和抖音分享
//     /// AI video editor and douyin share.
//     /// </summary>
//     public void AIEditorVideoShare(string fileName)
//     {
// #if UNITY_EDITOR

// #else
//         editVideo (fileName);
// #endif
//     }
//     #endregion

//     #region 录屏

//     /**
//  * 是否支持录屏
//  * 1.系统API 小于 21
//  * 2.CPU 频率小于1700MHZ
//  * 3.运行内存小于2048MB
//  *
//  * @param context context
//  * @return 若支持 返回 {@link ScreenRecordResult#ERRNO_SUCCESS}
//  */
//     public bool CheckSupScreenRecord()
//     {
//          return false;
//     }

//     /// <summary>
//     /// 开始录屏
//     /// Screen record start.
//     /// </summary>
//     public void ScreenRecordStart()
//     {
// #if UNITY_EDITOR
// #else
//      startRecording ();
// #endif
//     }

//     /// <summary>
//     /// 暂停录屏
//     /// Screen record pause.
//     /// </summary>
//     public void ScreenRecordPause()
//     {
// #if UNITY_EDITOR

// #else
//        // pauseRecording ();
// #endif
//     }

//     /// <summary>
//     /// 继续录屏
//     /// Screen record resume.
//     /// </summary>
//     public void ScreenRecordResume()
//     {
// #if UNITY_EDITOR

// #else
//       // continueRecording ();
// #endif
//     }

//     /// <summary>
//     /// 停止录屏
//     /// Screen record stop.
//     /// </summary>
//     public void ScreenRecordStop(bool autoEdit)
//     {
// #if UNITY_EDITOR

// #else
//         stopRecording();
// #endif
//     }
//     #endregion

//     public int Getrecordtime(int defaultValue = 30)
// 	{
// #if UNITY_EDITOR
// 	    return defaultValue;
// #else
// 	    int  Value = getRemoteConfigIntValue("recordtime");
//         if(Value!=null&&Value>0)
//             return Value;
// else
//         return defaultValue;
// #endif
// 	}

//          /// <summary>
//     /// 获取AB测试的配置
//     /// Gets the ab config.
//     /// </summary>
//     /// <returns>The ab config.</returns>
//     /// <param name="key">Key.</param>
//     /// <param name="defaultValue">Default value.</param>
//     /// <typeparam name="T">The 1st type parameter 类型，需是JSON兼容的数据类型.</typeparam>
//     public string GetAbConfig(string key, string defaultValue)
//     {
// #if UNITY_EDITOR
//         return defaultValue;
// #else
//         return getRemoteConfigStringValue (key);
// #endif
//     }
// #endif

//     /// <summary>
//     /// 通过注册方式获取通知权限开关状态(onNotificationOpen(bool isOpen))
//     /// </summary>
//     public void QueryPushNotification()
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("RiseSdk call QueryPushNotification!");
//         return;
// #endif
//         queryPushNotification();
//     }

//     /// <summary>
//     /// 通过注册方式获取ATT开关状态(OnAttAccepted(bool isOpen))
//     /// </summary>
//     public void RequestIDFA()
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("RiseSdk call RequestIDFA!");
//         return;
// #endif
//         requestIDFA();
//     }

//     /// <summary>
//     /// 跳转系统设置界面
//     /// </summary>
//     public void OpenApplicationSetting()
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("RiseSdk call OpenApplicationSetting");
//         return;
// #endif
//         openApplicationSetting();
//     }

//     public int GetScreenWidth()
//     {
// #if UNITY_EDITOR
//         return Screen.width;
// #else
//         return getScreenWidth ();
// #endif
//     }

//     public int GetScreenHeight()
//     {
// #if UNITY_EDITOR
//         return Screen.height;
// #else
//         return getScreenHeight ();
// #endif
//     }

//     // 展示系统loading界面
//     public void ShowProgressBar()
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("showProgressBar");
//         return;
// #endif
//         showProgressBar();
//     }

//     // 隐藏系统loading界面
//     public void HideProgressBar()
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("hideProgressBar");
//         return;
// #endif
//         hideProgressBar();
//     }

//     public int GetScreenDesignWidth()
//     {
// #if UNITY_EDITOR
//         return Screen.width;
// #else
//         return getScreenDesignWidth ();
// #endif
//     }

//     public int GetScreenDesignHeight()
//     {
// #if UNITY_EDITOR
//         return Screen.height;
// #else
//         return getScreenDesignHeight ();
// #endif
//     }

//     public bool IsIPhoneX()
//     {
// #if UNITY_EDITOR
//         return false;
// #else
//         return isIPhoneX ();
// #endif
//     }

//     public void ShowBanner(int pos)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.ShowBanner(pos);
// #else
//         showBanner (pos);
// #endif
//     }

//     public void ShowBanner(string tag, int pos)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.ShowBanner(tag, pos);
// #else
//         showBannerWithTag (tag, pos);
// #endif
//     }

//     public void CloseBanner()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.CloseBanner();
// #else
//         closeBanner ();
// #endif
//     }

//     public bool HasBanner()
//     {
// #if UNITY_EDITOR
//         return true;
// #else
//         return isBannerAvailable ();
// #endif
//     }

//     public bool HasBanner(string tag)
//     {
// #if UNITY_EDITOR
//         return true;
// #else
//         return isBannerAvailableWithTag (tag);
// #endif
//     }

//     public bool HasRewardAd()
//     {
// #if UNITY_EDITOR
//         return hasRewardAd;
// #else
//         return isVideoAvailable ();
// #endif
//     }

//     public bool HasRewardAd(string tag)
//     {
// #if UNITY_EDITOR
//         return hasRewardAd;
// #else
//         return isVideoAvailableWithTag (tag);
// #endif
//     }

//     public void ShowRewardAd(int placementId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.ShowRewardAd(placementId);
// #else
//         showRewardVideo (placementId);
// #endif
//     }

//     public void ShowRewardAd(string tag, int placementId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.ShowRewardAd(tag, placementId);
// #else
//         showRewardVideoWithTag (tag, placementId);
// #endif
//     }

//     public bool HasInterstitial(string tag)
//     {
// #if UNITY_EDITOR
//         return true;
// #else
//         return isInterstitialAvailable (tag);
// #endif
//     }


//     public bool HasRewardedInterstitial()
//     {
// #if UNITY_EDITOR
//         return true;
// #else
//        return hasRewardedInterstitial ();   
// #endif
//     }

//     public void ShowRewardedInterstitial(int rewardId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.ShowRewardAd(rewardId);
// #endif
//         // showRewardedInterstitial(rewardId);
//     }

//     public void ShowRewardedInterstitial(string tag, int rewardId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.ShowRewardAd(rewardId);
// #endif

//         // showRewardedInterstitial(tag, rewardId);
//     }


//     // club chat
//     public void ClubSendChat(string leagueID, string json)
//     {
// #if UNITY_EDITOR
//         Debug.Log("Club send chat!" + json);
// #endif
//         sendChat("https://mergeelves-chats.firebaseio.com/", "chats/" + leagueID, json);
//     }

//         // 加入联盟后，监听聊天前调用一次
//     public void ClubListenChat(string leagueID)
//     {
// #if UNITY_EDITOR
//         Debug.Log("ClubListenChat!" + leagueID);
// #endif
//         listen("https://mergeelves-chats.firebaseio.com/", "chats/" + leagueID);
//     }


//     //firestore cloud
//     public void InitFirestoreAfterSignIn(string provider)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[InitFirestoreAfterSignIn] Start!");
//         return;
// #endif
//         initFirestoreAfterSignIn(provider);
//     }

//     public void InitializeAndLinkGoogleAfterSignInFacebook()
//     {
// #if UNITY_EDITOR
//         Debug.Log("[InitializeAndLinkGoogleAfterSignInFacebook] Start!");
// #endif
//         initializeAndLinkGoogleAfterSignInFacebook();
//     }

//     public void LoginAppleBeforeFirestore(bool isForce)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[LoginAppleBeforeFirestore] Start!");
//         RiseSdkListener.Instance.signInAppleSuccess("");
//         return;
// #endif
//         loginAppleBeforeFirestore(isForce);
//     }

//     public void LinkFirestoreAfterAppleSignIn()
//     {
// #if UNITY_EDITOR
//         Debug.Log("[LinkFirestoreAfterAppleSignIn] Start!");
//         return;
// #endif
//         linkFirestoreAfterAppleSignIn();
//     }

//     public void ReadFirestore(string collectionName)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[ReadFirestore] collectionName:" + collectionName);
// #endif
//         readFirestore(collectionName);
//     }

//     public void ReadFirestore(string collectionName, string docementId)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[ReadFirestore] collectionName:" + collectionName);
// #endif
//         readFirestore2(collectionName, docementId);
//     }

//     public void MergeFirestore(string collectionName, string jsonData)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[MergeFirestore] collectionName:" + collectionName + " jsonData:" + jsonData);
// #endif
//         mergeFirestore(collectionName, jsonData);
//     }


//     public void SetFirestore(string collectionName, string jsonData)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[SetFirestore] collectionName:" + collectionName + " jsonData:" + jsonData);
// #endif
//         setFirestore(collectionName, jsonData);
//     }

//     public void UpdateFirestore(string collectionName, string translationId, string jsonData)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[UpdateFirestore] collectionName:" + collectionName + " jsonData:" + jsonData);
// #endif
//         updateFirestore(collectionName, translationId, jsonData);
//     }

//     public void DeleteFirestore(string collectionName)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[DeleteFirestore] collectionName:" + collectionName);
// #endif
//         deleteFirestore(collectionName);
//     }

//     public void SnapshotFirestore(string collectionName)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[snapshotFirestore] collectionName:" + collectionName);
// #endif
//         snapshotFirestore(collectionName);
//     }

//     public void SignOutFirestore()
//     {
// #if UNITY_EDITOR
//         Debug.Log("[SignOutFirestore] enter!");
// #endif
//         signOutFirestore();
//     }

//     public string GetFirebaseUserId()
//     {
// #if UNITY_EDITOR
//         return "";
// #endif
//         return getFirebaseUserId();
//     }

//     public void CloudFunction(string funcName)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[CloudFunction] funcName:" + funcName);
// #endif
//         cloudfunction(funcName);
//     }

//     public void CloudFunction(string funcName, string funcParamsJson)
//     {
// #if UNITY_EDITOR
//         Debug.Log($"[CloudFunction] funcName:{funcName}; funcParamsJson:{funcParamsJson}");
// #endif
//         cloudfunction2(funcName, funcParamsJson);
//     }

//     //追踪用户打开窗体的时间
//     public void TrackScreen(string screenClass, string screenName)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[TrackScreen] screenClass:" + screenClass + " screenName:" + screenName);
// #endif
//         trackScreen(screenClass, screenName);
//     }


//     public void CopyText(string str)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[CopyText] str:" + str);
// #endif
//         copyText(str);
//     }

//     public void PlayerFinder()
//     {
// #if UNITY_EDITOR
//         Debug.Log("[PlayerFinder] start!!");
// #endif
//         // playerFinder();
//     }

//     public void InitializeAndLinkFacebookAfterSignIn()
//     {
// #if UNITY_EDITOR
//         Debug.Log("[InitializeAndLinkFacebookAfterSignIn] enter!!");
//         return;
// #endif
//         initializeAndLinkFacebookAfterSignIn();
//     }

//     public void LogoutFacebook()
//     {
// #if UNITY_EDITOR
//         Debug.Log("[LogoutFacebook] enter!!");
// #endif
//         logoutFacebook();
//     }

//     public string GetPushToken()
//     {
// #if UNITY_EDITOR
//         return "Editor_Ios";
// #endif
//         // return getPushToken();
//         return null;
//     }

//     public void MMKVWithId(string collectionName)
//     {
//         // mmkvWithId(collectionName);
//     }

//     public void MMRemoveKey(string key)
//     {
//         mmRemoveKey(key);
//     }

//     public void MMRemoveKey2(string collectionName,string key)
//     {
//         // mmRemoveKey2(collectionName,key);
//     }

//     public bool MMContainsKey(string key)
//     {
//         return mmContainsKey(key);     
//     }

//     public bool MMContainsKey2(string collectionName,string key)
//     {
//         // return mmContainsKey2(collectionName,key);     
//         return false;
//     }

//     public void MMSetStringValue(string key, string value)
//     {
//         mmSetStringValue(key, value);
//     }

//     public void MMSetStringValue2(string collectionName,string key, string value)
//     {
//         // mmSetStringValue2(collectionName,key, value);
//     }

//     public string MMGetStringValue(string key, string defaultValue)
//     {
//         return mmGetStringValue(key, defaultValue);
//     }

//     public string MMGetStringValue2(string collectionName,string key, string defaultValue)
//     {
//         // return mmGetStringValue2(collectionName,key, defaultValue);
//         return null;
//     }

//     public void MMSetIntValue(string key, int value)
//     {
//         mmSetIntValue(key, value);
//     }

//     public void MMSetIntValue2(string collectionName,string key, int value)
//     {
//         // mmSetIntValue2(collectionName,key, value);
//     }

//     public int MMGetIntValue(string key, int defaultValue)
//     {
//         return mmGetIntValue(key, defaultValue);
//     }

//     public int MMGetIntValue2(string collectionName,string key, int defaultValue)
//     {
//         // return mmGetIntValue2(collectionName,key, defaultValue);
//         return 0;
//     }

//     public void MMSetLongValue(string key, long value)
//     {
//         mmSetLongValue(key, value);
//     }

//     public void MMSetLongValue2(string collectionName,string key, long value)
//     {
//         // mmSetLongValue2(collectionName,key, value);
//     }

//     public long MMGetLongValue(string key, long defaultValue)
//     {
//         return mmGetLongValue(key, defaultValue);
//     }

//     public long MMGetLongValue2(string collectionName,string key, long defaultValue)
//     {
//         // return mmGetLongValue2(collectionName,key, defaultValue);
//         return 0L;
//     }

//     public void MMSetBoolValue(string key, bool value)
//     {
//         mmSetBoolValue(key, value);
//     }

//     public void MMSetBoolValue2(string collectionName,string key, bool value)
//     {
//         // mmSetBoolValue2(collectionName,key, value);
//     }

//     public bool MMGetBoolValue(string key, bool defaultValue)
//     {
//         return mmGetBoolValue(key, defaultValue);
//     }

//     public bool MMGetBoolValue2(string collectionName,string key, bool defaultValue)
//     {
//         // return mmGetBoolValue2(collectionName,key, defaultValue);
//         return false;
//     }



//     public void SaveUserAttribute(string dataJsonString)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[SaveUserAttribute] enter!!");
// #endif
//         saveUserAttribute(dataJsonString);
//     }

//     public void SubscribeFCMTopic(string saveKey, string newTopic)
//     {
// #if UNITY_EDITOR
//         Debug.Log("[SubscribeFCMTopic] enter!!");
// #endif
//         // subscribeFCMTopic(saveKey, newTopic);
//     }

// //     public void AppFeedback(string sectionUrl, string systemInfo)
// //     {
// // #if UNITY_EDITOR
// //         Debug.Log("[AppFeedback] enter!!");
// // #endif
// //         // appFeedback(sectionUrl, systemInfo);
// //     }

//     public void HelpEngagement(string uid, string userInfoJson) {
// #if UNITY_EDITOR
//         Debug.Log("[HelpEngagement] enter, uid:::" + uid + ", info:::" + userInfoJson);
// #endif
//         displayHelpEngagement(uid, userInfoJson);
//     }

//     public void LogError(string errorMessage)
//     {
// #if UNITY_EDITOR

// #endif
//         // logIvyError(errorMessage);
//     }

//     public void OnKill()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.OnExit();
// #endif
//     }

//     /// <summary>
//     /// 检测计费点是否存在。
//     /// </summary>
//     /// <param name="billingId">计费点id</param>
//     public void HasPaid(int billingId)
//     {

//     }

//     //打开本地图片
//     public void OpenAlbum()
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("OpenAlbum success");
// #else

// #endif
//     }

//     public void OpenPhoto()
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("OpenPhoto success");
// #else

// #endif
//     }

//     public void InAppReview()
//     {
// #if UNITY_EDITOR
//         Debug.Log("InAppReview");

// #else
//         inAppReview();
// #endif
//     }

//     //存取 save
//     public void WriteSavedGame(string name, string data)
//     {
// #if UNITY_EDITOR
//         //RiseEditorAd.EditorAdInstance.Toast("CloseDeliciousBannerAd");
// #endif

//     }


//     /**
//      * 展示Google 游戏存档界面 
//      */
//     public void ShowSavedGameUI()
//     {
// #if UNITY_EDITOR
// #endif

//     }


//     public void ShowAd(string tag)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.ShowAd(tag);
// #else
//         showInterstitialAd (tag);
// #endif
//     }

//     public void ShowAd(string tag, FullAdType adType)
//     {
//         this.lastFullAdType = adType;
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.ShowAd(tag);
// #else
//         showInterstitialAd (tag);
// #endif
//     }

//     public void LoadAd(string tag)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("LoadAd: " + tag);
// #else
//         loadInterstitialAd (tag);
// #endif
//     }

//     public void ShowAd(string tag, int delayShowSeconds)
//     {
// #if UNITY_EDITOR
//         RiseSdkListener.Instance.StartCoroutine(delayStart((string t) => RiseEditorAd.EditorAdInstance.ShowAd(t), tag, delayShowSeconds));
// #else
//         showInterstitialAdWithTag (tag, delayShowSeconds);
// #endif
//     }

//     public void ShowAd(string tag, int delayShowSeconds, double delayTimeInterval)
//     {
// #if UNITY_EDITOR
//         RiseSdkListener.Instance.StartCoroutine(delayStart((string t) => RiseEditorAd.EditorAdInstance.ShowAd(t), tag, (float)(delayShowSeconds * delayTimeInterval)));
// #else
//         showInterstitialAdWithTag2 (tag, delayShowSeconds, delayTimeInterval);
// #endif
//     }

//     private IEnumerator delayStart(Action<string> action, string tag, float delaySeconds)
//     {
//         if (delaySeconds > 0)
//         {
//             yield return new WaitForSeconds(delaySeconds);
//         }
//         if (action != null)
//         {
//             action(tag);
//         }
//         yield return null;
//     }

//     public void ShowIconAd(float width, float xPercent, float yPercent)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.ShowIconAd(width, xPercent, yPercent);
// #else
//         showIconAd (width, xPercent, yPercent);
// #endif
//     }

//     public void ShowPopupIconAd()
//     {
// #if UNITY_EDITOR
// #else
//         showPopupIconAds ();
// #endif
//     }

//     public bool HasNativeAd(string tag)
//     {
// #if UNITY_EDITOR
//         return true;
// #else
//         return isNativeAvailable (tag);
// #endif
//     }

//     public void ShowNativeAd(string tag, float xPixel, float yPixel, string configJson)
//     {
// #if UNITY_EDITOR
// #else
//         showNativeAd (tag, xPixel, yPixel, configJson);
// #endif
//     }

//     public void ShowNativeAdWithFrame(string tag, float x, float y, float width, float height, string configJson)
//     {
// #if UNITY_EDITOR
// #else
//         showNativeAdWithFrame (tag, x, y, width, height, configJson);
// #endif
//     }

//     public void CloseNativeAd(string tag)
//     {
// #if UNITY_EDITOR
// #else
//         closeNativeAd(tag);
// #endif
//     }

//     public void CloseIconAd()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.CloseIconAd();
// #else
//         closeIconAd ();
// #endif
//     }

//     public void ShowDeliciousIconAd(float x, float y, float w, float h, string configJson)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("ShowDeliciousIconAd");
// #else
//         showDeliciousIconAd (x, y, w, h, configJson);
// #endif
//     }

//     public void CloseDeliciousIconAd()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("CloseDeliciousIconAd");
// #else
//         closeDeliciousIconAd ();
// #endif
//     }

//     public void ShowDeliciousBannerAd(float x, float y, float w, float h, string configJson)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("ShowDeliciousBannerAd");
// #else
//         showDeliciousBannerAd (x, y, w, h, configJson);
// #endif
//     }

//     public void CloseDeliciousBannerAd()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("CloseDeliciousBannerAd");
// #else
//         closeDeliciousBannerAd ();
// #endif
//     }

//     public void ShowDeliciousVideoAd(string configJson)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("ShowDeliciousVideoAd");
// #else
//         showDeliciousInterstitialAd (configJson);
// #endif
//     }

//     public bool HasDeliciousAd()
//     {
// #if !UNITY_EDITOR
//         return isDeliciousAdAvailable ();
// #endif
//         return false;
//     }

//     public void Rate()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Rate");
// #else
//         rateUs ();
// #endif
//     }

//     public void RateInApp()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Rate");
// #else
//         rateInApp ();
// #endif
//     }

//     public void RateWithStar(float star)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("RateWithStar, star: " + star);
// #else
//         rateUsWithStar (star);
// #endif
//     }

//     public void RateInAppWithStar(float star)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("RateInAppWithStar, star: " + star);
// #else
//         rateInAppWithStar (star);
// #endif
//     }

//     public bool IsNetworkConnected()
//     {
// #if UNITY_EDITOR
//         return true;
// #else
//         return isNetworkAvailable ();
// #endif
//     }

//     public void Pay(int billingId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Pay(billingId);
// #else
//         pay (billingId);
// #endif
//     }

//     public void PayWithPayload(int billingId, string payloadData)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.PayWithPayload(billingId, payloadData);
// #else
//         payWithPayload (billingId, payloadData);
// #endif
//     }

//     public string GetAppStoreVersion()
//     {
// #if PLATFORM_IOS || UNITY_IOS
//         return getAppstoreVersion();
// #elif PLATFORM_ANDROID
//         return "";
// #else
//         return "E";
// #endif
//     }

//     public void CheckSubscriptionActive()
//     {
// #if UNITY_EDITOR
// #if UNITY_IOS
// #else
//         RiseSdkListener.Instance.onCheckSubscriptionResult ("CheckSubscriptionActive");
// #endif
// #else
//         isSubscriptionActive ();
// #endif
//     }

//     public int[] GetPurchasedIds()
//     {
// #if UNITY_EDITOR
//         return new int[] { 1, 2, 3 };
// #else
//         return getPurchasedIds ();
// #endif
//     }

//     public void ClearPurchasedIds()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("ClearPurchasedIds");
// #else
//         clearPurchasedIds ();
// #endif
//     }

//     public void ClearPurchasedId(int billingId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("ClearPurchasedId, " + billingId);
// #else
//         clearPurchasedId (billingId);
// #endif
//     }

//     public bool IsAdsEnabled()
//     {
// #if UNITY_EDITOR
//         return true;
// #else
//         return isAdsEnabled ();
// #endif
//     }

//     public void SetAdsEnable(bool enable)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("SetAdsEnable: " + enable);
// #else
//         setAdsEnable (enable);
// #endif
//     }

//     /// <summary>
//     /// 判断是否刘海屏
//     /// </summary>
//     /// <returns></returns>
//     public bool HasNotch()
//     {
// #if UNITY_EDITOR
//         return false;
// #else
// #if DEBUG_LOG
//         Debug.LogError("czy-,- Sdk HasNotch :" + hasNotch()); 
// #endif
//         return hasNotch ();
// #endif
//     }

//     /// <summary>
//     /// 启用实名认证V
//     /// </summary>
//     /// <returns></returns>
//     public void VerifyIdCard()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("VerifyIdCard");
// #else
//         verifyIdCard ();
// #endif
//     }

//     /// <summary>
//     /// 判断用户是不是属于欧盟区
//     /// </summary>
//     /// <returns></returns>
//     public bool HasGDPR()
//     {
// #if UNITY_EDITOR
//         return true;
// #else
//         return hasGdpr ();
// #endif
//     }

//     /// <summary>
//     /// 显示广告设置界面
//     /// </summary>
//     public void ResetGDPR()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("ResetGDPR");
// #else
//         resetGdpr ();
// #endif
//     }

//     public void RestorePayments()
//     {
// #if UNITY_EDITOR
// #if UNITY_IOS
// #else
//         RiseSdkListener.Instance.onRestoreSuccess ("1");
// #endif
//         RiseEditorAd.EditorAdInstance.Toast("RestorePayments");
// #else
//         restorePayments ();
// #endif
//     }

//     public string GetPaymentDatas()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("GetPaymentDatas");
//         return "{}";
// #else
//         return getPaymentDatas ();
// #endif
//     }

//     public string GetPaymentData(int billingId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("GetPaymentData");
//         return "{}";
// #else
//         return getPaymentData (billingId);
// #endif
//     }

//     public string GetPopupIconAdsData()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("GetPopupIconAdsData");
//         return "";
// #else
//         return getPopupIconAdsData();
// #endif
//     }

//     public string GetExtraData()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("GetExtraData");
//         return "";
// #else
//         return getExtraData ();
// #endif
//     }

//     public string GetPushData()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("GetPushData");
//         return "{}";
// #else
//         return getPushData ();
// #endif
//     }


//     public void GetPaymentDataAsyn(int billingId)
//     {
// #if UNITY_EDITOR
//         //return "{}";
// #else
//         // getPaymentDataAsyn(billingId);
// #endif
//     }

//     public void GetPaymentDatasAsyn(string billingIds)
//     {
// #if UNITY_EDITOR
//         //return "{}";
// #else
//         // getPaymentDatasAsyn(billingIds);
// #endif
//     }



//     public string GetConfig(int configId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("GetConfig, configId: " + configId);
//         return "_";
// #else
//         return getConfig (configId);
// #endif
//     }

//     public void LoginGameCenter()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("LoginGameCenter");
//         RiseSdkListener.Instance.onGameCenterLoginSuccess("");
// #else
//         loginGameCenter();
// #endif
//     }
// #if !CN_IOS_Normal
//     public void Login()
//     {

// #if UNITY_EDITOR
//                 RiseEditorAd.EditorAdInstance.Toast("Login");
// #else
//                 login();
// #endif
//     }
// #endif
//     public void IsLogin()
//     {
// #if UNITY_EDITOR
//         RiseSdkListener.Instance.onWXPreLogState("0");
//         return;
// #else
//           isLogin();
//           return;
// #endif
//     }

//     public void Logout()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Logout");
// #else
//         logout ();
// #endif
//     }

//     public string GetMeFirstName()
//     {
// #if UNITY_EDITOR
//         return "FirstName";
// #else
//         return meFirstName ();
// #endif
//     }

//     public string GetMeLastName()
//     {
// #if UNITY_EDITOR
//         return "LastName";
// #else
//         return meLastName ();
// #endif
//     }

//     public string GetMeId()
//     {
// #if UNITY_EDITOR
//         return "12345678";
// #else
//         return meId ();
// #endif
//     }

//     public string GetMeName()
//     {
// #if UNITY_EDITOR
//         return "12345678";
// #else
//         return meName ();
// #endif
//     }
// #if !CN_IOS_Normal
//     public string Me()
//         {
// #if UNITY_EDITOR
//             return "{\"id\":\"12345678\", \"name\":\"hahaha\", \"picture\":\"http://img.qq1234.org/uploads/allimg/141205/3_141205195713_3.jpg\"}";
// #else
//             return me ();
// #endif
//         }
// #endif

//     public string GetFriends()
//     {
// #if UNITY_EDITOR
//         return "[]";
// #else
//         return friends ();
// #endif
//     }

//     public string GetMePictureURL()
//     {
// #if UNITY_EDITOR
//         return "http://img.qq1234.org/uploads/allimg/141205/3_141205195713_3.jpg";
// #else
//         return mePictureURL ();
// #endif
//     }

//     public void FetchFriends(bool invitable)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("FetchFriends");
// #else
//         fetchFriends (invitable);
// #endif
//     }

//     public void FetchScores()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("FetchScores");
// #else
//         FetchScores ();
// #endif
//     }

//     public void Invite()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Invite");
// #else
//         invite ();
// #endif
//     }

//     public void Share(string contentURL, string tag, string quote)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Share: " + contentURL + ", " + tag + ", " + quote);
// #else
//         shareContent (contentURL, tag, quote);
// #endif
//     }

//     public void Share()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Share");
// #else
//         share ();
// #endif
//     }

//     public const string ShareHashTag = "#MergeElves";
//     public void ShareWithURL(string shareID, string url, string quote, string hashTag = ShareHashTag)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("ShareWithURL");
// #else
//         // share2(shareID, url, quote, hashTag);
// #endif
//     }

//     public void Toast(string message)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("toast: " + message);
// #else
//         toast (message);
// #endif
//     }
//     public void DisplayUrl(string title, string url)
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning($"[DisplayUrl]title:{title},url:{url}");
// #endif
//         displayUrl(title, url);
//     }

//     public void SdkLog(string message)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("sdk log: " + message);
// #else
//         sdklog (message);
// #endif
//     }

//     public void TrackEvent(string category, string keyValueData)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Track: " + category + "\n" + keyValueData);
// #else
//         logEventWithData (category, keyValueData);
// #endif
//     }

//     public void TrackIvyEvent(string category, string keyValueData)
//     {
// #if TrackEventToast
//             Debug.LogError("ClickHouse:::" + category + ":::\n" + keyValueData);
//             Toast(category + ":::" + keyValueData); 
// #endif
// #if UNITY_EDITOR
//                 //RiseEditorAd.EditorAdInstance.Toast ("Track: " + category + ", keyValueData: " + keyValueData);
// #else
//              logEventWithData (category, keyValueData);
// #endif
//     }

//     public void TrackEvent(string category, string action, string label, int value)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Track: " + category + ", action: " + action + ", label: " + label + ", value: " + value);
// #else
//         logEventLikeGA (category, action, label, value);
// #endif
//     }

//     public void TrackFinishLevel(string level)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("TrackFinishLevel: " + level);
// #else
//         logFinishLevel (level);
// #endif
//     }

//     public void TrackFinishAchievement(string achievement)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("TrackFinishAchievement: " + achievement);
// #else
//         logFinishAchievement (achievement);
// #endif
//     }

//     public void TrackFinishTutorial(string tutorial)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("TrackFinishTutorial: " + tutorial);
// #else
//         logFinishTutorial (tutorial);
// #endif
//     }

//     public void OnEventUpdateLevel(int level)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Umeng, setPlayerLevel: " + level);
// #else
//         logPlayerLevel (level);
// #endif
//     }

//     #region Umeng
//     public void UM_onEvent(String eventId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Umeng, onEvent: " + eventId);
// #else
//         logEvent (eventId);
// #endif
//     }

//     public void UM_onEvent(String eventId, String eventLabel)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Umeng, onEvent: " + eventId + ", " + eventLabel);
// #else
//         logEventWithTag (eventId, eventLabel);
// #endif
//     }

//     public void UM_onPageStart(String pageName)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Umeng, onPageStart: " + pageName);
// #else
//         logPageStart (pageName);
// #endif
//     }

//     public void UM_onPageEnd(String pageName)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Umeng, onPageEnd: " + pageName);
// #else
//         logPageEnd (pageName);
// #endif
//     }

//     public void UM_startLevel(String level)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Umeng, startLevel: " + level);
// #else
//         logStartLevel (level);
// #endif
//     }

//     public void UM_failLevel(String level)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Umeng, failLevel: " + level);
// #else
//         logFailLevel (level);
// #endif
//     }

//     public void UM_finishLevel(String level)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Umeng, finishLevel: " + level);
// #else
//         logFinishLevel (level);
// #endif
//     }

//     public void UM_buy(String itemName, int count, double price)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Umeng, buy, item: " + itemName + ", count: " + count + ", price: " + price);
// #else
//         logBuy (itemName, count, price);
// #endif
//     }

//     public void UM_use(String itemName, int number, double price)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Umeng, use, item: " + itemName + ", number: " + number + ", price: " + price);
// #else
//         logUse (itemName, number, price);
// #endif
//     }

//     public void UM_bonus(String itemName, int number, double price, int trigger)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("Umeng, bonus, item: " + itemName + ", number: " + number + ", price: " + price + ", trigger: " + trigger);
// #else
//         logBonus (itemName, number, price, trigger);
// #endif
//     }
//     #endregion

//     #region GameCenter
//     public bool IsGameCenterAvailable()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("IsGameCenterAvailable");
//         return true;
// #else
//         return isGameCenterAvailable ();
// #endif
//     }

//     public void ShowLeaderboards()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("ShowLeaderboards");
// #else
//         showLeaderboards ();
// #endif
//     }

//     public void ShowLeaderboard(int leaderboardId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("ShowLeaderboard: " + leaderboardId);
// #else
//         showLeaderboard (leaderboardId);
// #endif
//     }

//     public void ShowAchievements()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("ShowAchievements");
// #else
//         showAchievements ();
// #endif
//     }

//     public void SubmitScore(int leaderboardId, long score)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("submitScore: " + leaderboardId + ", " + score);
// #else
//         submitScore (leaderboardId, score);
// #endif
//     }

//     public long GetMyHighScore(int leaderboardId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("GetMyHighScore: " + leaderboardId);
//         return 888;
// #else
//         return myHighScore (leaderboardId);
// #endif
//     }

//     public void SubmitAchievement(int achievementId, double percent)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("submitScore: " + achievementId + ", " + percent);
// #else
//         submitAchievement (achievementId, percent);
// #endif
//     }

//     public double GetAchievementProgress(int achievementId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("GetAchievementProgress: " + achievementId);
//         return 20;
// #else
//         return getAchievementProgress (achievementId);
// #endif
//     }
//     #endregion

//     public void SendMail(string address, string subject, string content, bool isHTML)
//     {
// #if !UNITY_EDITOR
//           sendMail (address, subject, content, isHTML);
// #endif
//     }

//     public bool JustShowFullAd()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("JustShowFullAd");
//         return false;
// #else
//         return justShowFullAd ();
// #endif
//     }

//     #region RemoteConfig
//     public int GetRemoteConfigInt(string remoteKey)
//     {
// #if UNITY_EDITOR
//         return remoteConfigValue;
// #else
//         return getRemoteConfigIntValue (remoteKey);
// #endif
//     }

//     public long GetRemoteConfigLong(string remoteKey)
//     {
// #if UNITY_EDITOR
//         return remoteConfigValue;
// #else
//         return getRemoteConfigLongValue (remoteKey);
// #endif
//     }

//     public double GetRemoteConfigDouble(string remoteKey)
//     {
// #if UNITY_EDITOR
//         return remoteConfigValue;
// #else
//         return getRemoteConfigDoubleValue (remoteKey);
// #endif
//     }

//     public bool GetRemoteConfigBoolean(string remoteKey)
//     {
// #if UNITY_EDITOR
//         return remoteConfigValue == 0 ? false : true;
// #else
//         return getRemoteConfigBoolValue (remoteKey);
// #endif
//     }

//     public string GetRemoteConfigString(string remoteKey)
//     {
// #if UNITY_EDITOR
//         return "";
// #else
//         return getRemoteConfigStringValue (remoteKey);
// #endif
//     }
//     #endregion

//     #region PushNotification
//     public void CancelRemoteNotification(string notiKey)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("CancelRemoteNotification");
// #else
//         cancelRemoteNotification (notiKey);
// #endif
//     }
//     public void CancelLocalNotification(string notiKey)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("CancelLocalNotification");
// #else
//         cancelLocalNotification (notiKey);
// #endif
//     }

//     public void CancelAllLocalNotifications()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("CancelAllLocalNotifications");
// #else
//         cancelAllLocalNotifications ();
// #endif
//     }

//     //返回自己传的userInfo json数据
//     public string GetLocalNotificationDataJson()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("GetLocalNotificationDataJson");
//         return "{}";
// #else
//         return getLocalNotificationDataJson ();
// #endif
//     }

//     /** 发送本地通知
//     @param key 本地通知表示，通知的id，同一个key重复发送的通知会覆盖前面的，在将来可以通过key来取消通知
//     @param title 本地通知的标题
//     @param msg 本地通知的内容
//     @param pushTime 如果小于当前时间戳，就按照延迟秒数来发送，如果是<=0就是立即发送，否则按照指定时间戳发送，时间戳精确到毫秒
//     @param interval 通知的循环周期，0表示不循环，可以设置按年，月，日，时，分循环
//     @param useSound 是否使用提示音
//     @param soundName特定提示音名字，需要打到包体中
//     @param userInfo 用户自定义数据，当点击通知打开应用的时候获取到
//     */
//     public void PushLocalNotification(string key, string title, string msg, long pushTime, int interval, bool useSound, string soundName, string userInfo, bool repeat
//  = false)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("PushLocalNotification");
// #else
//         pushLocalNotification (key, title, msg, pushTime, interval, repeat, useSound, soundName, userInfo);
// #endif
//     }

//     /** 发送本地通知
//      @param key 本地通知表示，在将来可以通过key来取消通知
//      @param title 本地通知的标题
//      @param msg 本地通知的内容
//      @param dateStr 通知触发日期，按照 yyyy-MM-dd HH:mm:ss 的时间格式定义
//      @param interval 通知的循环周期，0表示不循环，可以设置按年，月，日，时，分循环
//      @param useSound 是否使用提示音
//      @param soundName特定提示音名字，需要打到包体中
//      @param userInfo 用户自定义数据，当点击通知打开应用的时候获取到
//      */
//     public void PushLocalNotificationWithDateStr(string key, string title, string msg, string dateStr, int interval, bool useSound, string soundName, string userInfo, bool repeat
//  = false)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("PushLocalNotificationWithDateStr");
// #else
//         pushLocalNotificationWithDateStr (key, title, msg, dateStr, interval, repeat, useSound, soundName, userInfo);
// #endif
//     }

//     /// <summary>
//     /// 发送远程通知，给fb好友或者uuid设备或者特定主题的用户发送通知
//     /// </summary>
//     /// <param name="key">本地通知表示，通知的id，同一个key重复发送的通知会覆盖前面的，在将来可以通过key来取消通知</param>
//     /// <param name="title">标题</param>
//     /// <param name="content">内容</param>
//     /// <param name="pushTime">如果小于当前时间戳，就按照延迟秒数来发送，如果是小于等于0就是立即发送，否则按照指定时间戳发送，时间戳精确到毫秒</param>
//     /// <param name="localTimeZone">是否使用被推送用户本地时间</param>
//     /// <param name="fbIds">fb好友id，多个id使用逗号分隔</param>
//     /// <param name="uuids">sdk中获取的uid，多个id使用逗号分隔</param>
//     /// <param name="topics">主题</param>
//     /// <param name="iosBadge">iOS中icon右上的提示数字</param>
//     /// <param name="useSound">是否使用提示音</param>
//     /// <param name="soundName">特定提示音名字，需要打到包体中</param>
//     /// <param name="userInfo">用户自定义数据，当点击通知打开应用的时候获取到</param>
//     public void PushNotification(string key, string title, string content, long pushTime, bool localTimeZone, string fbIds, string uuids, string topics, int iosBadge, bool useSound, string soundName, string userInfo)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("PushNotification");
// #else
//         pushRemoteNotification (key, title, content, pushTime, localTimeZone, fbIds, uuids, topics, iosBadge, useSound, soundName, userInfo);
// #endif
//     }
//     #endregion

//     public void SaveBase64ImageToCameraRoll(string imagebase64)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("SaveBase64ImageToCameraRoll");
// #else
//         saveBase64ImageToCameraRoll(imagebase64);
// #endif
//     }
//     public void SetAdsUnderAgeMode(bool state)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("SetAdsUnderAgeMode " + state);
// #else
//         setAdsUnderAgeMode(state);
// #endif
//     }

//     public void SetUserProperty(string key, string value)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("SetUserProperty, key: " + key + ", value: " + value);
// #else
//         setUserPropertyString (key, value);
// #endif
//     }

//     public bool HasNewVersion()
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("HasNewVersion");
//         return false;
// #else
//         return hasNewVersion ();
// #endif
//         return false;
//     }
//     #region IOS_DUMMY
//     public void UM_onEventValue(string eventId, Dictionary<string, string> mapStr)
//     {
//     }

//     public void OnResume()
//     {
//     }

//     public void OnPause()
//     {
//     }

//     public void OnStart()
//     {
//     }

//     public void OnStop()
//     {
//     }

//     public void OnDestroy()
//     {
//     }

//     public void OnExit()
//     {
//     }

//     public void UM_pay(double money, String itemName, int number, double price)
//     {
//     }

//     public string EncodeParams(string dataStr)
//     {
//         return "";
//     }
//     public string GetConfig(string packageName, int configId)
//     {
// #if UNITY_EDITOR
//         RiseEditorAd.EditorAdInstance.Toast("GetConfig, packageName: " + packageName + ", configId: " + configId);
// #endif
//         return "";
//     }
//     #endregion


    
//     public bool ShowSignInWithApple(float x, float y, float w, float h, bool useBlackBackgrounds)
//     {
// #if UNITY_EDITOR
//         Debug.Log("ShowSignInWithApple");
//         return true;
// #elif UNITY_IOS || PLATFORM_IOS
//         return showSignInWithApple(x, y, w, h, true);
// #endif
//         return false;
//     }

//     public void HideSignInWithApple()
//     {
// #if UNITY_EDITOR
//         Debug.Log("HideSignInWithApple");
// #elif UNITY_IOS || PLATFORM_IOS
//         hideSignInWithApple();
// #endif
//     }

    
//      public void ShowAIHelp(String entranceId, String meta)
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("showAIHelp 2 params");
//         return;
// #endif
//         showAIHelp(entranceId, meta, null, null);
//     }
    
//     /// <summary>
//     /// AIHelp 是否已初始化
//     /// </summary>
//     /// <returns></returns>
//     public bool IsAIHelpInitialized()
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("init AIHelp");
//         return false;
// #endif
//         return isAIHelpInitialized();
//     }

//     /// <summary>
//     /// 获取未读消息数,每5分钟轮询一次
//     /// 监听 unreadMessageCount 获取返回值，参数 int
//     /// </summary>
//     /// <param name="onlyOnce">是否只获取一次</param>
//     public void LoadAIHelpUnreadMessageCount(bool onlyOnce)
//     {
// #if UNITY_EDITOR
//         Debug.LogWarning("LoadAIHelpUnreadMessageCount");
//         return;
// #endif
//         loadAIHelpUnreadMessageCount(onlyOnce);
//     }


// #endif

//     /// <summary>
//     /// 在左上角显示banner广告参数常量
//     /// </summary>
//     public const int POS_BANNER_LEFT_TOP = 1;

//     /// <summary>
//     /// 在顶部居中显示banner广告参数常量
//     /// </summary>
//     public const int POS_BANNER_MIDDLE_TOP = 3;

//     /// <summary>
//     /// 在右上角显示banner广告参数常量
//     /// </summary>
//     public const int POS_BANNER_RIGHT_TOP = 6;

//     /// <summary>
//     /// 在中间居中显示banner广告参数常量
//     /// </summary>
//     public const int POS_BANNER_MIDDLE_MIDDLE = 5;

//     /// <summary>
//     /// 在左下角显示banner广告参数常量
//     /// </summary>
//     public const int POS_BANNER_LEFT_BOTTOM = 2;

//     /// <summary>
//     /// 在底部居中显示banner广告参数常量
//     /// </summary>
//     public const int POS_BANNER_MIDDLE_BOTTOM = 4;

//     /// <summary>
//     /// 在右下角显示banner广告参数常量
//     /// </summary>
//     public const int POS_BANNER_RIGHT_BOTTOM = 7;

//     public const int POS_BANNER_LEFT_MIDDLE = 8;
//     public const int POS_BANNER_RIGHT_MIDDLE = 9;

//     public const int ANIMATE_BANNER_NONE = 0;
//     public const int ANIMATE_BANNER_TOP = 1;
//     public const int ANIMATE_BANNER_BOTTOM = 2;
//     public const int ANIMATE_BANNER_LEFT = 4;
//     public const int ANIMATE_BANNER_RIGHT = 8;
//     public const int ANIMATE_BANNER_ROTATION = 16;

//     /// <summary>
//     /// 游戏打开时显示大屏广告参数常量
//     /// </summary>
//     public const string M_START = "start";

//     /// <summary>
//     /// 暂停游戏时显示大屏广告参数常量
//     /// </summary>
//     public const string M_PAUSE = "pause";

//     /// <summary>
//     /// 游戏过关时显示大屏广告参数常量
//     /// </summary>
//     public const string M_PASSLEVEL = "passlevel";

//     public const string M_PASSLEVEL_1 = "passlevel1";

//     /// <summary>
//     /// 自定义时机显示大屏广告参数常量
//     /// </summary>
//     public const string M_CUSTOM = "custom";

//     /// <summary>
//     /// 计费成功标志常量
//     /// </summary>
//     public const int PAYMENT_RESULT_SUCCESS = 1;

//     /// <summary>
//     /// 计费失败标志常量
//     /// </summary>
//     public const int PAYMENT_RESULT_FAILS = 2;

//     /// <summary>
//     /// 计费取消标志常量
//     /// </summary>
//     public const int PAYMENT_RESULT_CANCEL = 3;

//     /// <summary>
//     /// 获取配置的AppId参数常量
//     /// </summary>
//     public const int CONFIG_KEY_APP_ID = 1;

//     /// <summary>
//     /// 获取配置的排行榜URL参数常量
//     /// </summary>
//     public const int CONFIG_KEY_LEADER_BOARD_URL = 2;

//     /// <summary>
//     /// 获取API Version参数常量
//     /// </summary>
//     public const int CONFIG_KEY_API_VERSION = 3;

//     /// <summary>
//     /// 获取本机屏幕宽度参数常量
//     /// </summary>
//     public const int CONFIG_KEY_SCREEN_WIDTH = 4;

//     /// <summary>
//     /// 获取本机屏幕高度参数常量
//     /// </summary>
//     public const int CONFIG_KEY_SCREEN_HEIGHT = 5;

//     /// <summary>
//     /// 获取本机语言参数常量
//     /// </summary>
//     public const int CONFIG_KEY_LANGUAGE = 6;

//     /// <summary>
//     /// 获取本机国家码参数常量
//     /// </summary>
//     public const int CONFIG_KEY_COUNTRY = 7;

//     /// <summary>
//     /// 获取应用的版本号参数常量
//     /// </summary>
//     public const int CONFIG_KEY_VERSION_CODE = 8;

//     /// <summary>
//     /// 获取应用的版本号名称参数常量
//     /// </summary>
//     public const int CONFIG_KEY_VERSION_NAME = 9;

//     /// <summary>
//     /// 获取应用的包名参数常量
//     /// </summary>
//     public const int CONFIG_KEY_PACKAGE_NAME = 10;

//     /// <summary>
//     /// 获取用户的UID（根据设备ID）参数常量
//     /// </summary>
//     public const int CONFIG_KEY_UUID = 11;

//     //其他广告类型
//     public const int ADTYPE_OTHER = -1;

//     //大屏广告类型
//     public const int ADTYPE_INTERTITIAL = 1;

//     //视频广告类型
//     public const int ADTYPE_VIDEO = 2;

//     //横幅广告类型
//     public const int ADTYPE_BANNER = 3;

//     //icon广告类型  ios has
//     public const int ADTYPE_ICON = 4;

//     //native广告类型
//     public const int ADTYPE_NATIVE = 5;


// #if Headline && UNITY_IOS
// public enum AdEventType : int
// {
// 	FullAdLoadCompleted = 1,
// 	FullAdLoadFailed,
// 	FullAdShown,// 大屏广告展示成功
// 	FullAdClicked,// 大屏广告被点击
// 	FullAdClosed,// 大屏广告被关闭

// 	RewardAdLoadFailed,
// 	RewardAdLoadCompleted,
// 	RewardAdShow,
// 	RewardAdShowFinished,
// 	RewardAdClick,
// 	RewardAdClosed,

// 	AdLoadCompleted,//广告加载成功(only ios)
// 	AdLoadFailed,//广告加载失败(only ios)
// 	AdShown,// 广告展示成功(大屏或bannner)(only ios)
// 	AdClosed,//广告被关闭(only ios)
// 	AdClicked,//广告被点击(only ios)


// 	CrossAdClicked,// 交叉推广广告被点击
// 	BannerAdClicked,// bannner广告被点击
// 	IconAdClicked,
// 	NativeAdClicked
// }


// #else
//     /// <summary>
//     /// 广告事件类型
//     /// </summary>
//     public enum AdEventType : int
//     {
//         FullAdLoadCompleted = 1,
//         FullAdLoadFailed,
//         RewardAdLoadFailed,
//         RewardAdLoadCompleted,
//         RewardAdShown,
//         RewardAdShowFinished,
//         RewardAdShowFailed,
//         RewardAdClosed,
//         RewardAdClicked,
//         FullAdClosed, // 大屏广告被关闭
//         FullAdShown, // 大屏广告展示成功
//         FullAdClicked, // 大屏广告被点击
//         BannerAdClicked, // bannner广告被点击
//         CrossAdClicked, // 交叉推广广告被点击
//         AdLoadCompleted, //广告加载成功(only ios)
//         AdLoadFailed, //广告加载失败(only ios)
//         AdShown, // 广告展示成功(大屏或bannner)(only ios)
//         AdClosed, //广告被关闭(only ios)
//         AdClicked, //广告被点击(only ios)
//         IconAdClicked,
//         NativeAdClicked
//     }
// #endif

//     public enum PaymentResult : int
//     {
//         Success = 1,
//         Failed,
//         Cancel,
//         PaymentSystemError,
//         PaymentSystemValid
//     }


//     /// <summary>
//     /// faceboook事件回调类型
//     /// </summary>
//     public enum SnsEventType : int
//     {
//         /// <summary>
//         /// faceboook登陆成功
//         /// </summary>
//         LoginSuccess = 1,
//         LoginFailed,

//         /// <summary>
//         /// faceboook邀请好友成功
//         /// </summary>
//         InviteSuccess,
//         InviteFailed,

//         /// <summary>
//         /// faceboook挑战好友成功
//         /// </summary>
//         ChallengeSuccess,
//         ChallengeFailed,

//         /// <summary>
//         /// faceboook给好友点赞成功
//         /// </summary>
//         LikeSuccess,
//         LikeFailed,
//         ShareSuccess,
//         ShareFailed,
//         ShareCancel
//     }

//     public enum LocalPushType : int
//     {
//         NoCycle = 0,
//         YearCycle = 1 << 2,
//         MonthCycle = 1 << 3,
//         DayCycle = 1 << 4,
//         HourCycle = 1 << 5,
//         MinuteCycle = 1 << 6,
//         SecondCycle = 1 << 7,
//         WeekDayCycle = 1 << 9,
//         WeekDayOrDinalCycle = 1 << 10
//     }

//     public enum FullAdType
//     {
//         once,
//         back,
//         flash,
//         settle,
//         none
//     }

//     public enum NetworkStatus
//     {
//         StatusUnknown = -1,
//         StatusNotReachable = 0,
//         StatusReachableViaWWAN = 1,
//         StatusReachableViaWiFi = 2,
//         StatusReachable = 3
//     }

// #if Headline
//     public enum DouYinShareResult
//     {
//         Fail,
//         Success,
//         SaveAlbum
//     }

//     public enum ScreenRecordResult
//     {
//         Fail,
//         Success,
//         EditingProgress,
//         AutoStopRecod
//     }

//     public enum AIVideoEditorShareResult
//     {
//         Fail,
//         Success,
//         EditingProgress,
//         RecordVideoEditSuccess
//     }

//     public enum RequestPhotoWritepermitResult
//     {
//         Fail,
//         Success
//     }
// #endif


//     public static double GetCurrentTimeInMills()
//     {
//         return 0.0d;
//         // return TimeC.Cn.GetEpochTimestampFromNow(false);
//     }

//     public static string CalculateMD5Hash(string input)
//     {
//         StringBuilder sb = new StringBuilder();
//         try
//         {
//             MD5 md5 = MD5.Create();
//             byte[] inputBytes = Encoding.UTF8.GetBytes(input);
//             inputBytes = md5.ComputeHash(inputBytes);
//             for (int i = 0; i < inputBytes.Length; i++)
//             {
//                 sb.Append(inputBytes[i].ToString("X2"));
//             }

//             return sb.ToString().ToLower();
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError("CalculateMD5Hash error:\n" + ex.StackTrace);
//         }
//         finally
//         {
//         }

//         return sb.ToString();
//     }

//     /// <summary>
//     /// Editor模式下的广告测试类，不可以调用该类的方法。
//     /// </summary>
//     private class RiseEditorAd : MonoBehaviour
//     {
//         private static RiseEditorAd _editorAdInstance = null;
//         public static bool hasInit = false;
//         private Rect bannerPos;
//         private bool bannerShow = false;
//         private string bannerContent = "";
//         private bool interstitialShow = false;
//         private string interstitialContent = "";
//         private bool rewardShow = false;
//         private string rewardContent = "";
//         private float scaleWidth = 1;
//         private float scaleHeight = 1;
//         private int originScreenWidth = 1;
//         private int originScreenHeight = 1;
//         private bool toastShow = false;
//         private List<string> toastList = new List<string>();
//         private GUIStyle toastStyle = null;
//         private int rewardAdId = NONE_REWARD_ID;
//         private string rewardAdTag = DEFAULT_REWARD_TAG;
//         private float iconAdWidth = 56;
//         private float iconAdXPercent = .2f;
//         private float iconAdYPercent = .2f;
//         private bool iconAdShow = false;
//         private string iconAdContent = "Icon Ad";
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//         private EventSystem curEvent = null;
// #endif

//         private const int NONE_REWARD_ID = -10;
//         private const string DEFAULT_REWARD_TAG = "DEFAULT";
//         private const string BANNER_DEFAULT_TXT = "Banner AD";
//         private const string INTERSTITIAL_DEFAULT_TXT = "\nInterstitial AD Test";
//         private const string REWARD_DEFAULT_TXT = "Free Coin AD Test: ";
//         private const int SCREEN_WIDTH = 854;
//         private const int SCREEN_HEIGHT = 480;
//         private const int GUI_DEPTH = -99;
//         private const int BANNER_WIDTH = 320;
//         private const int BANNER_HEIGHT = 50;

//         void Awake()
//         {
//             if (_editorAdInstance == null)
//             {
//                 _editorAdInstance = this;
//             }

//             DontDestroyOnLoad(gameObject);
//             if (Screen.width > Screen.height)
//             {
//                 originScreenWidth = SCREEN_WIDTH;
//                 originScreenHeight = SCREEN_HEIGHT;
//             }
//             else
//             {
//                 originScreenWidth = SCREEN_HEIGHT;
//                 originScreenHeight = SCREEN_WIDTH;
//             }

//             scaleWidth = Screen.width * 1f / originScreenWidth;
//             scaleHeight = Screen.height * 1f / originScreenHeight;
//             toastStyle = new GUIStyle();
//             toastStyle.fontStyle = FontStyle.Bold;
//             toastStyle.alignment = TextAnchor.MiddleCenter;
//             toastStyle.fontSize = 30;
//         }

//         public static RiseEditorAd EditorAdInstance
//         {
//             get
//             {
//                 if (_editorAdInstance == null)
//                 {
//                     _editorAdInstance = FindObjectOfType<RiseEditorAd>() == null
//                         ? new GameObject("RiseEditorAd").AddComponent<RiseEditorAd>()
//                         : _editorAdInstance;
//                 }

//                 if (!hasInit)
//                 {
//                     Debug.LogError("Fatal Error: \nNeed Call RiseSdk.Instance.Init () First At Initialize Scene");
//                 }

//                 return _editorAdInstance;
//             }
//         }

// #if UNITY_EDITOR
//         void OnGUI()
//         {
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//             if (curEvent == null) {
//                 curEvent = EventSystem.current;
//             }
// #endif
//             GUI.depth = GUI_DEPTH;
//             if (bannerShow)
//             {
//                 GUI.backgroundColor = Color.green;
//                 GUI.color = Color.red;
//                 if (GUI.Button(bannerPos, bannerContent))
//                 {
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 GUI.backgroundColor = Color.green;
//                 if (GUI.Button(bannerPos, bannerContent))
//                 {
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 GUI.backgroundColor = Color.green;
//                 if (GUI.Button(bannerPos, bannerContent))
//                 {
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 GUI.backgroundColor = Color.green;
//                 if (GUI.Button(bannerPos, bannerContent))
//                 {
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }
//             }

//             if (interstitialShow)
//             {
//                 GUI.backgroundColor = Color.black;
//                 GUI.color = Color.white;
//                 //GUI.backgroundColor = new Color (0, 0, 0, 1);
//                 //GUI.color = new Color (1, 0, 0, 1);
//                 if (GUI.Button(new Rect(Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight),
//                         "Close"))
//                 {
//                     InterstitialAdCallBack();
//                     interstitialShow = false;
//                     Instance.OnResume();
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), interstitialContent))
//                 {
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), interstitialContent))
//                 {
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), interstitialContent))
//                 {
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), interstitialContent))
//                 {
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 GUI.backgroundColor = Color.red;
//                 if (GUI.Button(new Rect(Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight),
//                         "Close"))
//                 {
//                     InterstitialAdCallBack();
//                     interstitialShow = false;
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 if (GUI.Button(new Rect(Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight),
//                         "Close"))
//                 {
//                     InterstitialAdCallBack();
//                     interstitialShow = false;
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 if (GUI.Button(new Rect(Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight),
//                         "Close"))
//                 {
//                     InterstitialAdCallBack();
//                     interstitialShow = false;
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 if (GUI.Button(new Rect(Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight),
//                         "Close"))
//                 {
//                     InterstitialAdCallBack();
//                     interstitialShow = false;
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }
//             }

//             if (rewardShow)
//             {
//                 GUI.backgroundColor = Color.black;
//                 GUI.color = Color.white;
//                 if (GUI.Button(new Rect(Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight),
//                         "Close"))
//                 {
//                     rewardShow = false;
//                     Instance.OnResume();
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                     RewardAdCallBack();
//                 }

//                 if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), rewardContent))
//                 {
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), rewardContent))
//                 {
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), rewardContent))
//                 {
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), rewardContent))
//                 {
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                 }

//                 GUI.backgroundColor = Color.red;
//                 if (GUI.Button(new Rect(Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight),
//                         "Close"))
//                 {
//                     rewardShow = false;
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                     RewardAdCallBack();
//                 }

//                 if (GUI.Button(new Rect(Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight),
//                         "Close"))
//                 {
//                     rewardShow = false;
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                     RewardAdCallBack();
//                 }

//                 if (GUI.Button(new Rect(Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight),
//                         "Close"))
//                 {
//                     rewardShow = false;
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                     RewardAdCallBack();
//                 }

//                 if (GUI.Button(new Rect(Screen.width - 100 * scaleWidth, 0, 100 * scaleWidth, 50 * scaleHeight),
//                         "Close"))
//                 {
//                     rewardShow = false;
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                     if (EventSystem.current != null) {
//                         EventSystem.current.enabled = false;
//                     }
// #endif
//                     RewardAdCallBack();
//                 }
//             }

//             if (iconAdShow)
//             {
//                 GUI.backgroundColor = Color.black;
//                 GUI.color = Color.white;
//                 //GUI.backgroundColor = new Color (0, 0, 0, 1);
//                 //GUI.color = new Color (1, 0, 0, 1);
//                 if (GUI.Button(
//                         new Rect(Screen.width * iconAdXPercent, Screen.height * iconAdYPercent, iconAdWidth,
//                             iconAdWidth), iconAdContent))
//                 {
//                 }

//                 if (GUI.Button(
//                         new Rect(Screen.width * iconAdXPercent, Screen.height * iconAdYPercent, iconAdWidth,
//                             iconAdWidth), iconAdContent))
//                 {
//                 }

//                 if (GUI.Button(
//                         new Rect(Screen.width * iconAdXPercent, Screen.height * iconAdYPercent, iconAdWidth,
//                             iconAdWidth), iconAdContent))
//                 {
//                 }

//                 if (GUI.Button(
//                         new Rect(Screen.width * iconAdXPercent, Screen.height * iconAdYPercent, iconAdWidth,
//                             iconAdWidth), iconAdContent))
//                 {
//                 }
//             }

//             if (toastList.Count > 0)
//             {
//                 GUI.backgroundColor = Color.black;
//                 GUI.color = Color.red;
//                 //GUI.contentColor = Color.red;
//                 GUI.Button(
//                     new Rect((Screen.width - 400 * scaleWidth) * .5f, Screen.height - 100 * scaleHeight,
//                         400 * scaleWidth, 50 * scaleHeight), toastList[0]);
//                 GUI.Button(
//                     new Rect((Screen.width - 400 * scaleWidth) * .5f, Screen.height - 100 * scaleHeight,
//                         400 * scaleWidth, 50 * scaleHeight), toastList[0]);
//                 //GUI.Label (new Rect ((Screen.width - 200 * scaleWidth) * .5f, Screen.height - 100 * scaleHeight, 200 * scaleWidth, 50 * scaleHeight), toastList [0], toastStyle);
//             }
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//             if (EventSystem.current != null) {
//                 EventSystem.current.enabled = true;
//             } else if (curEvent != null) {
//                 curEvent.enabled = true;
//                 EventSystem.current = curEvent;
//             }
// #endif
//         }

//         void Update()
//         {
//             if (Input.GetKeyDown(KeyCode.Escape))
//             {
//                 if (interstitialShow)
//                 {
//                     interstitialShow = false;
//                     InterstitialAdCallBack();
//                 }
//                 else if (rewardShow)
//                 {
//                     rewardShow = false;
//                     RewardAdCallBack();
//                 }
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//                 if (EventSystem.current != null) {
//                     EventSystem.current.enabled = true;
//                 } else if (curEvent != null) {
//                     curEvent.enabled = true;
//                     EventSystem.current = curEvent;
//                 }
// #endif
//             }
//         }

//         private void InterstitialAdCallBack()
//         {
//             if (interstitialShow)
//             {
// #if UNITY_IOS || PLATFORM_IOS
//                 RiseSdkListener.Instance.adDidClose("custom");
// #elif UNITY_ANDROID
//                 RiseSdkListener.Instance.onFullAdClosed("EditorAd");
// #endif
//             }
//         }

//         private void RewardAdCallBack()
//         {
//             if (rewardAdId != NONE_REWARD_ID)
//             {
//                 Toast("Show Reward Ad Success");
// #if UNITY_ANDROID
//                 RiseSdkListener.Instance.onReceiveReward("0|" + rewardAdId + "|EditorVideo");
// #elif UNITY_IOS || PLATFORM_IOS
//                 //RiseSdkListener.Instance.adReward(rewardAdTag + "|" + rewardAdId);
//                 Debug.LogWarning("[RewardAdCallBack]rewardAdId:" + rewardAdId + " rewardAdTag:" + rewardAdTag);
//                 RiseSdkListener.Instance.adReward("EditorVideo|" + rewardAdId);

// #endif
//             }

//             rewardAdId = NONE_REWARD_ID;
//             rewardAdTag = DEFAULT_REWARD_TAG;
//         }
// #endif

//         public void ShowBanner(int pos)
//         {
// #if UNITY_EDITOR
//             bannerContent = BANNER_DEFAULT_TXT + ", tag: default, pos: " + pos;
//             bannerShow = true;
//             SetBannerPos(pos);
// #endif
//         }

//         public void ShowBanner(string tag, int pos)
//         {
// #if UNITY_EDITOR
//             bannerContent = BANNER_DEFAULT_TXT + ", tag: " + tag + ", pos: " + pos;
//             bannerShow = true;
//             SetBannerPos(pos);
// #endif
//         }

//         public void ShowBanner(string tag, int pos, int animate)
//         {
// #if UNITY_EDITOR
//             bannerContent = BANNER_DEFAULT_TXT + ", tag: " + tag + ", pos: " + pos + ", animate: " + animate;
//             bannerShow = true;
//             SetBannerPos(pos);
// #endif
//         }

//         public void CloseBanner()
//         {
// #if UNITY_EDITOR
//             bannerShow = false;
// #endif
//         }

//         private void SetBannerPos(int pos)
//         {
// #if UNITY_EDITOR
//             switch (pos)
//             {
//                 case RiseSdk.POS_BANNER_LEFT_BOTTOM:
//                     bannerPos = new Rect(0, Screen.height - BANNER_HEIGHT * scaleHeight, BANNER_WIDTH * scaleWidth,
//                         BANNER_HEIGHT * scaleHeight);
//                     break;
//                 case RiseSdk.POS_BANNER_LEFT_TOP:
//                     bannerPos = new Rect(0, 0, BANNER_WIDTH * scaleWidth, BANNER_HEIGHT * scaleHeight);
//                     break;
//                 case RiseSdk.POS_BANNER_MIDDLE_BOTTOM:
//                     bannerPos = new Rect((Screen.width - BANNER_WIDTH * scaleWidth) * .5f,
//                         Screen.height - BANNER_HEIGHT * scaleHeight, BANNER_WIDTH * scaleWidth,
//                         BANNER_HEIGHT * scaleHeight);
//                     break;
//                 case RiseSdk.POS_BANNER_MIDDLE_MIDDLE:
//                     bannerPos = new Rect((Screen.width - BANNER_WIDTH * scaleWidth) * .5f,
//                         (Screen.height - BANNER_HEIGHT * scaleHeight) * .5f, BANNER_WIDTH * scaleWidth,
//                         BANNER_HEIGHT * scaleHeight);
//                     break;
//                 case RiseSdk.POS_BANNER_MIDDLE_TOP:
//                     bannerPos = new Rect((Screen.width - BANNER_WIDTH * scaleWidth) * .5f, 0, BANNER_WIDTH * scaleWidth,
//                         BANNER_HEIGHT * scaleHeight);
//                     break;
//                 case RiseSdk.POS_BANNER_RIGHT_BOTTOM:
//                     bannerPos = new Rect(Screen.width - BANNER_WIDTH * scaleWidth,
//                         Screen.height - BANNER_HEIGHT * scaleHeight, BANNER_WIDTH * scaleWidth,
//                         BANNER_HEIGHT * scaleHeight);
//                     break;
//                 case RiseSdk.POS_BANNER_RIGHT_TOP:
//                     bannerPos = new Rect(Screen.width - BANNER_WIDTH * scaleWidth, 0, BANNER_WIDTH * scaleWidth,
//                         BANNER_HEIGHT * scaleHeight);
//                     break;
//             }
// #endif
//         }

//         public void ShowAd(string tag)
//         {
// #if UNITY_EDITOR
//             interstitialShow = true;
//             interstitialContent = tag + INTERSTITIAL_DEFAULT_TXT;
//             Instance.OnPause();
// #endif
//         }

//         public void ShowRewardAd(int id)
//         {
// #if UNITY_EDITOR
//             rewardShow = true;
//             rewardAdId = id;
//             rewardAdTag = DEFAULT_REWARD_TAG;
//             rewardContent = REWARD_DEFAULT_TXT + rewardAdTag;
//             Instance.OnPause();
// #endif
//         }

//         public void ShowRewardAd(string tag, int id)
//         {
// #if UNITY_EDITOR
//             rewardShow = true;
//             rewardAdId = id;
//             rewardAdTag = tag;
//             rewardContent = REWARD_DEFAULT_TXT + tag;
//             Instance.OnPause();
// #endif
//         }

//         public void ShowIconAd(float width, float xPercent, float yPercent)
//         {
//             iconAdShow = true;
//             iconAdWidth = width;
//             iconAdXPercent = xPercent;
//             iconAdYPercent = yPercent;
//         }

//         public void CloseIconAd()
//         {
//             iconAdShow = false;
//         }

//         public void Pay(int billingId)
//         {
// #if !Headline
// #if UNITY_EDITOR

// #if UNITY_ANDROID
//             switch (EditorUtility.DisplayDialogComplex("Pay", "Pay: " + billingId, "TRY FAILURE", "NO", "YES"))
//             {
//                 case 0: //TRY FAILURE
//                     Toast("pay " + billingId + " Failed");
//                     RiseSdkListener.Instance.onPaymentFail(billingId + "");
//                     break;
//                 case 1: //NO
//                     Toast("pay " + billingId + " Canceled");
//                     RiseSdkListener.Instance.onPaymentCanceled(billingId + "");
//                     break;
//                 case 2: //YES
//                     Toast("pay " + billingId + " Success");
//                     var data = new RiseSdkData.PaymentData();
//                     data.billId = billingId;
//                     data.orderId = "test";
//                     var str = JsonConvert.SerializeObject(data);
//                     RiseSdkListener.Instance.onPaymentSuccess(str);
//                     break;
//             }
// #elif UNITY_IOS || PLATFORM_IOS
//             switch (EditorUtility.DisplayDialogComplex("Pay", "Pay: " + billingId, "TRY FAILURE", "NO", "YES"))
//             {
//                 case 0://TRY FAILURE
//                     Toast("pay " + billingId + " Failed");
//                     RiseSdkListener.Instance.onPaymentFailure(billingId + "");
//                     break;
//                 case 1://NO
//                     Toast("pay " + billingId + " Canceled");
//                     RiseSdkListener.Instance.onPaymentFailure(billingId + "");
//                     break;
//                 case 2://YES
//                     Toast("pay " + billingId + " Success");
//                     var data = new RiseSdkData.PaymentData();
//                     data.billId = billingId;
//                     data.orderId = "test";
//                     var str = JsonConvert.SerializeObject(data);
//                     RiseSdkListener.Instance.onPaymentSuccess(str);
//                     break;
//             }
// #endif
// #endif
// #endif
//         }

//         public void PayWithPayload(int billingId, string payloadData)
//         {
// #if !Headline
// #if UNITY_EDITOR
//             switch (EditorUtility.DisplayDialogComplex("Pay", "Pay: " + billingId, "TRY FAILURE", "NO", "YES"))
//             {
//                 case 0: //TRY FAILURE
//                     Toast("pay " + billingId + " Failed");
// #if UNITY_ANDROID
//                     RiseSdkListener.Instance.onPaymentFail(billingId.ToString());
// #elif UNITY_IOS || PLATFORM_IOS
//                     RiseSdkListener.Instance.onPaymentFailure(billingId.ToString());
// #endif
//                     break;
//                 case 1: //NO
//                     Toast("pay " + billingId + " Canceled");
// #if UNITY_ANDROID
//                     RiseSdkListener.Instance.onPaymentCanceled(billingId.ToString());
// #elif UNITY_IOS || PLATFORM_IOS
//                     RiseSdkListener.Instance.onPaymentFailure(billingId.ToString());
// #endif
//                     break;
//                 case 2: //YES
//                     var data = new RiseSdkData.PaymentData();
//                     data.billId = billingId;
//                     data.orderId = "test";
//                     data.payload = payloadData;
//                     var str = JsonConvert.SerializeObject(data);
//                     RiseSdkListener.Instance.onPaymentSuccessWithPayload(str);
//                     break;
//             }
// #endif
// #endif
//         }

//         public void MakeRequest(string url, Action<string> callback, Dictionary<string, string> param,
//             byte[] binaryData = null, int timeoutSecond = 10)
//         {
//             try
//             {
//                 if (string.IsNullOrEmpty(url))
//                 {
//                     if (callback != null)
//                     {
//                         callback(null);
//                     }
//                 }
//                 else
//                 {
//                     RiseSdkListener.Instance.StartCoroutine(
//                         PostRequest(url, param, callback, binaryData, timeoutSecond));
//                 }
//                 //StartCoroutine (GetRequest (url, param, callback));
//             }
//             catch (Exception e)
//             {
//                 Debug.LogError(e.StackTrace);
//             }
//         }

//         IEnumerator PostRequest(string url, Dictionary<string, string> post, Action<string> callback, byte[] binaryData,
//             int timeoutSecond)
//         {
//             WWWForm form = null;

//             if (post != null && post.Count > 0)
//             {
//                 form = new WWWForm();
//                 var enumerator = post.GetEnumerator();
//                 while (enumerator.MoveNext())
//                 {
//                     form.AddField(enumerator.Current.Key, enumerator.Current.Value);
//                 }

//                 if (binaryData != null)
//                 {
//                     form.AddBinaryData("file", binaryData);
//                 }
//             }

//             Debug.Log("PostRequest, url: " + url +
//                       (form != null ? ", WWWForm: " + Encoding.Default.GetString(form.data) : ""));

//             UnityWebRequest uwr = null;
//             if (form == null)
//             {
//                 uwr = UnityWebRequest.Get(url);
//             }
//             else
//             {
//                 uwr = UnityWebRequest.Post(url, form);
//             }

//             uwr.timeout = timeoutSecond;
//             yield return uwr.Send();
//             yield return null;
//             if (callback != null)
//             {
//                 if (!uwr.isNetworkError && uwr.responseCode == 200L)
//                 {
//                     callback(uwr.downloadHandler.text);
//                 }
//                 else
//                 {
//                     callback(null);
//                 }
//             }

//             uwr.Dispose();
//         }

//         private bool timeCounting = false;

//         public void Toast(string msg)
//         {
//             return; // 看不清，直接屏蔽了
// #if UNITY_EDITOR
//             toastList.Add(msg);
//             if (!timeCounting)
//             {
//                 timeCounting = true;
//                 StartCoroutine(CheckToast());
//             }
// #endif
//         }

//         private IEnumerator CheckToast(float time = 2)
//         {
//             yield return new WaitForSeconds(time);
//             if (toastList.Count > 0)
//             {
//                 toastList.RemoveAt(0);
//             }

//             if (toastList.Count > 0)
//             {
//                 StartCoroutine(CheckToast());
//             }
//             else
//             {
//                 timeCounting = false;
//             }
//         }

//         public void Alert(string title, string msg)
//         {
// #if UNITY_EDITOR
//             EditorUtility.DisplayDialog(title, msg, "NO", "OK");
// #endif
//         }

//         public void OnExit()
//         {
// #if UNITY_EDITOR
//             if (EditorUtility.DisplayDialog("Exit", "Are you sure to exit?", "YES", "NO"))
//             {
//                 EditorApplication.isPlaying = false;
//             }
// #endif
//         }
//     }

//     private class FileLRUCache
//     {
//         private int maxCapacity = 10;
//         private int size = 0;
//         private LinkedNode head = null;
//         private LinkedNode tail = null;
//         private Dictionary<string, LinkedNode> cache = null;

//         private const string CACHE_FILE = "filedirmeta";
//         private const string SPLIT_FLAG = "@^@";
//         private const string KEY_VALUE_SPLIT_FLAG = "^_^";
//         private static string defFilePath = "/";

//         public void DownloadFile(string url, Action<string, WWW> resultEvent)
//         {
//             if (string.IsNullOrEmpty(url))
//             {
//                 if (resultEvent != null)
//                 {
//                     resultEvent("", null);
//                 }

//                 return;
//             }

//             string saveName = CalculateMD5Hash(url);

//             if (!File.Exists(defFilePath + saveName))
//             {
//                 RiseSdkListener.Instance.StartCoroutine(Download(url, saveName, resultEvent));
//             }
//             else
//             {
//                 RiseSdkListener.Instance.StartCoroutine(LoadLocal(null, saveName, resultEvent));
//             }
//         }

//         public void LoadLocalFile(string filePath, Action<string, WWW> resultEvent)
//         {
//             if (string.IsNullOrEmpty(filePath))
//             {
//                 if (resultEvent != null)
//                 {
//                     resultEvent(filePath, null);
//                 }
//             }
//             else
//             {
//                 if (File.Exists(filePath))
//                 {
//                     RiseSdkListener.Instance.StartCoroutine(LoadLocal(filePath, null, resultEvent));
//                 }
//                 else
//                 {
//                     if (resultEvent != null)
//                     {
//                         resultEvent(filePath, null);
//                     }
//                 }
//             }
//         }

//         public bool FileDownloaded(string url)
//         {
//             string saveName = CalculateMD5Hash(url);
//             return File.Exists(defFilePath + saveName);
//         }

//         IEnumerator Download(string url, string saveName, Action<string, WWW> resultEvent)
//         {
//             if (string.IsNullOrEmpty(url))
//             {
//                 if (resultEvent != null)
//                 {
//                     resultEvent(defFilePath + saveName, null);
//                     Debug.LogWarning("Download File error, url: " + url + ", saveName: " + saveName);
//                 }
//             }
//             else
//             {
//                 WWW www = new WWW(url);
//                 yield return www;
//                 set(saveName, defFilePath + saveName);
//                 if (string.IsNullOrEmpty(www.error))
//                 {
//                     if (www.bytes != null && www.bytes.Length > 1000)
//                     {
//                         byte[] bytesData = www.bytes;
//                         File.WriteAllBytes(defFilePath + saveName, bytesData);
//                         if (resultEvent != null)
//                         {
//                             resultEvent(defFilePath + saveName, www);
//                         }
//                     }
//                     else if (resultEvent != null)
//                     {
//                         resultEvent(defFilePath + saveName, null);
//                     }
//                 }
//                 else if (resultEvent != null)
//                 {
//                     resultEvent(defFilePath + saveName, null);
//                     Debug.LogError("Download File error, url: " + url + ", saveName: " + saveName + ", www.error: " +
//                                    www.error);
//                 }
//             }
//         }

//         IEnumerator LoadLocal(string filePath, string saveName, Action<string, WWW> resultEvent)
//         {
//             if (string.IsNullOrEmpty(filePath))
//             {
//                 if (resultEvent != null)
//                 {
//                     resultEvent(filePath + saveName, null);
//                     Debug.LogWarning("LoadLocal File error, filePath: " + filePath + ", saveName: " + saveName);
//                 }
//             }
//             else
//             {
//                 if (string.IsNullOrEmpty(filePath))
//                 {
//                     filePath = defFilePath;
//                 }

//                 if (saveName == null)
//                 {
//                     saveName = "";
//                 }

//                 string path = "file:///" + filePath + saveName;
//                 WWW www = new WWW(path);
//                 yield return www;
//                 set(saveName, filePath + saveName);
//                 if (string.IsNullOrEmpty(www.error))
//                 {
//                     if (www.bytes != null && www.bytes.Length > 1000)
//                     {
//                         if (resultEvent != null)
//                         {
//                             resultEvent(filePath + saveName, www);
//                         }
//                     }
//                     else if (resultEvent != null)
//                     {
//                         resultEvent(filePath + saveName, null);
//                     }
//                 }
//                 else if (resultEvent != null)
//                 {
//                     resultEvent(filePath + saveName, null);
//                     Debug.LogError("LoadLocal File error, filePath: " + filePath + ", saveName: " + saveName +
//                                    ", www.error: " + www.error);
//                 }
//             }
//         }

//         public enum FileType
//         {
//             Image,
//             Text
//         }

//         public FileLRUCache(int capacity)
//         {
//             maxCapacity = capacity;
//             cache = new Dictionary<string, LinkedNode>();
//             head = new LinkedNode();
//             tail = new LinkedNode();
//             head.prev = null;
//             head.next = tail;
//             tail.prev = head;
//             tail.next = null;
//             try
//             {
//                 defFilePath = Application.persistentDataPath + "/filecache/";
//                 if (!Directory.Exists(defFilePath))
//                 {
//                     Directory.CreateDirectory(defFilePath);
//                 }
//             }
//             catch (System.Exception ex)
//             {
//                 Debug.LogError("FileLRUCache init error\n" + ex.StackTrace);
//                 defFilePath = Application.persistentDataPath + "/filecache/files/";
//             }
//             finally
//             {
//                 if (!Directory.Exists(defFilePath))
//                 {
//                     Directory.CreateDirectory(defFilePath);
//                 }

//                 string filePath = defFilePath + CACHE_FILE;

//                 if (!File.Exists(filePath))
//                 {
//                     File.Create(filePath);
//                     RiseSdkListener.Instance.StartCoroutine(delayLoad(filePath, 1));
//                 }
//                 else
//                 {
//                     LoadLocalFile(filePath, loadCache);
//                 }
//             }
//         }

//         private IEnumerator delayLoad(string path, float delayTime)
//         {
//             yield return new WaitForSeconds(delayTime);
//             LoadLocalFile(path, loadCache);
//         }

//         private void loadCache(string path, WWW www)
//         {
//             if (www != null)
//             {
//                 string data = www.text;

//                 if (!string.IsNullOrEmpty(data))
//                 {
//                     string[] keyValues = data.Split(SPLIT_FLAG.ToCharArray());
//                     LinkedNode node = null;
//                     LinkedNode tailPrev = null;
//                     size = 0;
//                     string[] keyValue = null;

//                     for (int i = 0, len = keyValues.Length; i < len; i++)
//                     {
//                         keyValue = null;
//                         if (!string.IsNullOrEmpty(keyValues[i]))
//                         {
//                             keyValue = keyValues[i].Split(KEY_VALUE_SPLIT_FLAG.ToCharArray());
//                         }

//                         if (keyValue != null && keyValue.Length > 1 && !string.IsNullOrEmpty(keyValue[0]) &&
//                             !string.IsNullOrEmpty(keyValue[1]))
//                         {
//                             node = new LinkedNode();
//                             node.key = keyValue[0];
//                             node.value = keyValue[1];
//                             tailPrev = tail.prev;
//                             tail.prev = node;
//                             node.next = tail;
//                             node.prev = tailPrev;
//                             tailPrev.next = node;
//                             cache.Add(keyValue[0], node);
//                             size++;
//                         }
//                     }
//                 }
//             }
//         }

//         private void writeCache()
//         {
// #if UNITY_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
//             if (writting != null) {
//                 RiseSdkListener.Instance.StopCoroutine (writting);
//             }
// #endif
//             writting = RiseSdkListener.Instance.StartCoroutine(delayWrite());
//         }

//         private Coroutine writting = null;

//         private IEnumerator delayWrite()
//         {
//             yield return new WaitForSeconds(1);

//             string str = "";
//             LinkedNode node = head.next;

//             while (node != null && node != tail)
//             {
//                 str += node.key + KEY_VALUE_SPLIT_FLAG + node.value + SPLIT_FLAG;
//                 node = node.next;
//             }

//             str = str.Remove(str.Length - SPLIT_FLAG.Length, SPLIT_FLAG.Length);
//             File.WriteAllText(defFilePath + CACHE_FILE, str, UTF8Encoding.UTF8);
//         }

//         private void set(string key, string value)
//         {
//             if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
//             {
//                 return;
//             }

//             LinkedNode node = null;

//             if (cache.ContainsKey(key))
//             {
//                 node = cache[key];
//                 moveToFront(node);
//             }
//             else
//             {
//                 node = new LinkedNode();
//                 node.key = key;
//                 node.value = value;
//                 linkAtFront(node);
//                 cache.Add(key, node);
//                 size++;
//             }

//             checkCapacity();
//             writeCache();
//         }

//         private void checkCapacity()
//         {
//             while (size > maxCapacity)
//             {
//                 size--;
//                 removeLast();
//             }
//         }

//         private string get(string key)
//         {
//             if (!string.IsNullOrEmpty(key))
//             {
//                 return cache[key].value;
//             }

//             return "";
//         }

//         private void linkAtFront(LinkedNode node)
//         {
//             LinkedNode headNext = head.next;
//             head.next = node;
//             node.prev = head;
//             node.next = headNext;
//             headNext.prev = node;
//         }

//         private void moveToFront(LinkedNode node)
//         {
//             LinkedNode prevNode = node.prev;
//             LinkedNode nextNode = node.next;

//             if (prevNode == null || nextNode == null)
//             {
//                 return;
//             }

//             prevNode.next = nextNode;
//             nextNode.prev = prevNode;
//             linkAtFront(node);
//         }

//         private void removeLast()
//         {
//             LinkedNode tailPrev = tail.prev;
//             LinkedNode tailPrevPrev = tailPrev.prev;

//             if (tailPrev == head || tailPrevPrev == null)
//             {
//                 return;
//             }

//             tailPrevPrev.next = tail;
//             tail.prev = tailPrevPrev;
//             cache.Remove(tailPrev.key);
//             File.Delete(tailPrev.value);
//         }
//     }

//     private class LinkedNode
//     {
//         public string key = null;
//         public string value = null;
//         public LinkedNode prev = null;
//         public LinkedNode next = null;
//     }

// #if !UNITY_IOS || !PLATFORM_IOS
//     public bool IsIPhoneX()
//     {
//         return false;
//     }
// #endif


//     /// <summary>
//     /// 推送窗口关闭时
//     /// </summary>
//     public void InAppMessageDismissed()
//     {
// #if UNITY_EDITOR
//         Debug.Log("inAppMessage Dismissed");
// #endif
// #if UNITY_ANDROID
//         if (_class != null)
//         {
//             _class.CallStatic("inAppMessageDismissed");
//         }
// #elif UNITY_IOS || PLATFORM_IOS
//         inAppMessageDismissed();
// #endif
//     }


//     /// <summary>
//     /// 玩家点击完成支付或者领取了推送的奖励
//     /// </summary>
//     public void InAppMessageClicked()
//     {
// #if UNITY_EDITOR
//         Debug.Log("inAppMessage Clicked");
// #endif
// #if UNITY_ANDROID
//         if (_class != null)
//         {
//             _class.CallStatic("inAppMessageClicked", 0);
//         }
// #elif UNITY_IOS || PLATFORM_IOS
//         inAppMessageClicked(0);
// #endif
//     }

//     ///<summary>
//     ///是否已开启通知权限
//     ///</summary>
//     public bool IsNotificationEnabled()
//     {
//         // return false; // sdk无此方法
// #if UNITY_ANDROID
//         if (_class != null)
//         {
//             return _class.CallStatic<bool>("isNotificationEnabled");
//         }
// #endif
//         return false;
//     }

//     ///<summary>
//     ///开启通知权限
//     ///</summary>
//     public void OpenNotificationSettings()
//     {
//         // return; // sdk无此方法
// #if UNITY_ANDROID
//         if (_class != null)
//         {
//             _class.CallStatic("openNotificationSettings");
//         }
// #endif
//     }
// }

// /// <summary> 国内部分 </summary>
// public sealed partial class RiseSdk
// {
//     /// <summary> 个人信息 </summary>
//     public string Me()
//     {
// #if UNITY_EDITOR
//         return "";
// #endif

// #if CN_Android_Normal
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<string>("me");
//         }
// #endif
// #if IVYSDK_DY || IVYSDK_WX
//         return MiniSdk.Me();
// #endif
// #if CN_IOS_Normal
//         me();
// #endif
//         return "";
//     }

//     /// <summary> 请求tx云函数,建议使用CloudFunc.CommonRequest </summary>
//     public void CallTxCloudFunc(string funcName, string jsonObj)
//     {
// #if CN_Android_Normal
//         if (_class != null && DebugMono)
//         {
//             if (funcName.StartsWith("api_"))
//             {
//                 _class.CallStatic("cloudFuncWithoutLogin", funcName, jsonObj);
//             }
//             else
//             {
//                 _class.CallStatic("cloudFunc", funcName, jsonObj);
//             }
//         }
// #endif
// #if IVYSDK_DY || IVYSDK_WX
//         CloudStorage.CallFunction(funcName, jsonObj);
// #endif
// #if CN_IOS_Normal
// #if UNITY_EDITOR
//         return;
// #endif
//         if (funcName.StartsWith("api_"))
//         {
//             cloudFuncWithoutLogin(funcName,jsonObj);
//         }
//         else
//         {
//             cloudFunc(funcName,jsonObj);
//         }
// #endif
//     }


//     /// <summary> 登陆 </summary>
//     public void Login()
//     {
// #if UNITY_EDITOR
//         RiseSdkListener.Instance.onWXLogSuccess("");
// #endif
// #if CN_Android_Normal
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("login");
//         }
// #endif
// #if CN_IOS_Normal
//         login();
// #endif
// #if IVYSDK_WX || IVYSDK_DY
//         Debug.Log("调用MiniSdk Login");
//         MiniSdk.Login();
// #endif
//     }

//     /// <summary>
//     /// 微信登录实名验证
//     /// </summary>
//     public void WXVerify(string name, string idCard)
//     {
// #if CN_Android_Normal
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("adultVerify", name, idCard);
//         }
// #endif
// #if CN_IOS_Normal
//         adultVerify(name,idCard);
// #endif
// #if IVYSDK_WX
//         //TODO:微信实名认证
// #endif
//     }

//     /// <summary>
//     /// 显示SDK的身份验证界面
//     /// </summary>
//     public void ShowRealNameInputDialog()
//     {
// #if CN_Android_Normal
// #if UNITY_EDITOR
// #endif
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("showRealNameInputDialog");
//         }
// #endif
//     }

//     /// <summary> 发起通知 </summary>
//     /// <param name="delay">通知延迟时间，单位min</param>
//     public void PushNotification(string tag, string title, string subtitle, long delay, bool autoCancel)
//     {
// #if CN_Android_Normal
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("push", tag, title, subtitle, delay, autoCancel);
//         }
// #endif
//     }

//     /// <summary> 取消指定通知 </summary>
//     public void CancelNotification(string tag)
//     {
// #if CN_Android_Normal
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("cancelNotification", tag);
//         }
// #endif
//     }

//     /// <summary> 取消所有通知 </summary>
//     public void CancelAllNotification()
//     {
// #if CN_Android_Normal
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("cancelAllNotification");
//         }
// #endif
//     }

// #if CN_IOS_Normal
//     /// <summary>
//     /// IOS退出游戏
//     /// </summary>
//     public void ForceQuit()
//     {
//         Application.Quit();
//     }
//     public void Create()
//     {
//         onCreate();
//         DelayInvoke.DelayDo(showGameProtocolDialog, 1f);
//     }
// #endif

//     /// <summary>
//     /// 跳转到加qq群界面
//     /// </summary>
//     /// <param name="id">任意值</param>
//     /// <param name="key">https://qun.qq.com/join.html</param>
//     public void JoinQQGroup(string id, string key)
//     {
// #if CN_Android_Normal
//         if (_class != null && DebugMono)
//         {
//             _class.CallStatic("joinQQGroup", id, key);
//         }
// #endif
//     }

//     //开始录屏
//     public void StartRecordScreen(bool isRecordAudio, int maxRecordTime)
//     {
// #if UNITY_EDITOR
//         RiseSdkListener.Instance.OnStartRecordScreenSuccess();
//         return;
// #endif
// #if IVYSDK_DY
//         DEBUG.LogErrorX($"Start Recording");
//         MiniSdk.StartRecordScreen(isRecordAudio, maxRecordTime);
// #endif
//     }

//     // 结束录屏
//     public void StopRecordScreen()
//     {
// #if UNITY_EDITOR
//         RiseSdkListener.Instance.OnStopRecordScreenComplete();
//         return;
// #endif
// #if IVYSDK_DY
//         DEBUG.LogErrorX($"Stop Recording");
//         MiniSdk.StopRecordScreen();
// #endif
//     }

//     /// <summary>
//     /// >3s 才可以分享 单位 ms
//     /// </summary>
//     /// <returns></returns>
//     public int GetRecordDuration()
//     {
// #if IVYSDK_DY
//         return MiniSdk.GetRecordDuration();
// #endif
//         return 0;
//     }

//     // 分享
//     public void ShareVideo()
//     {
// #if IVYSDK_DY
//         MiniSdk.ShareVideo();
// #endif
//     }

//     /// <summary>
//     /// 是否允许打开侧边栏
//     /// </summary>
//     public void CheckScene()
//     {
// #if IVYSDK_DY
//         MiniSdk.CheckScene();
// #endif
//     }

//     /// <summary>
//     /// 导航打开侧边栏
//     /// </summary>
//     public void NavigateToScene()
//     {
// #if IVYSDK_DY
//         MiniSdk.NavigateToScene();
// #endif
//     }

//     /// <summary>
//     /// 是否从侧边栏进入
//     /// </summary>
//     /// <returns></returns>
//     public bool IsFromSidebarCard()
//     {
// #if IVYSDK_DY
//         return MiniSdk.IsFromSidebarCard();
// #endif
//         return false;
//     }

//     public void ReplaceSensitiveWords(string words)
//     {
// #if IVYSDK_DY
//         MiniSdk.ReplaceSensitiveWords(words);
// #endif
//     }

//     public void ShowKeyboard(string defaultValue, int maxLength)
//     {
// #if IVYSDK_DY
//         MiniSdk.ShowKeyboard();
// #elif IVYSDK_WX
//         MiniSdk.ShowKeyboard(defaultValue, maxLength);
// #endif
//     }

//     public void HideKeyboard()
//     {
// #if IVYSDK_DY || IVYSDK_WX
//         MiniSdk.HideKeyboard();
// #endif
//     }

//     public Dictionary<string, object> GetSystemInfo()
//     {
// #if IVYSDK_DY || IVYSDK_WX
//         return MiniSdk.GetSystemInfo();
// #endif
//         return new Dictionary<string, object>();
//     }

//     public void SetClipboardData(string text)
//     {
// #if IVYSDK_DY || IVYSDK_WX
//         MiniSdk.SetClipboardData(text);
// #endif
//     }

//     public bool IsIOSSystem()
//     {
// #if IVYSDK_DY || IVYSDK_WX
//         return MiniSdk.IsIOSSystem();
// #endif
//         return false;
//     }

//     /// <summary> android、ios 环境下 获取刘海屏高度 </summary>
//     public int GetNotchHeight()
//     {
// #if UNITY_EDITOR
//         return 0;
// #endif
// #if CN_Android_Normal
//         if (_class != null && DebugMono)
//         {
//             return _class.CallStatic<int>("getNotchHeight");
//         }
// #elif CN_IOS_Normal
//         return getNotchHeight();
// #endif
//         return 0;
//     }

//     /// <summary> wx、抖音 小游戏环境下获取左右安全距离和菜单按钮所需偏移量 </summary>
//     public Vector3 GetHorizonSafeAreaOffsetAndMenuOffset()
//     {
// #if IVYSDK_WX || IVYSDK_DY
//         return MiniSdk.GetHorizonSafeAreaOffsetAndMenuOffset();
// #endif
//         return Vector3.zero;
//     }

//     /// <summary>
//     /// 注册WX端 返回前台事件
//     /// </summary>
//     /// <param name="result"></param>
//     public void RegisterWxShowFunc(Action result)
//     {
// #if IVYSDK_WX
//         MiniSdk.RegisterWxShowFunc(result);
// #endif
//     }

//     /// <summary>
//     /// 注销WX端返回返回前台事件，不传参则全部注销
//     /// </summary>
//     /// <param name="result"></param>
//     public void UnregisterWxShowFunc(Action result)
//     {
// #if IVYSDK_WX
//         MiniSdk.UnregisterWxShowFunc(result);
// #endif
//     }

//     /// <summary>
//     /// 注册WX端返回主菜单的回调
//     /// </summary>
//     /// <param name="action"></param>
//     public void RegisterWxHideFunc(Action action)
//     {
// #if IVYSDK_WX
//         MiniSdk.RegisterWxHideFunc(action);
// #endif
//     }

//     /// <summary>
//     /// 注销返回主菜单的回调，不传参则全部注销
//     /// </summary>
//     /// <param name="action"></param>
//     public void UnregisterWxHideFunc(Action action = null)
//     {
// #if IVYSDK_WX
//         MiniSdk.UnregisterWxHideFunc(action);
// #endif
//     }

//     public void CleanAllFileCache()
//     {
// #if IVYSDK_WX
//         MiniSdk.CleanAllFileCache();
// #endif
//     }

// }
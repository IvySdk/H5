// #region Using

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Text;
// using Newtonsoft.Json;
// using UnityEngine;

// #endregion

// /// <summary>
// /// SDK接口回调类
// /// </summary>
// public partial class RiseSdkListener : MonoBehaviour
// {
//     #region 登录游戏补单时，使用的

//     public static List<string> LastPaySuccessData;
//     public static List<string> LastPayWithLoadSuccessData;

//     #endregion

//     public void CompletedLoadPayData()
//     {
//         if (LastPaySuccessData != null)
//         {
//             while (LastPaySuccessData.Count > 0)
//             {
//                 string data = LastPaySuccessData[0];
//                 LastPaySuccessData.RemoveAt(0);
//                 onPaymentSuccess(data);
//             }

//             LastPaySuccessData = null;
//         }

//         if (LastPayWithLoadSuccessData != null)
//         {
//             while (LastPayWithLoadSuccessData.Count > 0)
//             {
//                 string data2 = LastPayWithLoadSuccessData[0];
//                 LastPayWithLoadSuccessData.RemoveAt(0);
//                 onPaymentSuccessWithPayload(data2);
//             }

//             LastPayWithLoadSuccessData = null;
//         }
//     }


//     public static void ClearOnAdEvent()
//     {
//         if (OnAdEvent != null)
//         {
//             var list = OnAdEvent.GetInvocationList();
//             for (var i = list.Length - 1; i >= 0; --i)
//             {
//                 OnAdEvent -= list[i] as Action<RiseSdk.AdEventType, int, string, int>;
//             }
//         }
//     }
// #if UNITY_ANDROID || MiniGame && !CN_IOS_Normal || CN_WX || IVYSDK_DY
//     /// <summary>
//     /// 支付的结果回调事件
//     /// </summary>
//     public static event Action<RiseSdk.PaymentResult, int, string> OnPaymentEvent;

//     //支付接口初始化成功回调
//     public static event Action<string> OnStoreLoaded;

//     /// <summary>
//     /// 有payload的支付结果回调
//     /// </summary>
//     public static event Action<RiseSdk.PaymentResult, int, string, string> OnPaymentWithPayloadEvent;

//     /// <summary>
//     /// 网络状态变化回调：android返回StatusNotReachable或StatusReachable
//     /// </summary>
//     public static event Action<RiseSdk.NetworkStatus> OnNetworkChangedEvent;

//     /// <summary>
//     /// 下载文件的结果回调事件
//     /// </summary>
//     public static event Action<bool, int, string> OnCacheUrlResult;

//     /// <submit or load, success, leader board id, extra data>
//     public static event Action<bool, bool, string, string> OnLeaderBoardEvent;

//     public static event Action<int, bool, string> OnReceiveServerResult;

//     public static event Action<string> OnReceivePaymentsPrice;

//     /// <summary>
//     /// 获取后台自定义json数据的结果回调事件
//     /// </summary>
//     public static event Action<string> OnReceiveServerExtra;

//     /// <summary>
//     /// 获取通知栏消息的结果回调事件
//     /// </summary>
//     public static event Action<string> OnReceiveNotificationData;

//     public static event Action<string> OnSavedGameRead;

//     public static event Action<string> OnSaveGameSuccess;

//     public static bool saveGameReadFlag = false;

//     public static event Action<string> OnChatMessage;

//     public static event Action<string> OnFirestoreReadData;

//     public static event Action<string> OnFirestoreReadFail;

//     public static event Action<string> OnFirestoreUpdateSuccess;

//     public static event Action<string> OnFirestoreConnected;

//     public static event Action<string> OnFirestoreConnectError;

//     public static event Action<string> OnFirestoreLinkError;

//     public static event Action<string> OnFirestoreUpdateFail;

//     public static event Action<string> OnFirestoreMergeSuccess;

//     public static event Action<string> OnFirestoreMergeFail;

//     public static event Action<string> OnFirestoreSetSuccess;

//     public static event Action<string> OnFirestoreSetFail;

//     public static event Action<string> OnPaymentData;

//     public static event Action<string> OnShareSuccess;

//     //InAppMessage运营活动推送
//     public static event Action<string> OnInAppMessageDisplayed;

//     public static event Action<string> OnFirestoreSnapshot;

//     public static event Action<string> OnSubscriptionState;

//     /// <summary>
//     /// 1.RiseSdk.AdEventType
//     /// 2.rewardId
//     /// 3.ad tag
//     /// 4.RiseSdk.ADTYPE_
//     /// 5.video skipped  //max 4 param limited.
//     /// </summary>
//     public static event Action<RiseSdk.AdEventType, int, string, int> OnAdEvent;

//     public static event Action OnResumeAdEvent;

//     public static event Action<string> OnInAppMessageEvent;

//     public static event Action<int> OnAIHelpUnreadMessageEvent;

//     private static RiseSdkListener _instance;

//     /// <summary>
//     /// 单例对象
//     /// </summary>
//     public static RiseSdkListener Instance
//     {
//         get
//         {
//             if (_instance == null)
//             {
//                 // check if there is a IceTimer instance already available in the scene graph
//                 _instance = FindObjectOfType(typeof(RiseSdkListener)) as RiseSdkListener;

//                 // nope, create a new one
//                 if (_instance == null)
//                 {
//                     GameObject obj = new GameObject("RiseSdkListener");
//                     _instance = obj.AddComponent<RiseSdkListener>();
//                     DontDestroyOnLoad(obj);
//                 }
//             }

//             return _instance;
//         }
//     }

//     void OnApplicationPause(bool pauseStatus)
//     {
//         if (pauseStatus)
//         {
//             RiseSdk.Instance.OnPause();
//         }
//     }

//     void OnApplicationFocus(bool focusStatus)
//     {
//         if (focusStatus)
//         {
//             RiseSdk.Instance.OnResume();
//         }
//     }

//     void OnApplicationQuit()
//     {
//         RiseSdk.Instance.OnStop();
//         RiseSdk.Instance.OnDestroy();
//     }

//     void Awake()
//     {
//         RiseSdk.Instance.OnStart();
//     }

//     public void OnResumeAd()
//     {
//         if (OnResumeAdEvent != null)
//             OnResumeAdEvent();
//     }


//     private void OnEnable()
//     {
// #if IVYSDK_WX || IVYSDK_DY
//         RiseSdk.Instance.RegisterWxHideFunc(OnWxHideFunc);
//         RiseSdk.Instance.RegisterWxShowFunc(OnWxShowFunc);
// #endif
//     }

//     private void OnDisable()
//     {
// #if IVYSDK_WX || IVYSDK_DY
//         RiseSdk.Instance.UnregisterWxHideFunc(OnWxHideFunc);
//         RiseSdk.Instance.UnregisterWxShowFunc(OnWxShowFunc);
// #endif
//     }

//     private void OnWxHideFunc()
//     {
//         OnApplicationFocus(false);
//         OnApplicationPause(true);
//         OnApplicationQuit();
//     }

//     private void OnWxShowFunc()
//     {
//         OnApplicationPause(false);
//         OnApplicationFocus(true);
//     }

//     /// <summary>
//     /// 网络状态变化的回调
//     /// </summary>
//     /// <param name="data">返回0或1</param>
//     public void onNetworkChanged(string data)
//     {
//         if (OnNetworkChangedEvent != null && OnNetworkChangedEvent.GetInvocationList().Length > 0)
//         {
//             int status = 0;
//             if (int.TryParse(data, out status))
//             {
//                 RiseSdk.NetworkStatus networkStatus = status > 0
//                     ? RiseSdk.NetworkStatus.StatusReachable
//                     : RiseSdk.NetworkStatus.StatusNotReachable;
//                 OnNetworkChangedEvent(networkStatus);
//             }
//         }
//     }


//     /// <summary>
//     /// 支付初始化建立时，SDK自动调用。
//     /// </summary>
//     public void onStoreLoaded(string type)
//     {
//         if (OnStoreLoaded != null && OnStoreLoaded.GetInvocationList().Length > 0)
//         {
//             OnStoreLoaded(type);
//         }
//     }


//     #region 支付相关

//     /// <summary>
//     /// 支付成功结果回调方法，SDK自动调用。
//     /// </summary>
//     /// <param name="data"> {PaymentCallBackData} </param>
//     public void onPaymentSuccess(string data)
//     {
//         if (string.IsNullOrEmpty(data))
//             return;
//         RiseSdkData.PaymentData resData;
//         try
//         {
//             resData = JsonConvert.DeserializeObject<RiseSdkData.PaymentData>(data);
//         }
//         catch
//         {
//             return;
//         }
//         if (string.IsNullOrEmpty(resData.payload))
//         {
//             OnPaymentEvent?.Invoke(RiseSdk.PaymentResult.Success, resData.billId, resData.orderId);
//         }
//         else
//         {
//             OnPaymentWithPayloadEvent?.Invoke(RiseSdk.PaymentResult.Success, resData.billId, resData.payload,
//                 resData.orderId);
//         }
//     }

//     /// <summary>
//     /// payload支付成功结果回调，SDK自动调用。
//     /// </summary>
//     /// <param name="data"></param>
//     public void onPaymentSuccessWithPayload(string data)
//     {
//         if (string.IsNullOrEmpty(data))
//             return;
//         RiseSdkData.PaymentData resData;
//         try
//         {
//             resData = JsonConvert.DeserializeObject<RiseSdkData.PaymentData>(data);
//         }
//         catch
//         {
//             return;
//         }
//         if (string.IsNullOrEmpty(resData.payload))
//         {
//             OnPaymentEvent?.Invoke(RiseSdk.PaymentResult.Success, resData.billId, resData.orderId);
//         }
//         else
//         {
//             OnPaymentWithPayloadEvent?.Invoke(RiseSdk.PaymentResult.Success, resData.billId, resData.payload,
//                 resData.orderId);
//         }
//     }

//     /// <summary>
//     /// 支付失败结果回调方法，SDK自动调用。
//     /// </summary>
//     /// <param name="billingId">计费点Id</param>
//     public void onPaymentFail(string billingId)
//     {
//         OnPaymentEvent?.Invoke(RiseSdk.PaymentResult.Failed, int.Parse(billingId), "");
//         OnPaymentWithPayloadEvent?.Invoke(RiseSdk.PaymentResult.Failed, int.Parse(billingId), "", "");
//     }

//     /// <summary>
//     /// 支付取消结果回调方法，SDK自动调用。
//     /// </summary>
//     /// <param name="billingId">计费点Id</param>
//     public void onPaymentCanceled(string billingId)
//     {
//         OnPaymentEvent?.Invoke(RiseSdk.PaymentResult.Cancel, int.Parse(billingId), "");
//         OnPaymentWithPayloadEvent?.Invoke(RiseSdk.PaymentResult.Cancel, int.Parse(billingId), "", "");
//     }

//     /// <summary>
//     /// 设置支付系统状态，SDK自动调用。
//     /// </summary>
//     public void onPaymentSystemError(string data)
//     {
//         RiseSdk.Instance.SetPaymentSystemValid(false);
//         OnPaymentEvent?.Invoke(RiseSdk.PaymentResult.PaymentSystemError, -1, "");
//     }

//     /// <summary>
//     /// 设置支付系统状态，SDK自动调用。
//     /// </summary>
//     public void onPaymentSystemValid(string data)
//     {
//         RiseSdk.Instance.SetPaymentSystemValid(true);
//         OnPaymentEvent?.Invoke(RiseSdk.PaymentResult.PaymentSystemError, -1, "");
//     }

//     public void onReceiveBillPrices(string data)
//     {
//         if (OnReceivePaymentsPrice != null && OnReceivePaymentsPrice.GetInvocationList().Length > 0)
//         {
//             OnReceivePaymentsPrice(data);
//         }
//     }

//     #endregion


//     public void onSubmitSuccess(string leaderBoardTag)
//     {
//         if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList().Length > 0)
//         {
//             OnLeaderBoardEvent(true, true, leaderBoardTag, "");
//         }
//     }

//     public void onSubmitFailure(string leaderBoardTag)
//     {
//         if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList().Length > 0)
//         {
//             OnLeaderBoardEvent(true, false, leaderBoardTag, "");
//         }
//     }

//     public void onLoadSuccess(string data)
//     {
//         if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList().Length > 0)
//         {
//             string[] results = data.Split('|');
//             OnLeaderBoardEvent(false, true, results[0], results[1]);
//         }
//     }

//     public void onLoadFailure(string leaderBoardTag)
//     {
//         if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList().Length > 0)
//         {
//             OnLeaderBoardEvent(false, false, leaderBoardTag, "");
//         }
//     }

//     public void onServerResult(string data)
//     {
//         if (OnReceiveServerResult != null && OnReceiveServerResult.GetInvocationList().Length > 0)
//         {
//             string[] results = data.Split('|');
//             int resultCode = int.Parse(results[0]);
//             bool success = int.Parse(results[1]) == 0;
//             OnReceiveServerResult(resultCode, success, results[2]);
//         }
//     }

//     /// <summary>
//     /// 下载文件结果回调方法，SDK自动调用。
//     /// </summary>
//     /// <param name="data">返回的数据</param>
//     public void onCacheUrlResult(string data)
//     {
//         if (OnCacheUrlResult != null && OnCacheUrlResult.GetInvocationList().Length > 0)
//         {
//             //tag,success,name
//             string[] results = data.Split('|');
//             int tag = int.Parse(results[0]);
//             bool success = int.Parse(results[1]) == 0;
//             if (success)
//             {
//                 OnCacheUrlResult(true, tag, results[2]);
//             }
//             else
//             {
//                 OnCacheUrlResult(false, tag, "");
//             }
//         }
//     }

//     /// <summary>
//     /// 获取后台配置的自定义json数据的回调。当SDK初始化完成，第一次取到数据后会自动调用该方法，如果需要可以提前添加监听。
//     /// </summary>
//     /// <param name="data">返回后台配置的自定义json数据，如：{"x":"x", "x":8, "x":{x}, "x":[x]}</param>
//     public void onReceiveServerExtra(string data)
//     {
//         if (OnReceiveServerExtra != null && OnReceiveServerExtra.GetInvocationList().Length > 0)
//         {
//             OnReceiveServerExtra(data);
//         }
//     }

//     /// <summary>
//     /// 获取到通知栏消息数据的回调。当点击通知栏消息打开应用时，会自动调用该方法，如果需要可以提前添加监听。
//     /// </summary>
//     /// <param name="data">后台配置的数据</param>
//     public void onReceiveNotificationData(string data)
//     {
//         if (OnReceiveNotificationData != null && OnReceiveNotificationData.GetInvocationList().Length > 0)
//         {
//             OnReceiveNotificationData(data);
//         }
//     }

//     public void onChatMessage(string jsonData)
//     {
//         if (OnChatMessage != null && OnChatMessage.GetInvocationList().Length > 0)
//         {
//             OnChatMessage(jsonData);
//         }
//     }

//     public void onShareSuccess(string data)
//     {
//         if (OnShareSuccess != null && OnShareSuccess.GetInvocationList().Length > 0)
//         {
//             OnShareSuccess?.Invoke(data);
//         }
//     }

//     /// <summary>
//     /// 接收InAppMessage活动推送
//     /// </summary>
//     /// <param name="json"></param>
//     public void onInAppMessageDisplayed(string json)
//     {
//         if (OnInAppMessageDisplayed != null && OnInAppMessageDisplayed.GetInvocationList().Length > 0)
//         {
//             OnInAppMessageDisplayed?.Invoke(json);
//         }
//     }

//     public void onFirestoreSnapshot(string data)
//     {
//         if (OnFirestoreSnapshot != null && OnFirestoreSnapshot.GetInvocationList().Length > 0)
//         {
//             OnFirestoreSnapshot(data);
//         }
//     }

//     public void onSubscriptionState(string data)
//     {
//         if (OnSubscriptionState != null && OnSubscriptionState.GetInvocationList().Length > 0)
//         {
//             OnSubscriptionState(data);
//         }
//     }

//     //支付数据返回
//     public void onPaymentData(string data)
//     {
//         if (OnPaymentData != null && OnPaymentData.GetInvocationList().Length > 0)
//         {
//             OnPaymentData(data);
//         }
//     }

//     //选择数据成功
//     public void onSavedGameRead(string data)
//     {
//         if (OnSavedGameRead != null && OnSavedGameRead.GetInvocationList().Length > 0)
//         {
//             OnSavedGameRead(data);
//         }
//     }

//     //上传数据成功
//     public void onSaveGameSuccess(string data)
//     {
//         if (OnSaveGameSuccess != null && OnSaveGameSuccess.GetInvocationList().Length > 0)
//         {
//             OnSaveGameSuccess(data);
//         }
//     }


//     //上传数据或者选择数据失败  提示  操作失败，请稍后再试。。。
//     public void onSaveGameFailed(string data)
//     {
//         if (OnSavedGameRead != null && OnSavedGameRead.GetInvocationList().Length > 0)
//         {
//             OnSavedGameRead(data);
//         }
//     }


//     /// <summary>
//     /// 显示视频广告的结果回调方法，SDK自动调用。
//     /// </summary>
//     /// <param name="data">返回的结果数据</param>
//     public void onReceiveReward(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             bool success = false;
//             int id = -1;
//             string tag = "Default";
//             bool skippedVideo = false;
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] results = data.Split('|');
//                 if (results != null && results.Length > 1)
//                 {
//                     success = int.Parse(results[0]) == 0;
//                     id = int.Parse(results[1]);
//                     if (results.Length > 2)
//                     {
//                         tag = results[2];
//                         if (results.Length > 3)
//                         {
//                             skippedVideo = int.Parse(results[3]) == 0 ? true : false;
//                         }
//                     }
//                 }
//             }

//             Debug.Log($"**** onReceiveReward {data}");
//             Debug.Log($"**** onReceiveReward {success}");
//             if (success)
//             {
//                 OnAdEvent(RiseSdk.AdEventType.RewardAdShowFinished, id, tag, RiseSdk.ADTYPE_VIDEO);
//             }
//             else
//             {
//                 OnAdEvent(RiseSdk.AdEventType.RewardAdShowFailed, id, tag, RiseSdk.ADTYPE_VIDEO);
//             }
//         }
//     }

//     /// <summary>
//     /// 大屏广告被关闭的回调方法，SDK自动调用。
//     /// </summary>
//     /// <param name="data">返回的数据</param>
//     public void onFullAdClosed(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             string tag = "Default";
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] msg = data.Split('|');
//                 if (msg != null && msg.Length > 0)
//                 {
//                     tag = msg[0];
//                 }
//             }

//             OnAdEvent(RiseSdk.AdEventType.FullAdClosed, -1, tag, RiseSdk.ADTYPE_INTERTITIAL);
//         }
//     }

//     /// <summary>
//     /// 大屏广告被点击的回调方法，SDK自动调用。    
//     /// </summary>
//     /// <param name="data">返回的数据</param>
//     public void onFullAdClicked(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             string tag = "Default";
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] msg = data.Split('|');
//                 if (msg != null && msg.Length > 0)
//                 {
//                     tag = msg[0];
//                 }
//             }

//             OnAdEvent(RiseSdk.AdEventType.FullAdClicked, -1, tag, RiseSdk.ADTYPE_INTERTITIAL);
//         }
//     }

//     /// <summary>
//     /// 大屏广告展示成功的回调方法，SDK自动调用。
//     /// </summary>
//     /// <param name="data">返回的数据</param>
//     public void onAdShow(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             string tag = "Default";
//             int type = RiseSdk.ADTYPE_INTERTITIAL;
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] msg = data.Split('|');
//                 if (msg != null && msg.Length > 1)
//                 {
//                     int.TryParse(msg[0], out type);
//                     tag = msg[1];
//                 }
//             }

//             RiseSdk.AdEventType eventType = RiseSdk.AdEventType.FullAdClicked;
//             switch (type)
//             {
//                 case RiseSdk.ADTYPE_INTERTITIAL:
//                     eventType = RiseSdk.AdEventType.FullAdShown;
//                     break;
//                 case RiseSdk.ADTYPE_VIDEO:
//                     eventType = RiseSdk.AdEventType.RewardAdShown;
//                     break;
//                 case RiseSdk.ADTYPE_BANNER:
//                 case RiseSdk.ADTYPE_ICON:
//                 case RiseSdk.ADTYPE_NATIVE:
//                     eventType = RiseSdk.AdEventType.AdShown;
//                     break;
//             }

//             OnAdEvent(eventType, -1, tag, type);
//         }
//     }

//     /// <summary>
//     /// 大屏广告被点击的回调方法，SDK自动调用。    
//     /// </summary
//     /// <param name="data">返回的数据</param>
//     public void onAdClicked(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             string tag = "Default";
//             int adType = RiseSdk.ADTYPE_INTERTITIAL;
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] msg = data.Split('|');
//                 if (msg != null && msg.Length > 1)
//                 {
//                     int.TryParse(msg[0], out adType);
//                     tag = msg[1];
//                 }
//             }

//             RiseSdk.AdEventType eventType = RiseSdk.AdEventType.FullAdClicked;
//             switch (adType)
//             {
//                 case RiseSdk.ADTYPE_INTERTITIAL:
//                     eventType = RiseSdk.AdEventType.FullAdClicked;
//                     break;
//                 case RiseSdk.ADTYPE_VIDEO:
//                     eventType = RiseSdk.AdEventType.RewardAdClicked;
//                     break;
//                 case RiseSdk.ADTYPE_BANNER:
//                     eventType = RiseSdk.AdEventType.BannerAdClicked;
//                     break;
//                 case RiseSdk.ADTYPE_ICON:
//                     eventType = RiseSdk.AdEventType.IconAdClicked;
//                     break;
//                 case RiseSdk.ADTYPE_NATIVE:
//                     eventType = RiseSdk.AdEventType.NativeAdClicked;
//                     break;
//             }

//             //OnAdEvent (RiseSdk.AdEventType.AdClicked, -1, tag, adType);
//             OnAdEvent(eventType, -1, tag, adType);
//         }
//     }

//     /// <summary>
//     /// 视频广告被关闭的回调方法，SDK自动调用。
//     /// </summary>
//     /// <param name="data">返回的数据</param>
//     public void onVideoAdClosed(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             string tag = "Default";
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] msg = data.Split('|');
//                 if (msg != null && msg.Length > 0)
//                 {
//                     tag = msg[0];
//                 }
//             }

//             OnAdEvent(RiseSdk.AdEventType.RewardAdClosed, -1, tag, RiseSdk.ADTYPE_VIDEO);
//         }
//     }

//     /// <summary>
//     /// banner广告被点击的回调方法，SDK自动调用。
//     /// </summary>
//     /// <param name="data">返回的数据</param>
//     public void onBannerAdClicked(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             string tag = "Default";
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] msg = data.Split('|');
//                 if (msg != null && msg.Length > 0)
//                 {
//                     tag = msg[0];
//                 }
//             }

//             OnAdEvent(RiseSdk.AdEventType.BannerAdClicked, -1, tag, RiseSdk.ADTYPE_BANNER);
//         }
//     }

//     /// <summary>
//     /// 交叉推广广告被点击的回调方法，SDK自动调用。
//     /// </summary>
//     /// <param name="data">返回的数据</param>
//     public void onCrossAdClicked(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             string tag = "Default";
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] msg = data.Split('|');
//                 if (msg != null && msg.Length > 0)
//                 {
//                     tag = msg[0];
//                 }
//             }

//             OnAdEvent(RiseSdk.AdEventType.CrossAdClicked, -1, tag, RiseSdk.ADTYPE_OTHER);
//         }
//     }

//     /// <summary>
//     /// 视频加载的回调方法，SDK自动调用。
//     /// </summary>
//     /// <param name="data">返回的数据</param>
//     public void adLoaded(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             string tag = "Default";
//             int adType = -1;
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] str = data.Split('|');
//                 if (str.Length == 1)
//                 {
//                     tag = str[0];
//                 }
//                 else if (str.Length >= 2)
//                 {
//                     tag = str[0];
//                     int.TryParse(str[1], out adType);
//                 }
//             }

//             OnAdEvent(RiseSdk.AdEventType.AdLoadCompleted, -1, tag, adType);
//         }
//     }


//     public void onInAppMessageEvent(string data)
//     {
//         if (OnInAppMessageEvent != null && OnInAppMessageEvent.GetInvocationList().Length > 0)
//         {
//             OnInAppMessageEvent(data);
//         }
//     }

//     public void unreadMessageCount(string str)
//     {
//         if (OnAIHelpUnreadMessageEvent != null && OnAIHelpUnreadMessageEvent.GetInvocationList().Length > 0)
//         {
//             int.TryParse(str, out var count);
//             OnAIHelpUnreadMessageEvent(count);
//         }
//     }
// #elif UNITY_IOS || PLATFORM_IOS ||CN_IOS_Normal
//     /// <summary>
//     /// 大屏和视频广告的回调事件
//     /// 1.RiseSdk.AdEventType
//     /// 2.rewardId
//     /// 3.ad tag
//     /// 4.RiseSdk.ADTYPE_
//     /// 5.video skipped  //max 4 param limited.
//     /// </summary>
//     public static event Action<RiseSdk.AdEventType, int, string, int> OnAdEvent;



//     ///// <summary>
//     ///// 视频广告的回调事件
//     ///// </summary>
//     //public static event Action<RiseSdk.AdEventType, int, string> OnRewardAdEvent;

//     /// <summary>
//     /// 支付的回调事件
//     /// </summary>
//     public static event Action<RiseSdk.PaymentResult, int, string> OnPaymentEvent;

//     //支付接口初始化成功回调
//     public static event Action<string> OnStoreLoaded;

//     /// <summary>
//     /// 有payload的支付结果回调
//     /// </summary>
//     public static event Action<RiseSdk.PaymentResult, int, string, string> OnPaymentWithPayloadEvent;

//     /// <summary>
//     /// 网络状态变化回调：ios返回StatusUnknown或StatusNotReachable或StatusReachableViaWWAN或StatusReachableViaWiFi
//     /// </summary>
//     public static event Action<RiseSdk.NetworkStatus> OnNetworkChangedEvent;

//     public static event Action<int, long> OnCheckSubscriptionResult;
//     public static event Action OnRestoreFailureEvent;
//     public static event Action<int> OnRestoreSuccessEvent;
//     public static event Action<RiseSdk.SnsEventType, int> OnSNSEvent;
//     /// <summary>
//     /// 获取后台自定义json数据的结果回调事件
//     /// </summary>
//     public static event Action<string> OnReceiveServerExtra;

//     public static event Action<string> OnGameCenterLoginSuccessEvent;
//     public static event Action OnGameCenterLoginFailureEvent;
//     public static event Action<bool> OnNotificationOpen;
//     public static event Action<bool> OnAttAccepted;
//     /// <summary>
//     /// 获取通知栏消息的结果回调事件
//     /// </summary>
//     public static event Action<string> OnReceiveNotificationData;

//     public static event Action<string> OnSavedGameRead;

//     public static event Action<string> OnSaveGameSuccess;

//     public static bool saveGameReadFlag = false;

//     public static event Action<string> OnChatMessage;

//     public static event Action<string> OnFirestoreReadData;

//     public static event Action<string> OnFirestoreReadFail;

//     public static event Action<string> OnFirestoreUpdateSuccess;

//     public static event Action<string> OnFirestoreConnected;

//     public static event Action<string> OnFirestoreConnectError;

//     public static event Action<string> OnFirestoreLinkError;

//     public static event Action<string> OnFirestoreUpdateFail;

//     public static event Action<string> OnFirestoreMergeSuccess;

//     public static event Action<string> OnFirestoreMergeFail;

//     public static event Action<string> OnFirestoreSetSuccess;

//     public static event Action<string> OnFirestoreSetFail;

//     public static event Action<string> OnPaymentData;

//     public static event Action<string> OnShareSuccess;

//     //InAppMessage运营活动推送
//     public static event Action<string> OnInAppMessageDisplayed;

// #if  !CN_IOS_Normal //移到最下面
//         public static event Action<string> OnCloudFunctionResult;

//         public static event Action<string> OnCloudFunctionFailed;
// #endif


//     public static event Action<string> OnFirestoreSnapshot;

//     public static event Action<string> SignInAppleSuccess;
//     public static event Action<string> SignInAppleFailure;

//     public static event Action<string> OnLoginAppleSuccess;
//     public static event Action<string> OnLoginAppleFailed;

//     private static RiseSdkListener _instance;
//     public static event Action OnResumeAdEvent;

//     public static event Action<string> OnInAppMessageEvent;
//     public static event Action<int> OnAIHelpUnreadMessageEvent;

//     public static RiseSdkListener Instance
//     {
//         get
//         {
//             if (!_instance)
//             {
//                 // check if there is a IceTimer instance already available in the scene graph
//                 _instance = FindObjectOfType(typeof(RiseSdkListener)) as RiseSdkListener;
//                 // nope, create a new one
//                 if (!_instance)
//                 {
//                     var obj = new GameObject("RiseSdkListener");
//                     _instance = obj.AddComponent<RiseSdkListener>();
//                     DontDestroyOnLoad(obj);
//                 }
//             }
//             return _instance;
//         }
//     }

//     public void adReward(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {

//             string tag = "Default";
//             int placementId = -1;
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] str = data.Split('|');
//                 if (str.Length == 1)
//                 {
//                     tag = str[0];
//                 }
//                 else if (str.Length >= 2)
//                 {
//                     tag = str[0];
//                     int.TryParse(str[1], out placementId);
//                 }
//             }
//             OnAdEvent(RiseSdk.AdEventType.RewardAdShowFinished, placementId, tag, RiseSdk.ADTYPE_VIDEO);
//             Debug.Log("adReward : " + data);
//         }
//     }

//     public void adLoaded(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             string tag = "Default";
//             int adType = -1;
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] str = data.Split('|');
//                 if (str.Length == 1)
//                 {
//                     tag = str[0];
//                 }
//                 else if (str.Length >= 2)
//                 {
//                     tag = str[0];
//                     int.TryParse(str[1], out adType);
//                 }
//             }
//             OnAdEvent(RiseSdk.AdEventType.AdLoadCompleted, -1, tag, adType);
//         }
//     }

//     public void adShowFailed(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             string tag = "Default";
//             int adType = -1;
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] str = data.Split('|');
//                 if (str.Length == 1)
//                 {
//                     tag = str[0];
//                 }
//                 else if (str.Length >= 2)
//                 {
//                     tag = str[0];
//                     int.TryParse(str[1], out adType);
//                 }
//             }
//             OnAdEvent(RiseSdk.AdEventType.AdLoadFailed, -1, tag, adType);
//         }
//     }

//     public void adDidShown(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             string tag = "Default";
//             int adType = -1;
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] str = data.Split('|');
//                 if (str.Length == 1)
//                 {
//                     tag = str[0];
//                 }
//                 else if (str.Length >= 2)
//                 {
//                     tag = str[0];
//                     int.TryParse(str[1], out adType);
//                 }
//             }
//             OnAdEvent(RiseSdk.AdEventType.AdShown, -1, tag, adType);
//         }
//     }

//     public void adDidClose(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             string tag = "Default";
//             int adType = -1;
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] str = data.Split('|');
//                 if (str.Length == 1)
//                 {
//                     tag = str[0];
//                 }
//                 else if (str.Length >= 2)
//                 {
//                     tag = str[0];
//                     int.TryParse(str[1], out adType);
//                 }
//             }
//             OnAdEvent(RiseSdk.AdEventType.AdClosed, -1, tag, adType);
//         }
//     }

//     public void adDidClick(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             string tag = "Default";
//             int adType = RiseSdk.ADTYPE_INTERTITIAL;
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] str = data.Split('|');
//                 if (str.Length == 1)
//                 {
//                     tag = str[0];
//                 }
//                 else if (str.Length >= 2)
//                 {
//                     tag = str[0];
//                     int.TryParse(str[1], out adType);
//                 }
//             }
//             RiseSdk.AdEventType eventType = RiseSdk.AdEventType.FullAdClicked;
//             switch (adType)
//             {
//                 case RiseSdk.ADTYPE_INTERTITIAL:
//                     eventType = RiseSdk.AdEventType.FullAdClicked;
//                     break;
//                 case RiseSdk.ADTYPE_VIDEO:
//                     eventType = RiseSdk.AdEventType.RewardAdClicked;
//                     break;
//                 case RiseSdk.ADTYPE_BANNER:
//                     eventType = RiseSdk.AdEventType.BannerAdClicked;
//                     break;
//                 case RiseSdk.ADTYPE_ICON:
//                     eventType = RiseSdk.AdEventType.IconAdClicked;
//                     break;
//                 case RiseSdk.ADTYPE_NATIVE:
//                     eventType = RiseSdk.AdEventType.NativeAdClicked;
//                     break;
//             }
//             OnAdEvent(eventType, -1, tag, adType);
//         }
//     }

//     public void onInitialized(string msg)
//     {
//         Debug.Log("wsq=== onInitialized!");
//     }

//     /// <summary>
//     /// 网络状态变化的回调
//     /// </summary>
//     /// <param name="data">返回-1或0或1或2</param>
//     public void onNetworkChanged(string data)
//     {
//         Debug.Log("wsq=== onNetworkChanged : " + data);
//         if (OnNetworkChangedEvent != null && OnNetworkChangedEvent.GetInvocationList().Length > 0)
//         {
//             int status = 0;
//             if (int.TryParse(data, out status))
//             {
//                 RiseSdk.NetworkStatus networkStatus = RiseSdk.NetworkStatus.StatusNotReachable;
//                 try
//                 {
//                     networkStatus = (RiseSdk.NetworkStatus)status;
//                 }
//                 catch (Exception e)
//                 {
//                     Debug.LogError("parse network status error:::" + e.StackTrace);
//                 }
//                 finally
//                 {
//                     OnNetworkChangedEvent(networkStatus);
//                 }
//             }
//         }
//     }

//     /// <summary>
//     /// 支付初始化建立时，SDK自动调用。
//     /// </summary>
//     public void onStoreLoaded(string type)
//     {
//         if (OnStoreLoaded != null && OnStoreLoaded.GetInvocationList().Length > 0)
//         {
//             OnStoreLoaded(type);
//         }
//     }


//     /// <summary>
//     /// 支付成功结果回调方法，SDK自动调用。
//     /// </summary>
//     /// <param name="data"> {PaymentCallBackData} </param>
//     public void onPaymentSuccess(string data)
//     {
//         if (string.IsNullOrEmpty(data))
//             return;
//         RiseSdkData.PaymentData resData;
//         try
//         {
//             resData = JsonConvert.DeserializeObject<RiseSdkData.PaymentData>(data);
//         }
//         catch
//         {
//             return;
//         }
//         OnPaymentEvent?.Invoke(RiseSdk.PaymentResult.Success, resData.billId, resData.orderId);
//     }
//     // public void onPaymentSuccess(string billingId)
//     // {
//     //     if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList().Length > 0)
//     //     {
//     //         OnPaymentEvent(RiseSdk.PaymentResult.Success, int.Parse(billingId));
//     //     }
//     //     else
//     //     {
//     //         if (!string.IsNullOrEmpty(billingId))
//     //         {
//     //             if (LastPaySuccessData == null)
//     //                 LastPaySuccessData = new List<string>();
//     //             LastPaySuccessData.Add(billingId);
//     //         }
//     //     }
//     // }

//     /// <summary>
//     /// payload支付成功结果回调，SDK自动调用。
//     /// </summary>
//     /// <param name="data"></param>
//     public void onPaymentSuccessWithPayload(string data)
//     {
//         if (string.IsNullOrEmpty(data))
//             return;
//         RiseSdkData.PaymentData resData;
//         try
//         {
//             resData = JsonConvert.DeserializeObject<RiseSdkData.PaymentData>(data);
//         }
//         catch
//         {
//             return;
//         }
//         OnPaymentWithPayloadEvent?.Invoke(RiseSdk.PaymentResult.Success, resData.billId, resData.payload,
//             resData.orderId);
//     }
//     // public void onPaymentSuccessWithPayload(string data)
//     // {
//     //     if (OnPaymentWithPayloadEvent != null && OnPaymentWithPayloadEvent.GetInvocationList().Length > 0)
//     //     {
//     //         if (!string.IsNullOrEmpty(data))
//     //         {
//     //             string[] strArray = data.Split('|');
//     //             int id = 0;
//     //             if (strArray.Length > 1 && int.TryParse(strArray[0], out id))
//     //             {
//     //                 OnPaymentWithPayloadEvent(RiseSdk.PaymentResult.Success, id, strArray[1]);
//     //             }
//     //         }
//     //     }
//     //      else
//     //     {
//     //         if (!string.IsNullOrEmpty(data))
//     //         {
//     //             if (LastPayWithLoadSuccessData == null)
//     //                 LastPayWithLoadSuccessData = new List<string>();
//     //             LastPayWithLoadSuccessData.Add(data);
//     //         }
//     //     }
//     // }


//     /// <summary>
//     /// 支付失败结果回调方法，SDK自动调用。
//     /// </summary>
//     /// <param name="billingId">计费点Id</param>
//     public void onPaymentFailure(string billingId)
//     {
//         OnPaymentEvent?.Invoke(RiseSdk.PaymentResult.Failed, int.Parse(billingId), "");
//         OnPaymentWithPayloadEvent?.Invoke(RiseSdk.PaymentResult.Failed, int.Parse(billingId), "", "");
//     }
//     // public void onPaymentFailure(string billingId)
//     // {
//     //     if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList().Length > 0)
//     //     {
//     //         OnPaymentEvent(RiseSdk.PaymentResult.Failed, int.Parse(billingId));
//     //     }
//     //     if (OnPaymentWithPayloadEvent != null && OnPaymentWithPayloadEvent.GetInvocationList().Length > 0)
//     //     {
//     //         OnPaymentWithPayloadEvent(RiseSdk.PaymentResult.Failed, int.Parse(billingId), null);
//     //     }
//     // }


//     /// <summary>
//     /// 显示视频广告的结果回调方法，SDK自动调用。
//     /// </summary>
//     /// <param name="data">返回的结果数据</param>
//     public void onReceiveReward(string data)
//     {
//         if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
//         {
//             bool success = false;
//             int id = -1;
//             string tag = "Default";
//             bool skippedVideo = false;
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] results = data.Split('|');
//                 if (results != null && results.Length > 1)
//                 {
//                     success = int.Parse(results[0]) == 0;
//                     id = int.Parse(results[1]);
//                     if (results.Length > 2)
//                     {
//                         tag = results[2];
//                         if (results.Length > 3)
//                         {
//                             skippedVideo = int.Parse(results[3]) == 0 ? true : false;
//                         }
//                     }
//                 }
//             }

//             Debug.Log($"**** onReceiveReward {data}");
//             Debug.Log($"**** onReceiveReward {success}");
//             if (success)
//             {
//                 OnAdEvent(RiseSdk.AdEventType.RewardAdShowFinished, id, tag, RiseSdk.ADTYPE_VIDEO);
//             }
//             else
//             {
//                 OnAdEvent(RiseSdk.AdEventType.RewardAdShowFailed, id, tag, RiseSdk.ADTYPE_VIDEO);
//             }
//         }
//     }

//     public void onCheckSubscriptionResult(string data)
//     {
//         if (OnCheckSubscriptionResult != null && OnCheckSubscriptionResult.GetInvocationList().Length > 0)
//         {
//             int billingId = -1;
//             long remainSeconds = 0;
//             if (!string.IsNullOrEmpty(data))
//             {
//                 string[] str = data.Split(',');
//                 if (str.Length >= 2)
//                 {
//                     billingId = int.Parse(str[0]);
//                     remainSeconds = long.Parse(str[1]);
//                 }
//             }
//             OnCheckSubscriptionResult(billingId, remainSeconds);
//         }
//     }

//     public void onRestoreFailure(string error)
//     {
//         if (OnRestoreFailureEvent != null && OnRestoreFailureEvent.GetInvocationList().Length > 0)
//         {
//             OnRestoreFailureEvent();
//         }
//     }

//     public void onRestoreSuccess(string billingId)
//     {
//         if (OnRestoreSuccessEvent != null && OnRestoreSuccessEvent.GetInvocationList().Length > 0)
//         {
//             OnRestoreSuccessEvent(int.Parse(billingId));
//         }
//     }

//     public void snsShareSuccess(string data)
//     {
//         if (OnSNSEvent != null && OnSNSEvent.GetInvocationList().Length > 0)
//         {
//             OnSNSEvent(RiseSdk.SnsEventType.ShareSuccess, 0);
//         }
//     }

//     public void snsShareFailure(string data)
//     {
//         if (OnSNSEvent != null && OnSNSEvent.GetInvocationList().Length > 0)
//         {
//             OnSNSEvent(RiseSdk.SnsEventType.ShareFailed, 0);
//         }
//     }

//     public void snsShareCancel(string data)
//     {
//         if (OnSNSEvent != null && OnSNSEvent.GetInvocationList().Length > 0)
//         {
//             OnSNSEvent(RiseSdk.SnsEventType.ShareCancel, 0);
//         }
//     }

//     public void snsLoginSuccess(string data)
//     {
//         if (OnSNSEvent != null && OnSNSEvent.GetInvocationList().Length > 0)
//         {
//             OnSNSEvent(RiseSdk.SnsEventType.LoginSuccess, 0);
//         }
//     }

//     public void snsLoginFailure(string data)
//     {
//         if (OnSNSEvent != null && OnSNSEvent.GetInvocationList().Length > 0)
//         {
//             OnSNSEvent(RiseSdk.SnsEventType.LoginFailed, 0);
//         }
//     }
//     /// <summary>
//     /// 获取后台配置的自定义json数据的回调。当SDK初始化完成，第一次取到数据后会自动调用该方法，如果需要可以提前添加监听。
//     /// </summary>
//     /// <param name="data">后台配置的自定义json数据，如：{"x":"x", "x":8, "x":{x}, "x":[x]}</param>
//     public void onReceiveServerExtra(string data)
//     {
//         if (OnReceiveServerExtra != null && OnReceiveServerExtra.GetInvocationList().Length > 0)
//         {
//             OnReceiveServerExtra(data);
//         }
//     }

//     /// <summary>
//     /// 获取到通知栏消息数据的回调。当点击通知栏消息打开应用时，会自动调用该方法，如果需要可以提前添加监听。
//     /// </summary>
//     /// <param name="data">后台配置的数据</param>
//     public void onReceiveNotificationData(string data)
//     {
//         if (OnReceiveNotificationData != null && OnReceiveNotificationData.GetInvocationList().Length > 0)
//         {
//             OnReceiveNotificationData(data);
//         }
//     }


//     //选择数据成功
//     public void onSavedGameRead(string data)
//     {
//         if (OnSavedGameRead != null && OnSavedGameRead.GetInvocationList().Length > 0)
//         {
//             OnSavedGameRead(data);
//         }
//     }

//     //上传数据成功
//     public void onSaveGameSuccess(string data)
//     {
//         if (OnSaveGameSuccess != null && OnSaveGameSuccess.GetInvocationList().Length > 0)
//         {
//             OnSaveGameSuccess(data);
//         }
//     }


//     //上传数据或者选择数据失败  提示  操作失败，请稍后再试。。。
//     public void onSaveGameFailed(string data)
//     {
//         if (OnSavedGameRead != null && OnSavedGameRead.GetInvocationList().Length > 0)
//         {
//             OnSavedGameRead(data);
//         }
//     }

//     public void onChatMessage(string jsonData)
//     {
//         if (OnChatMessage != null && OnChatMessage.GetInvocationList().Length > 0)
//         {
//             OnChatMessage(jsonData);
//         }
//     }


//     public void signInAppleSuccess(string msg)
//     {
//         if (SignInAppleSuccess != null && SignInAppleSuccess.GetInvocationList().Length > 0)
//         {
//             SignInAppleSuccess(msg);
//         }
//     }
//     public void signInAppleFailure(string msg)
//     {
//         if (SignInAppleFailure != null && SignInAppleFailure.GetInvocationList().Length > 0)
//         {
//             SignInAppleFailure(msg);
//         }
//     }
//     public void onLoginAppleSuccess(string msg)
//     {
//         if (OnLoginAppleSuccess != null && OnLoginAppleSuccess.GetInvocationList().Length > 0)
//         {
//             OnLoginAppleSuccess(msg);
//         }
//     }
//     public void onLoginAppleFailed(string msg)
//     {
//         if (OnLoginAppleFailed != null && OnLoginAppleFailed.GetInvocationList().Length > 0)
//         {
//             OnLoginAppleFailed(msg);
//         }
//     }

//     public void onInAppMessageEvent(string data)
//     {
//         if (OnInAppMessageEvent != null && OnInAppMessageEvent.GetInvocationList().Length > 0)
//         {
//             OnInAppMessageEvent(data);
//         }
//     }

//     public void unreadMessageCount(string str)
//     {
//         if (OnAIHelpUnreadMessageEvent != null && OnAIHelpUnreadMessageEvent.GetInvocationList().Length > 0)
//         {
//             int.TryParse(str, out var count);
//             OnAIHelpUnreadMessageEvent(count);
//         }
//     }


//     //支付数据返回
//     public void onPaymentData(string data)
//     {
//         if (OnPaymentData != null && OnPaymentData.GetInvocationList().Length > 0)
//         {
//             OnPaymentData(data);
//         }
//     }

//     public void onShareSuccess(string data)
//     {
//         if (OnShareSuccess != null && OnShareSuccess.GetInvocationList().Length > 0)
//         {
//             OnShareSuccess?.Invoke(data);
//         }
//     }

//     /// <summary>
//     /// 接收InAppMessage活动推送
//     /// </summary>
//     /// <param name="json"></param>
//     public void onInAppMessageDisplayed(string json)
//     {
//         if (OnInAppMessageDisplayed != null && OnInAppMessageDisplayed.GetInvocationList().Length > 0)
//         {
//             OnInAppMessageDisplayed?.Invoke(json);
//         }
//     }

//     public void onGameCenterLoginSuccess(string playerId)
//     {
//         if (OnGameCenterLoginSuccessEvent != null && OnGameCenterLoginSuccessEvent.GetInvocationList().Length > 0)
//         {
//             OnGameCenterLoginSuccessEvent(playerId);
//         }
//     }

//     public void onGameCenterLoginFailure(string msg)
//     {
//         if (OnGameCenterLoginFailureEvent != null && OnGameCenterLoginFailureEvent.GetInvocationList().Length > 0)
//         {
//             OnGameCenterLoginFailureEvent();
//         }
//     }

//     public void onNotificationOpen(string isOpen)
//     {
//         if (OnNotificationOpen != null && OnNotificationOpen.GetInvocationList().Length > 0)
//         {
//             OnNotificationOpen(isOpen.Equals("0"));
//         }
//     }

//     public void onAttAccepted(string result)
//     {
//         if (OnAttAccepted != null && OnAttAccepted.GetInvocationList().Length > 0)
//         {
//             OnAttAccepted(result.Equals("0"));
//         }
//     }
// #endif
// }

// /// <summary> 国内部分 </summary>
// public partial class RiseSdkListener
// {
//     private static void LogX(string msg)
//     {
// #if DEBUG_LOG || UNITY_EDITOR || GameTest
//         UnityEngine.Debug.Log($"o.O --- {msg}");
// #endif
//     }

//     #region 隐私及初始化

//     public static event Action<bool> Event_OnUserProtocol;

//     public void onUserProtocol(string result)
//     {
//         LogX($"onUserProtocol {result}");
//         var res = RiseSdkConst.SDK_CALLBACK_success.Equals(result);
//         Event_OnUserProtocol?.Invoke(res);
//     }

//     #endregion

//     #region 登陆

//     public static event Action<bool> Event_CheckPreLoginState;

//     /// <summary> 查询登陆状态 </summary>
//     public void onWXPreLogState(string result)
//     {
//         LogX($"onWXPreLogState(isLogin result) {result}");
//         var res = RiseSdkConst.SDK_CALLBACK_success.Equals(result);
//         Event_CheckPreLoginState?.Invoke(res);
//     }

//     /// <summary> state </summary>
//     public static event Action<int, string> Event_LoginResult;

//     /// <summary> state </summary>
//     public static event Action<int> VerifyDelegate;

//     /// <param name="result"> 无实际意义 </param>
//     public void onWXLogSuccess(string result)
//     {
//         LogX($"onWXLogSuccess {result}");
//         Event_LoginResult?.Invoke(0, result); // 0表示 Success 给外部枚举使用 (LoginState)
//     }

//     /// <param name="result"> 可传参失败原因 </param>
//     public void onWXLogFailed(string result)
//     {
//         LogX($"onWXLogFailed {result}");
//         Event_LoginResult?.Invoke(1, result); // 1表示 fail 给外部枚举使用 (LoginState)
//     }

//     public void onWXUninstall(string result)
//     {
//         LogX($"onWXUninstall {result}");
//         Event_LoginResult?.Invoke(2, result); // 2表示 Uninstall 给外部枚举使用 (LoginState)
//     }

//     public void IdVerifySuccess(string result)
//     {
//         LogX($"IdVerifySuccess {result}");
//         VerifyDelegate?.Invoke(0); // 0表示 Success 给外部枚举使用 (IdVerifyState)
//     }

//     public void IdVerifyFailed(string result)
//     {
//         LogX($"IdVerifyFailed {result}");
//         VerifyDelegate?.Invoke(1); // 0表示 fail 给外部枚举使用 (IdVerifyState)
//     }

//     public void IdVerifyCancel(string result)
//     {
//         LogX($"IdVerifyCancel {result}");
//         VerifyDelegate?.Invoke(2); // 0表示 Cancel 给外部枚举使用 (IdVerifyState)
//     }

//     #endregion

//     public static event Action Event_RequireWXLogin;

//     public void RequireWXLogin(string result)
//     {
//         LogX($"RequireWXLogin {result}");
//         Event_RequireWXLogin?.Invoke();
//     }

//     #region 云函数

//     public static event Action<string, string> OnCloudFunctionResult;

//     public static event Action<string> OnCloudFunctionFailed;

//     private Dictionary<string, List<RiseSdkData.CloudFuncData>> cloudFuncTempDict = new Dictionary<string, List<RiseSdkData.CloudFuncData>>();

//     public void cloudFuncSuccess(string result)
//     {
//         // CloudFuncData
//         LogX($"cloudFuncSuccess {result}");

//         var data = JsonConvert.DeserializeObject<RiseSdkData.CloudFuncData>(result);

//         string funcKey = data.funcKey;
//         int count = data.count;
//         if (cloudFuncTempDict.TryGetValue(funcKey, out var list))
//         {
//             list.Add(data);
//         }
//         else
//         {
//             list = new List<RiseSdkData.CloudFuncData>();
//             list.Add(data);
//             cloudFuncTempDict.Add(funcKey, list);
//         }

//         if (count <= list.Count)
//         {
//             list.Sort((a, b) => a.index.CompareTo(b.index));
//             StringBuilder sb = new StringBuilder();
//             foreach (var val in list)
//             {
//                 sb.Append(val.data);
//             }

//             OnCloudFunctionResult?.Invoke(funcKey, sb.ToString());
//             cloudFuncTempDict.Remove(funcKey);
//         }
//         else
//         {
//         }
//     }

//     public void cloudFuncFail(string result)
//     {
//         // funcKey
//         LogX($"cloudFuncFail {result}");
//         OnCloudFunctionFailed?.Invoke(result);
//     }

//     #endregion


//     #region 抖音小游戏相关的回调

//     public static event Action<Dictionary<string, object>> Event_OnShowWithDict;

//     public void OnShowWithDict(Dictionary<string, object> param)
//     {
//         Debug.Log($"OnShowWithDict param:{JsonConvert.SerializeObject(param)}");
//         Event_OnShowWithDict?.Invoke(param);
//     }

//     public static event Action Event_StartRecordScreenSuccess;
//     public static event Action<int, string> Event_StartRecordScreenError;
//     public static event Action<string> Event_StartRecordScreenTimeOut;

//     public void OnStartRecordScreenSuccess()
//     {
//         Debug.Log($"OnStartRecordScreenSuccess ");
//         Event_StartRecordScreenSuccess?.Invoke();
//     }

//     public void OnStartRecordScreenError(int code, string msg)
//     {
//         Debug.Log($"OnStartRecordScreenError code:{code}, msg:{msg}");
//         Event_StartRecordScreenError?.Invoke(code, msg);
//     }

//     public void OnStartRecordScreenTimeOut(string path)
//     {
//         Debug.Log($"OnStartRecordScreenTimeOut path:{path}");
//         Event_StartRecordScreenTimeOut?.Invoke(path);
//     }

//     public static event Action Event_StopRecordScreenComplete;
//     public static event Action<int, string> Event_StopRecordScreenError;

//     public void OnStopRecordScreenComplete()
//     {
//         Debug.Log("OnStopRecordScreenComplete");
//         Event_StopRecordScreenComplete?.Invoke();
//     }

//     public void OnStopRecordScreenError(int code, string msg)
//     {
//         Debug.Log($"OnStopRecordScreenError code:{code}, msg:{msg}");
//         Event_StopRecordScreenError?.Invoke(code, msg);
//     }

//     public static event Action<Dictionary<string, object>> Event_ShareVideoSuccess;
//     public static event Action<string> Event_ShareVideoFailed;
//     public static event Action Event_ShareVideoCancel;

//     public void OnShareVideoSuccess(Dictionary<string, object> result)
//     {
//         Debug.Log($"OnCheckSceneSuccess {JsonConvert.SerializeObject(result)}");
//         Event_ShareVideoSuccess?.Invoke(result);
//     }

//     public void OnShareVideoFailed(string msg)
//     {
//         Debug.Log($"OnShareVideoFailed {msg}");
//         Event_ShareVideoFailed?.Invoke(msg);
//     }

//     public void OnShareVideoCancel()
//     {
//         Debug.Log("OnShareVideoCancel");
//         Event_ShareVideoCancel?.Invoke();
//     }

//     public static event Action<bool> Event_CheckSceneSuccess;
//     public static event Action Event_CheckSceneComplete;
//     public static event Action<int, string> Event_CheckSceneError;

//     public void OnCheckSceneSuccess(bool result)
//     {
//         Debug.Log($"OnCheckSceneSuccess {result}");
//         Event_CheckSceneSuccess?.Invoke(result);
//     }

//     public void OnCheckSceneComplete()
//     {
//         Debug.Log($"OnCheckSceneComplete");
//         Event_CheckSceneComplete?.Invoke();
//     }

//     public void OnCheckSceneError(int code, string msg)
//     {
//         Debug.Log($"OnCheckSceneError code:{code}, msg:{msg}");
//         Event_CheckSceneError?.Invoke(code, msg);
//     }

//     public static event Action Event_NavigateToSceneSuccess;
//     public static event Action Event_NavigateToSceneComplete;
//     public static event Action<int, string> Event_NavigateToSceneError;

//     public void OnNavigateToSceneSuccess()
//     {
//         Debug.Log($"OnNavigateToSceneSuccess");
//         Event_NavigateToSceneSuccess?.Invoke();
//     }

//     public void OnNavigateToSceneComplete()
//     {
//         Debug.Log($"OnNavigateToSceneComplete");
//         Event_NavigateToSceneComplete?.Invoke();
//     }

//     public void OnNavigateToSceneError(int code, string msg)
//     {
//         Debug.Log($"OnNavigateToSceneError code:{code}, msg:{msg}");
//         Event_NavigateToSceneError?.Invoke(code, msg);
//     }

//     public static event Action<int, int, string> Event_ReplaceSensitiveWords;

//     // 屏蔽字
//     /// code      int: 返回状态码，0为成功，否则为失败
//     /// result    int     1: 有敏感内容；2: 无敏感内容
//     /// content   string  被替换的内容（无敏感内容则返回值与传入的word相同）
//     public void OnReplaceSensitiveWords(int code, int result, string content)
//     {
//         Event_ReplaceSensitiveWords?.Invoke(code, result, content);
//     }

//     public static event Action<string> Event_KeyboardInput;
//     public static event Action<string> Event_KeyboardConfirm;
//     public static event Action Event_KeyboardComplete;

//     public void OnKeyboardInput(string value)
//     {
//         Event_KeyboardInput?.Invoke(value);
//     }
//     public void OnKeyboardConfirm(string result)
//     {
//         Event_KeyboardConfirm?.Invoke(result);
//     }

//     public void OnKeyboardComplete()
//     {
//         Event_KeyboardComplete?.Invoke();
//     }

//     public static event Action<bool> Event_OnClipboardComplete;

//     public void OnClipboardComplete(bool result)
//     {
//         Event_OnClipboardComplete?.Invoke(result);
//     }

//     #endregion
// }
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ivy.MiniGame
{
    #region 回调数据定义

    [Serializable]
    //回调返回基类
    public class ActionResult
    {
        ///错误码
        [SerializeField] public int errcode;

        ///错误信息
        [SerializeField] public string errmsg;
    }

    ///登录回调
    [Serializable]
    public class LoginResult : ActionResult
    {
        /// <summary>
        /// userId
        /// </summary>
        [SerializeField] public string openid;

        /// <summary>
        /// 开放平台的通用统一id 目前站位用
        /// </summary>
        [SerializeField] public string unionid;
    }

    ///banner广告回调
    public class BannerAdResult : ActionResult
    {
    }

    ///视频广告回调
    [Serializable]
    public class RewardedVideoAdResult : ActionResult
    {
        [SerializeField] public string data;
    }

    ///插屏广告回调
    public class InterstitialAdResult : ActionResult
    {
    }

    #endregion

    #region 枚举定义

    public enum BannerPosition
    {
        /// <summary>
        /// 在左上角显示banner广告参数常量
        /// </summary>
        POS_LEFT_TOP = 1,

        /// <summary>
        /// 在顶部居中显示banner广告参数常量
        /// </summary>
        POS_MIDDLE_TOP = 3,

        /// <summary>
        /// 在右上角显示banner广告参数常量
        /// </summary>
        POS_RIGHT_TOP = 6,

        /// <summary>
        /// 在中间居中显示banner广告参数常量
        /// </summary>
        POS_MIDDLE_MIDDLE = 5,

        /// <summary>
        /// 在左下角显示banner广告参数常量
        /// </summary>
        POS_LEFT_BOTTOM = 2,

        /// <summary>
        /// 在底部居中显示banner广告参数常量
        /// </summary>
        POS_MIDDLE_BOTTOM = 4,

        /// <summary>
        /// 在右下角显示banner广告参数常量
        /// </summary>
        POS_RIGHT_BOTTOM = 7,

        POS_LEFT_MIDDLE = 8,
        POS_RIGHT_MIDDLE = 9,
    }

    #endregion

    internal partial interface MiniSdkInterface
    {
        public void InitSdk(Action<int> ret);

        public void ReportGameStart();

        #region 登录

        public string Me();

        public void IsLogin(Action<bool> preLogin);

        public void Login(Action<string> loginSuccess, Action<string> loginFail);

        #endregion

        #region 本地存储

        bool HasLocalKey(string key);

        void DeleteLocalKey(string key);
        public void DeleteAll();
        void SetLocalString(string key, string value = "");

        string GetLocalStringSync(string key, string defaultValue);

        void SetLocalInt(string key, int value);

        int GetLocalIntSync(string key, int defaultValue);

        void SetLocalFloat(string key, float value);

        float GetLocalFloatSync(string key, float defaultValue);

        #endregion

        #region 震动

        public void VibratePhone(bool isShort);

        #endregion

        public void SetClipboardData(string text, Action<bool> result);
        
        public Dictionary<string, object> GetSystemInfo();
        public bool IsIOSSystem();
        public Vector3 GetHorizonSafeAreaOffsetAndMenuOffset();
        
        
    }

#if CN_DY
    //抖音渠道的专属接口
    internal partial interface MiniSdkInterface
    {
        //开始录屏
        public void StartRecordScreen(bool isRecordAudio, int maxRecordTime, Action onStartCallback, Action<int, string> onErrorCallback, 
            Action<string> onTimeOutCallback);
        //结束录屏
        public void StopRecordScreen(Action onCompleteCallback, Action<int, string> onErrorCallback);
        public int GetRecordDuration();
        //分享
        public void ShareVideo(Action<Dictionary<string, object>> onSuccessCallback, Action<string> onFailedCallback, Action onCancelledCallback, StarkSDKSpace.UNBridgeLib.LitJson.JsonData shareJson);
        //侧边栏
        public void CheckScene(Action<bool> success, Action complete, Action<int, string> error);
        public void NavigateToScene(Action success, Action complete, Action<int, string> error);
        public bool IsFromSidebarCard();

        public void OpenCustomerServicePage(Action<bool> onComplete);
        public void OpenAwemeCustomerService(StarkSDKSpace.UNBridgeLib.LitJson.JsonData options, Action onSuccess, Action<int, string> onFail);

        public void ReplaceSensitiveWords(string word, Action<int, string, StarkSDKSpace.UNBridgeLib.LitJson.JsonData> callback);
        public void ShowKeyboard(Action<string> onKeyboardConfirmEvent);
        public void HideKeyboard(Action onKeyboardCompleteEvent = null);
        public void OnKeyboardInput(Action<string> onKeyboardInputEvent);
    }
#elif CN_WX
    //微信渠道的专属接口
    internal partial interface MiniSdkInterface
    {
        public void ShowKeyboard(string defaultValue, int maxLength, Action<string> onInput,
            Action<string> onConfirm, Action onComplete);
        public void HideKeyboard();
        
        public void OpenCustomerServicePage(Action<bool> onComplete);
        public void RegisterWxShowFunc(Action result);
        public void UnregisterWxShowFunc(Action result);
        public void RegisterWxHideFunc(Action result);
        public void UnregisterWxHideFunc(Action result);

        public void CleanAllFileCache();
    }
#endif
}
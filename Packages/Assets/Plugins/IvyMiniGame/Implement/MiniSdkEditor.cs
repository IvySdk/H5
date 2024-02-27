using System;
using System.Collections.Generic;
using Ivy.Utils;
using UnityEngine;


namespace Ivy.MiniGame
{
    public class MiniSdkEditor : MiniSdkInterface
    {
        public void InitSdk(System.Action<int> ret)
        {
            ret?.Invoke(SdkErrorCode.Ok);
        }

        public void ReportGameStart()
        {
        }

        #region 登录
        public string Me()
        {
            return "";
        }

        public void IsLogin(System.Action<bool> preLogin)
        {
        }

        public void Login(Action<string> loginSuccess, Action<string> loginFail)
        {
        }

        #endregion

        #region 本地存储
        public bool HasLocalKey(string key)
        {
            return UnityEngine.PlayerPrefs.HasKey(key);
        }

        public void DeleteLocalKey(string key)
        {
            UnityEngine.PlayerPrefs.DeleteKey(key);
        }

        public void DeleteAll()
        {
            UnityEngine.PlayerPrefs.DeleteAll();
        }

        public void SetLocalString(string key, string value = "")
        {
            UnityEngine.PlayerPrefs.SetString(key, value);
        }

        public string GetLocalStringSync(string key, string defaultValue)
        {
            return UnityEngine.PlayerPrefs.GetString(key, defaultValue);
        }

        public void SetLocalInt(string key, int value)
        {
            UnityEngine.PlayerPrefs.SetInt(key, value);
        }

        public int GetLocalIntSync(string key, int defaultValue)
        {
            return UnityEngine.PlayerPrefs.GetInt(key, defaultValue);
        }

        public void SetLocalFloat(string key, float value)
        {
            UnityEngine.PlayerPrefs.SetFloat(key, value);
        }

        public float GetLocalFloatSync(string key, float defaultValue)
        {
            return UnityEngine.PlayerPrefs.GetFloat(key, defaultValue);
        }
        #endregion

        #region 震动

        public void VibratePhone(bool isShort)
        {

        }

        #endregion

        public void SetClipboardData(string text, Action<bool> result)
        {

        }

        public bool IsIOSSystem()
        {
            return false;
        }

        public Dictionary<string, object> GetSystemInfo()
        {
            return new Dictionary<string, object>();
        }

        public Vector3 GetHorizonSafeAreaOffsetAndMenuOffset()
        {
            return Vector3.zero;
        }

#if IVYSDK_DY
        public void StartRecordScreen(bool isRecordAudio, int maxRecordTime, Action onStartCallback, Action<int, string> onErrorCallback, Action<string> onTimeOutCallback)
        {
        }

        public void StopRecordScreen(Action onCompleteCallback, Action<int, string> onErrorCallback)
        {
        }

        public int GetRecordDuration()
        {
            return 0;
        }
        
        public void ShareVideo(Action<Dictionary<string, object>> onSuccessCallback, Action<string> onFailedCallback, Action onCancelledCallback, StarkSDKSpace.UNBridgeLib.LitJson.JsonData shareJson)
        {
        }

        public void CheckScene(Action<bool> success, Action complete, Action<int, string> error)
        {
        }

        public void NavigateToScene(Action success, Action complete, Action<int, string> error)
        {
        }

        public bool IsFromSidebarCard()
        {
            return false;
        }

        public void OpenCustomerServicePage(Action<bool> onComplete)
        {
            
        }

        public void OpenAwemeCustomerService(StarkSDKSpace.UNBridgeLib.LitJson.JsonData options, Action onSuccess, Action<int, string> onFail)
        {
            
        }

        public void ReplaceSensitiveWords(string word, Action<int, string, StarkSDKSpace.UNBridgeLib.LitJson.JsonData> callback)
        {
            
        }
        
        public void ShowKeyboard(Action<string> onKeyboardConfirmEvent)
        {
            
        }

        public void HideKeyboard(Action onKeyboardCompleteEvent)
        {
            
        }

        public void OnKeyboardInput(Action<string> onKeyboardInputEvent)
        {
            
        }
        
#endif

#if IVYSDK_WX
        public void ShowKeyboard(string defaultValue, int maxLength, Action<string> onInput,
            Action<string> onConfirm, Action onComplete)
        {

        }

        public void HideKeyboard()
        {

        }

        public void OpenCustomerServicePage(Action<bool> onComplete)
        {

        }

        public void RegisterWxShowFunc(Action result)
        {
        }

        public void UnregisterWxShowFunc(Action result)
        {
        }

        public void RegisterWxHideFunc(Action result)
        {
        }

        public void UnregisterWxHideFunc(Action result)
        {
        }

        public void CleanAllFileCache()
        {

        }
#endif

    }
}



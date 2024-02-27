using System;
using System.Collections.Generic;
using Ivy.Cloud;
using Newtonsoft.Json;
using UnityEngine;

namespace Ivy.MiniGame
{
#if IVYSDK_DY
    public class ByteGame : MiniSdkInterface
    {
        private bool isFromSidebarCard = false;

        public void InitSdk(Action<int> ret)
        {
            Debug.Log("初始化 ByteGame SDK");
            StarkSDKSpace.StarkSDK.API.GetStarkAppLifeCycle().OnShowWithDict = OnShowOneParam;
            var si = StarkSDKSpace.StarkSDK.API.GetSystemInfo();
            MiniGameConfig.platform = si.platform;
            ret?.Invoke(Utils.SdkErrorCode.Ok);
        }

        private void OnShowOneParam(Dictionary<string, object> param)
        {
            if (param == null || param.Count < 1)
            {
                Debug.Log("ByteGame OnShowWithDict 回调-->无数据");
                return;
            }

            Debug.Log($"ByteGame OnShowWithDict 回调-->${JsonConvert.SerializeObject(param)}");
            var launchFrom = param.ContainsKey("launch_from") && "homepage".Equals(param["launch_from"]);
            var location = param.ContainsKey("location") && "sidebar_card".Equals(param["location"]);

            if (launchFrom && location)
            {
                isFromSidebarCard = true;
            }

            Debug.Log($"ByteGame OnShowWithDict 回调-->isFromSidebarCard:{isFromSidebarCard}");

            com.Ivy.IvySdkListener.Instance.OnDYShowWithDict(param);
        }

        public void ReportGameStart()
        {
        }

        #region 登录

        public string Me()
        {
            //  {"logInfo":"{\"openId\":\"\",\"nickName\":\"\",\"avatarUrl\":\"\",\"gender\":0,\"city\":\"\",\"province\":\"\",\"country\":\"\"}",
            Dictionary<string, object> logInfo = new();
            Dictionary<string, string> logInfoContent = new()
            {
                ["openId"] = MiniGameConfig.openId,
                ["unionId"] = MiniGameConfig.unionId
            };

            logInfo["logInfo"] = JsonConvert.SerializeObject(logInfoContent);
            return JsonConvert.SerializeObject(logInfo);
        }

        public void IsLogin(Action<bool> preLogin)
        {
            preLogin?.Invoke(false);
            // StarkSDKSpace.StarkSDK.API.GetAccountManager().CheckSession(() =>
            // {
            //     if (HasLocalKey(DY_OPEN_ID_KEY))
            //     {
            //         var openId = GetLocalStringSync(DY_OPEN_ID_KEY, "");
            //         if (!string.IsNullOrEmpty(openId))
            //         {
            //             MiniGameConfig.openid = openId;
            //             preLogin?.Invoke(true);
            //         }
            //         else
            //         {
            //             preLogin?.Invoke(false);
            //         }
            //     }
            //     else
            //     {
            //         preLogin?.Invoke(false);
            //     }
            // }, msg =>
            // {
            //     preLogin?.Invoke(false);
            // });
        }

        public void Login(Action<string> loginSuccess, Action<string> loginFail)
        {
            StarkSDKSpace.StarkSDK.API.GetAccountManager().Login((c1, c2, isLogin) =>
            {
                Debug.Log($"StarkSDK Login success:[{c1}] [{c2}] {isLogin}");
                if (!isLogin)
                {
                    loginFail?.Invoke("isLogin is false");
                    return;
                }

                Cloud.CloudStorage.CallFunctionToSelf("user_login", new Dictionary<string, object>
                {
                    ["appid"] = MiniGameConfig.appid,
                    ["code"] = c1
                }, (functionName, content) =>
                {
                    var user = JsonUtility.FromJson<LoginCallback>(content);
                    MiniGameConfig.openId = user.data.uuid;
                    MiniGameConfig.LoginCode = user.data.login_code;
                    MiniGameConfig.unionId = user.data.unionid;
                    loginSuccess?.Invoke(content);
                }, errorMsg => { loginFail?.Invoke(errorMsg); });
            }, msg => { loginFail?.Invoke(msg); }, true);
        }

        #endregion

        #region 本地存储

        public bool HasLocalKey(string key)
        {
            return StarkSDKSpace.StarkSDK.API.PlayerPrefs.HasKey(key);
        }

        public void DeleteLocalKey(string key)
        {
            StarkSDKSpace.StarkSDK.API.PlayerPrefs.DeleteKey((key));
        }

        public void DeleteAll()
        {
            StarkSDKSpace.StarkSDK.API.PlayerPrefs.DeleteAll();
            //TODO 缓存的账号信息不删除
        }

        public void SetLocalString(string key, string value = "")
        {
            StarkSDKSpace.StarkSDK.API.PlayerPrefs.SetString(key, value);
        }

        public string GetLocalStringSync(string key, string defaultValue)
        {
            return StarkSDKSpace.StarkSDK.API.PlayerPrefs.GetString(key, defaultValue);
        }

        public void SetLocalInt(string key, int value)
        {
            StarkSDKSpace.StarkSDK.API.PlayerPrefs.SetInt(key, value);
        }

        public int GetLocalIntSync(string key, int defaultValue)
        {
            return StarkSDKSpace.StarkSDK.API.PlayerPrefs.GetInt(key, defaultValue);
        }

        public void SetLocalFloat(string key, float value)
        {
            StarkSDKSpace.StarkSDK.API.PlayerPrefs.SetFloat(key, value);
        }

        public float GetLocalFloatSync(string key, float defaultValue)
        {
            return StarkSDKSpace.StarkSDK.API.PlayerPrefs.GetFloat(key, defaultValue);
        }

        #endregion

        #region 震动

        public void VibratePhone(bool isShort)
        {
            StarkSDKSpace.StarkSDK.API.Vibrate(new long[] { isShort ? 500 : 1400 });
        }

        #endregion

        #region 录屏

        public void StartRecordScreen(bool isRecordAudio, int maxRecordTime, Action onStartCallback, Action<int, string> onErrorCallback,
            Action<string> onTimeOutCallback)
        {
            if (StarkSDKSpace.StarkSDK.API.GetStarkGameRecorder().GetVideoRecordState() != StarkSDKSpace.StarkGameRecorder.VideoRecordState.RECORD_STARTED)
            {
                StarkSDKSpace.StarkSDK.API.GetStarkGameRecorder().StartRecord(isRecordAudio, maxRecordTime,
                    () => { onStartCallback?.Invoke(); },
                    (code, msg) => { onErrorCallback?.Invoke(code, msg); },
                    path => { onTimeOutCallback?.Invoke(path); });
            }
        }

        public void StopRecordScreen(Action onCompleteCallback, Action<int, string> onErrorCallback)
        {
            StarkSDKSpace.StarkSDK.API.GetStarkGameRecorder().StopRecord(path => { onCompleteCallback?.Invoke(); },
                (code, msg) => { onErrorCallback?.Invoke(code, msg); });
        }

        public int GetRecordDuration()
        {
            var recordDuration = StarkSDKSpace.StarkSDK.API.GetStarkGameRecorder().GetRecordDuration();
            Debug.Log($"获取录制时间 recordDuration :{recordDuration}");
            return recordDuration;
        }

        #endregion

        #region 分享

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onSuccessCallback"></param>
        /// <param name="onFailedCallback"></param>
        /// <param name="onCancelledCallback"></param>
        /// <param name="shareJson"></param>
        ///   shareJson["title"] = "SC小游戏，高品质游戏！";
        ///   shareJson["desc"] = "快来一起玩吧！";
        ///   shareJson["imageUrl"] = "";
        ///   shareJson["extra"] = extraJson;
        ///     extraJson["videoTopics"] = videoTopics;
        ///     extraJson["hashtag_list"] = videoTopics;
        ///     extraJson["videoPath"] = targetPath;
        ///     extraJson["video_title"] = "StarkSDK Demo";
        ///     extraJson["defaultBgm"] = "https://v.douyin.com/RCkLY1N/";
        ///         JsonData videoTopics = new JsonData();
        ///         videoTopics.SetJsonType(JsonType.Array);
        ///         videoTopics.Add("SC小游戏");
        ///         videoTopics.Add("字节游戏");
        /// 
        public void ShareVideo(Action<Dictionary<string, object>> onSuccessCallback, Action<string> onFailedCallback, Action onCancelledCallback,
            StarkSDKSpace.UNBridgeLib.LitJson.JsonData shareJson = null)
        {
            if (shareJson == null)
            {
                StarkSDKSpace.StarkSDK.API.GetStarkGameRecorder().ShareVideo(result => { onSuccessCallback?.Invoke(result); },
                    errMsg => { onFailedCallback?.Invoke(errMsg); },
                    () => { onCancelledCallback?.Invoke(); });
            }
            else
            {
                StarkSDKSpace.StarkSDK.API.GetStarkGameRecorder().ShareVideoWithJson(result => { onSuccessCallback?.Invoke(result); },
                    errMsg => { onFailedCallback?.Invoke(errMsg); },
                    () => { onCancelledCallback?.Invoke(); }, shareJson);
            }
        }

        #endregion

        #region 侧边栏相关

        public void CheckScene(Action<bool> success, Action complete, Action<int, string> error)
        {
            StarkSDKSpace.StarkSDK.API.GetStarkSideBarManager().CheckScene(StarkSDKSpace.StarkSideBar.SceneEnum.SideBar, success, complete, error);
        }

        public void NavigateToScene(Action success, Action complete, Action<int, string> error)
        {
            StarkSDKSpace.StarkSDK.API.GetStarkSideBarManager().NavigateToScene(StarkSDKSpace.StarkSideBar.SceneEnum.SideBar, success, complete, error);
        }

        public bool IsFromSidebarCard()
        {
            return isFromSidebarCard;
        }

        #endregion

        // 客服
        public void OpenCustomerServicePage(Action<bool> onComplete)
        {
            StarkSDKSpace.StarkSDK.API.OpenCustomerServicePage(onComplete);
        }

        // 通过客服页面发起支付
        public void OpenAwemeCustomerService(StarkSDKSpace.UNBridgeLib.LitJson.JsonData options, Action onSuccess, Action<int, string> onFail)
        {
            StarkSDKSpace.StarkSDK.API.OpenAwemeCustomerService(options, onSuccess, onFail);
        }

        // 屏蔽字
        public void ReplaceSensitiveWords(string word, Action<int, string, StarkSDKSpace.UNBridgeLib.LitJson.JsonData> callback)
        {
            StarkSDKSpace.StarkSDK.API.ReplaceSensitiveWords(word, callback);
        }

        // 键盘
        public void ShowKeyboard(Action<string> onKeyboardConfirmEvent)
        {
            StarkSDKSpace.StarkSDK.API.GetStarkKeyboard().onKeyboardConfirmEvent = str => { onKeyboardConfirmEvent?.Invoke(str); };
            StarkSDKSpace.StarkSDK.API.GetStarkKeyboard().ShowKeyboard();
        }

        public void HideKeyboard(Action onKeyboardHideEvent = null)
        {
            StarkSDKSpace.StarkSDK.API.GetStarkKeyboard().onKeyboardCompleteEvent = str => { onKeyboardHideEvent?.Invoke(); };
            StarkSDKSpace.StarkSDK.API.GetStarkKeyboard().HideKeyboard();
        }

        public void OnKeyboardInput(Action<string> onKeyboardInputEvent)
        {
            StarkSDKSpace.StarkSDK.API.GetStarkKeyboard().onKeyboardInputEvent = str => { onKeyboardInputEvent?.Invoke(str); };
        }

        public void SetClipboardData(string text, Action<bool> result)
        {
            StarkSDKSpace.StarkSDK.API.GetStarkClipboard().SetClipboardData(text, (isSuccess, errMsg) => { result?.Invoke(isSuccess); });
        }

        // 获取手机的型号
        public Dictionary<string, object> GetSystemInfo()
        {
            var systemInfo = new Dictionary<string, object>
            {
                ["model"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().model,
                ["system"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().system,
                ["platform"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().platform,
                ["brand"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().brand,
                ["language"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().language,
                ["deviceScore"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().deviceScore.ToString(),
                ["hostName"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().hostName,
                ["hostVersion"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().hostVersion,
                ["pixelRatio"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().pixelRatio,
                ["safeArea"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().safeArea,
                ["screenHeight"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().screenHeight,
                ["screenWidth"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().screenWidth,
                ["scVersion"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().scVersion,
                ["sdkVersion"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().sdkVersion,
                ["statusBarHeight"] = StarkSDKSpace.StarkSDK.API.GetSystemInfo().statusBarHeight
            };

            return systemInfo;
        }

        public bool IsIOSSystem()
        {
            if ("ios".Equals(MiniGameConfig.platform, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public Vector3 GetHorizonSafeAreaOffsetAndMenuOffset()
        {
            Vector3 result = Vector3.zero;
            var rect = StarkSDKSpace.StarkSDK.API.GetMenuButtonLayout();
            var systemInfo = StarkSDKSpace.StarkSDK.API.GetSystemInfo();
            var rate = systemInfo.pixelRatio;
            result.x = (int)(systemInfo.safeArea.left * rate);
            result.y = (int)((systemInfo.screenWidth - systemInfo.safeArea.right) * rate);
            int leftPos = rect.OptGetInt("left");
            int rightPos = rect.OptGetInt("right");
            double offset = (rightPos - leftPos) / 2 * 0.6;
            if (IsIOSSystem())
            {
                offset = -25;
            }
            result.z = (float)((systemInfo.screenWidth - leftPos + offset) * rate);
            return result;
        }
    }
#endif
}
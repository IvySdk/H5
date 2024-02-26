#if CN_WX
using System;
using System.Collections.Generic;
using Ivy.Cloud;
using Newtonsoft.Json;
using UnityEngine;
using WeChatWASM;
using SystemInfo = WeChatWASM.SystemInfo;

namespace Ivy.MiniGame
{
    internal class WeChatGame : MiniSdkInterface
    {
        public void InitSdk(System.Action<int> ret)
        {
            //微信sdk初始化 调用一次
            WeChatWASM.WX.InitSDK(code => { InitCloud(code, ret); });
            WeChatWASM.WX.GetSystemInfoAsync(new WeChatWASM.GetSystemInfoAsyncOption
            {
                success = ret => { MiniGameConfig.platform = ret.platform; }
            });
            WX.OnShow((listenerResult => { _onShowResult?.Invoke(); }));
            WX.OnHide(listenerResult => { _onHideResult?.Invoke(); });
        }

        private void InitCloud(int code, System.Action<int> ret)
        {
            ret?.Invoke(code);
            return; // 用不到

            //初始化云开发环境
            WeChatWASM.WX.cloud.Init(new WeChatWASM.CallFunctionInitParam
            {
                env = "tales-8gc353vk39216ab2",
                traceUser = true
            });
            ret?.Invoke(code);
        }

        public void ReportGameStart()
        {
            //小游戏启动报告
            WeChatWASM.WX.ReportGameStart();
        }

        public string Me()
        {
            Dictionary<string, object> logInfo = new();
            Dictionary<string, string> logInfoContent = new()
            {
                ["openId"] = MiniGameConfig.openId,
                ["unionId"] = MiniGameConfig.unionId
            };

            logInfo["logInfo"] = JsonConvert.SerializeObject(logInfoContent);
            return JsonConvert.SerializeObject(logInfo);
        }

        #region 登录

        public void IsLogin(Action<bool> preLogin)
        {
            preLogin?.Invoke(false);
            return;
            WeChatWASM.WX.CheckSession(new WeChatWASM.CheckSessionOption
            {
                success = resp =>
                {
                    Utils.Log.Print($"CheckSession success:{resp.errMsg}");
                    preLogin?.Invoke(true);
                },
                fail = resp =>
                {
                    Utils.Log.Print($"CheckSession fail:{resp.errMsg}");
                    preLogin?.Invoke(false);
                },
                complete = resp => { Utils.Log.Print($"CheckSession complete:{resp.errMsg}"); }
            });
        }

        public void Login(Action<string> loginSuccess, Action<string> loginFail)
        {
            WeChatWASM.WX.Login(new WeChatWASM.LoginOption
            {
                complete = resp => { Utils.Log.Print($"login complete:{resp.errMsg}"); },
                success = resp =>
                {
                    Utils.Log.Print($"login success:{resp.code}");
                    Cloud.CloudStorage.CallFunctionToSelf("user_login", new Dictionary<string, object>
                    {
                        ["appid"] = MiniGameConfig.appid,
                        ["code"] = resp.code
                    }, (functionName, content) =>
                    {
                        var user = JsonUtility.FromJson<LoginCallback>(content);
                        MiniGameConfig.openId = user.data.uuid;
                        MiniGameConfig.LoginCode = user.data.login_code;
                        MiniGameConfig.unionId = user.data.unionid;
                        loginSuccess?.Invoke(content);
                    }, errorMsg => { loginFail?.Invoke(errorMsg); });

                },fail = resp =>
                {
                    loginFail?.Invoke(resp.errMsg);
                }
            });
        }

        #endregion

        #region 本地存储

        public void DeleteAll()
        {
            WeChatWASM.WX.StorageDeleteAllSync();
        }

        public void DeleteLocalKey(string key)
        {
            WeChatWASM.WX.StorageDeleteKeySync(key);
        }

        public bool HasLocalKey(string key)
        {
            return WeChatWASM.WX.StorageHasKeySync(key);
        }

        public void SetLocalString(string key, string value = "")
        {
            WeChatWASM.WX.StorageSetStringSync(key, value);
        }

        public string GetLocalStringSync(string key, string defaultValue)
        {
            return WeChatWASM.WX.StorageGetStringSync(key, defaultValue);
        }

        public void SetLocalInt(string key, int value)
        {
            WeChatWASM.WX.StorageSetIntSync(key, value);
        }

        public int GetLocalIntSync(string key, int defaultValue)
        {
            return WeChatWASM.WX.StorageGetIntSync(key, defaultValue);
        }

        public void SetLocalFloat(string key, float value)
        {
            WeChatWASM.WX.StorageSetFloatSync(key, value);
        }

        public float GetLocalFloatSync(string key, float defaultValue)
        {
            return WeChatWASM.WX.StorageGetFloatSync(key, defaultValue);
        }

        #endregion

        #region 震动

        public void VibratePhone(bool isShort)
        {
            if (isShort)
            {
                WeChatWASM.WX.VibrateShort(new WeChatWASM.VibrateShortOption
                {
                    type = "light"
                });
            }
            else
            {
                WeChatWASM.WX.VibrateLong(new WeChatWASM.VibrateLongOption());
            }
        }

        #endregion

        #region 剪切板

        public void SetClipboardData(string text)
        {
            WeChatWASM.WX.SetClipboardData(new SetClipboardDataOption()
            {
                data = text
            });
        }

        public void SetClipboardData(string text, Action<bool> result)
        {
            WeChatWASM.WX.SetClipboardData(new SetClipboardDataOption()
            {
                data = text,
                success = resp => { result?.Invoke(true); },
                fail = resp => { result?.Invoke(false); }
            });
        }

        #endregion

        #region 键盘相关

        private Action<string> _onInput;
        private Action<string> _onConfirm;
        private Action _onComplete;

        public void ShowKeyboard(string defaultValue, int maxLength, Action<string> onInput,
            Action<string> onConfirm, Action onComplete)
        {
            WeChatWASM.WX.ShowKeyboard(new ShowKeyboardOption()
            {
                defaultValue = defaultValue,
                maxLength = maxLength,
                confirmType = "done"
            });
            _onInput = onInput;
            _onConfirm = onConfirm;
            _onComplete = onComplete;
            WX.OnKeyboardConfirm(KeyboardEvent_OnConfirm);
            WX.OnKeyboardComplete(KeyboardEvent_OnComplete);
            WX.OnKeyboardInput(KeyboardEvent_OnInput);
        }

        public void HideKeyboard()
        {
            WeChatWASM.WX.HideKeyboard(new HideKeyboardOption());
            _onInput = null;
            _onConfirm = null;
            _onComplete = null;
            WX.OffKeyboardInput(KeyboardEvent_OnInput);
            WX.OffKeyboardConfirm(KeyboardEvent_OnConfirm);
            WX.OffKeyboardComplete(KeyboardEvent_OnComplete);
        }

        private void KeyboardEvent_OnInput(OnKeyboardInputListenerResult result)
        {
            _onInput?.Invoke(result.value);
        }

        private void KeyboardEvent_OnConfirm(OnKeyboardInputListenerResult result)
        {
            _onConfirm?.Invoke(result.value);
        }

        private void KeyboardEvent_OnComplete(OnKeyboardInputListenerResult result)
        {
            _onComplete?.Invoke();
        }

        #endregion


        // 获取手机的型号
        public Dictionary<string, object> GetSystemInfo()
        {
            SystemInfo info = WeChatWASM.WX.GetSystemInfoSync();
            var systemInfo = new Dictionary<string, object>
            {
                ["model"] = info.model,
                ["system"] = info.system,
                ["platform"] = info.platform,
                ["brand"] = info.brand,
                ["language"] = info.language,
                ["pixelRatio"] = info.pixelRatio,
                ["safeArea"] = info.safeArea,
                ["screenHeight"] = info.screenHeight,
                ["screenWidth"] = info.screenWidth,
                ["sdkVersion"] = info.SDKVersion,
                ["statusBarHeight"] = info.statusBarHeight,
                ["version"] = info.version,
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

        #region 用户安全区域偏移量

        public Vector3 GetHorizonSafeAreaOffsetAndMenuOffset()
        {
            Vector3 result = Vector3.zero;
            var rect = WeChatWASM.WX.GetMenuButtonBoundingClientRect();
            var systemInfo = WeChatWASM.WX.GetSystemInfoSync();
            var rate = systemInfo.pixelRatio;
            result.x = (int)(systemInfo.safeArea.left * rate);
            result.y = (int)((systemInfo.screenWidth - systemInfo.safeArea.right) * rate);

            result.z = (int)((systemInfo.screenWidth - rect.left) * rate);
            return result;
        }

        #endregion

        #region 客服

        public void OpenCustomerServicePage(Action<bool> onComplete)
        {
            WeChatWASM.WX.OpenCustomerServiceConversation(new OpenCustomerServiceConversationOption()
            {
                complete = (res) => { Utils.Log.Print("OpenCustomerServicePage complete"); },
                fail = (res) => { onComplete?.Invoke(false); },
                success = (res) => { onComplete?.Invoke(true); }
            });
        }

        #endregion

        #region unity中失效的事件注册

        private Action _onShowResult;

        public void RegisterWxShowFunc(Action result)
        {
            if (result == null)
            {
                return;
            }
            _onShowResult += result;
        }

        public void UnregisterWxShowFunc(Action result)
        {
            if (result == null)
            {
                return;
            }
            _onShowResult -= result;
        }

        private Action _onHideResult;

        public void RegisterWxHideFunc(Action result)
        {
            if (result == null)
            {
                return;
            }
            _onHideResult += result;
        }

        public void UnregisterWxHideFunc(Action result)
        {
            if (result == null)
            {
                return;
            }
            _onHideResult -= result;
        }

        #endregion

        #region 清理文件缓存

        public void CleanAllFileCache()
        {
            WX.CleanAllFileCache((res) =>
            {
                Utils.Log.Print($"CleanAllFileCache {res}");
                if (res)
                {
                    WX.RestartMiniProgram(new RestartMiniProgramOption()
                    {
                        
                    });
                }
            });
        }

        #endregion
    }
}
#endif
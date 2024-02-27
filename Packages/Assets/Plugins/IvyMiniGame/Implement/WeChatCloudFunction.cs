using System;
using UnityEngine;

namespace Ivy.Cloud
{
#if IVYSDK_WX
    /// <summary>
    /// 登录返回的用户信息
    /// </summary>
    [System.Serializable]
    internal class UserInfo
    {
        [SerializeField] internal string openid;
        [SerializeField] internal string appid;
        [SerializeField] internal string unionid;
    }

    [Serializable]
    internal class SettingResult : MiniGame.ActionResult
    {
        [SerializeField] internal string table;
        [SerializeField] internal string key;
        [SerializeField] internal string value;
    }

    public static class CloudFunction
    {
        //从腾讯云函数上获取openid
        public static void GetOpenid(System.Action<MiniGame.LoginResult> rlt)
        {
            WeChatWASM.WX.cloud.CallFunction(new WeChatWASM.CallFunctionParam
            {
                name = "getOpenid",
                data = "{}",
                success = resp =>
                {
                    Utils.Log.Print($"CallFunction success {resp.result}");
                    var uInfo = JsonUtility.FromJson<UserInfo>(resp.result);
                    rlt?.Invoke(new MiniGame.LoginResult
                    {
                        errcode = Utils.SdkErrorCode.Ok,
                        openid = uInfo.openid,
                        unionid = uInfo.unionid
                    });
                },
                fail = resp =>
                {
                    Utils.Log.Print($"CallFunction fail {resp.result}");
                    rlt?.Invoke(new MiniGame.LoginResult
                    {
                        errcode = Utils.SdkErrorCode.ServerTimeout,
                        errmsg = resp.result
                    });
                }
            });
        }

        private const string UserSettingTableName = "UserSetting";
        //赋值 腾讯云函数用户设置
        public static void SetCloudSetting(string key, string value, Action<bool> ret)
        {
            WeChatWASM.WX.cloud.CallFunction(new WeChatWASM.CallFunctionParam
            {
                name = "setCloudData",
                data = JsonUtility.ToJson(new SettingResult
                {
                    table = UserSettingTableName,
                    key = key,
                    value = value
                }),
                success = resp =>
                {
                    ret?.Invoke(true);
                },
                fail = resp =>
                {
                    Utils.Log.Print($"setCloudSetting fail:{resp.result}");
                    ret?.Invoke(false);
                },
                complete = resp =>
                {
                    Utils.Log.Print($"setCloudSetting complete:{resp.result}");
                }
            });
        }

        //获取 腾讯云函数用户设置
        public static void GetCLoudSetting(string key, Action<bool, string> ret)
        {
            WeChatWASM.WX.cloud.CallFunction(new WeChatWASM.CallFunctionParam
            {
                name = "getCloudData",
                data = JsonUtility.ToJson(new SettingResult
                {
                    table = UserSettingTableName,
                    key = key
                }),
                success = resp =>
                {
                    var uInfo = JsonUtility.FromJson<SettingResult>(resp.result);
                    try
                    {
                        ret?.Invoke(uInfo.errcode == Utils.SdkErrorCode.Ok, uInfo.value);
                    }
                    catch (Exception)
                    {
                        ret?.Invoke(false, "");
                    }
                },
                fail = resp =>
                {
                    Utils.Log.Print($"getCloudSetting fail:{resp.result}");
                    ret?.Invoke(false, resp.result);
                },
                complete = resp =>
                {
                    Utils.Log.Print($"getCloudSetting complete:{resp.result}");
                }
            });
        }
    }
#endif
}

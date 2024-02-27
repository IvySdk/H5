using System;
using System.Collections.Generic;
using System.Linq;
using com.Ivy;
using Newtonsoft.Json;
using UnityEngine;

namespace Ivy.Cloud
{
    [Serializable]
    internal class UserCallBack
    {
        [SerializeField] internal string uuid;
        [SerializeField] internal string unionid;
        [SerializeField] internal string login_code;
    }

    [Serializable]
    internal class LoginCallback
    {
        [SerializeField] internal int code;
        [SerializeField] internal string message;
        [SerializeField] internal UserCallBack data;
    }

    [Serializable]
    public class CloudFuncData
    {
        [SerializeField] public string funcKey; // 云函数的请求 key 
        [SerializeField] public int count; // 当前数据分段的 数量
        [SerializeField] public int index; // 当前数据分端 索引，用于数据拼接排序
        [SerializeField] public string data; // 当前数据分段的 数据
    }

    internal class CloudCode
    {
        internal const int Fail = 0;
        internal const int Success = 1;
        internal const int UnLogin = 401;
        internal const int NetErr = -114514001; // 自定义的
        internal const int DataErr = -114514002;  // 自定义的

    }

    public static class CloudStorage
    {
        private static bool isFailed = true;

        //返回云存储是否初始化完
        public static bool IsRunning()
        {
            return !isFailed;
        }

        private static Dictionary<string, string> actionMap = new();

        public static void InitCloudStorage()
        {
            Utils.HttpUtil.I.HttpCloudPost(MiniGame.MiniGameConfig.listRequest, "{}", (code, content) =>
            {
                try
                {
                    var listResp = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                    var objMap = JsonConvert.DeserializeObject<Dictionary<string, object>>(listResp["data"].ToString());
                    actionMap = objMap.ToDictionary(
                        x => x.Key,
                        x => x.Value.ToString());
                    isFailed = false;
                }
                catch (Exception e)
                {
                    Utils.Log.Print(e.Message);
                }
            });
        }

        private static void _CallFunctionToSelf(string action, string content, Action<string, string> onSuccess,
            Action<string> onFail)
        {
            if (isFailed)
            {
                onFail?.Invoke($"{action}");
                return;
            }

            if (!actionMap.TryGetValue(action, out var url))
            {
                onFail?.Invoke($"{action}");
                return;
            }

            Utils.HttpUtil.I.HttpCloudPost(url, content, (code, respContent) =>
            {
                Utils.Log.Print($" ----[_CallFunctionToSelf] {action} {respContent}");
                if (code == CloudCode.Success)
                {
                    onSuccess?.Invoke(action, respContent);
                }
                else
                {
                    onFail?.Invoke($"{action}");
                }
            });
        }

        private static void _CallFunctionToGame(string action, string content, Action<string, string> onSuccess,
            Action<string> onFail)
        {
            if (isFailed)
            {
                onFail?.Invoke($"{action}");
                return;
            }

            if (!actionMap.TryGetValue(action, out var url))
            {
                onFail?.Invoke($"{action}");
                return;
            }

            Utils.HttpUtil.I.HttpCloudPost(url, content, (code, respContent) =>
            {
                Utils.Log.Print($" ----[_CallFunctionToGame] {action} {respContent}");
                if (code == CloudCode.Fail || code == CloudCode.Success || code == CloudCode.UnLogin)
                {
                    onSuccess?.Invoke(action, respContent);
                }
                else
                {
                    onFail?.Invoke($"{action}");
                }
            });
        }

        /// <summary>
        /// 面向游戏内业务的云函数调用
        /// </summary>
        /// <param name="action">方法名</param>
        /// <param name="content">所需参数内容</param>
        public static void CallFunction(string action, string content)
        {
            var obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
            CallFunctionToGame(action, obj, (functionName, data) =>
            {
                IvySdkListener.Instance.OnTencentCloudFuncSuccess(JsonUtility.ToJson(new CloudFuncData
                {
                    funcKey = functionName,
                    count = 1,
                    index = 1,
                    data = data
                }));
            }, functionName => { IvySdkListener.Instance.OnTencentCloudFuncFail(functionName); });
        }

        /// <summary> 给自己(sdk)用的云函数接口;针对云函数失败信息直接处理</summary>
        internal static void CallFunctionToSelf(string action, Dictionary<string, object> contentMap,
            Action<string, string> onSuccess, Action<string> onFail)
        {
            CallFunction(action, contentMap, onSuccess, onFail, false);
        }

        /// <summary> 给客户端(游戏内逻辑)用的云函数接口;即使云函数返回失败也将之完整传递给客户端，由客户端处理；仅超时会传递给客户端调用失败 </summary>
        private static void CallFunctionToGame(string action, Dictionary<string, object> contentMap,
            Action<string, string> onSuccess, Action<string> onFail)
        {
            CallFunction(action, contentMap, onSuccess, onFail, true);
        }

        private static void CallFunction(string action, Dictionary<string, object> contentMap,
            Action<string, string> onSuccess, Action<string> onFail, bool toGame)
        {
            if (contentMap == null)
            {
                onFail?.Invoke($"{action}");
                return;
            }

            contentMap["version"] = MiniGame.MiniGameConfig.versionCode;
            contentMap["os"] = MiniGame.MiniGameConfig.os;
            contentMap["channel"] = MiniGame.MiniGameConfig.channel;
            if (!string.IsNullOrEmpty(MiniGame.MiniGameConfig.unionId))
            {
                contentMap["unionid"] = MiniGame.MiniGameConfig.unionId;
            }

            switch (action)
            {
                case "api_timestamp":
                case "api_remote_config":
                    {
                        contentMap["appid"] = MiniGame.MiniGameConfig.appid;
                        contentMap["uuid"] = MiniGame.MiniGameConfig.openId;
                        break;
                    }
                case "api_latest_version":
                    {
                        contentMap["appid"] = MiniGame.MiniGameConfig.appid;
                        break;
                    }
                //支付请求暂时不处理
                case "pay_payAliOrderLocal":
                case "pay_payWxOrderLocal":
                    {
                        //调用失败
                        onFail?.Invoke(action);
                        break;
                    }
                case "pay_queryOrderLocal":
                case "pay_queryOrderDouYin":
                case "pay_confirmOrder":
                case "pay_unConfirmOrder":
                    {
                        // //调用失败
                        // onFail?.Invoke(action);
                        contentMap["appid"] = MiniGame.MiniGameConfig.appid;
                        contentMap["uuid"] = MiniGame.MiniGameConfig.openId;
                        contentMap["login_code"] = MiniGame.MiniGameConfig.LoginCode;
                        break;
                    }
                // case "user_save":
                // case "user_get":
                // case "user_get_wxinfo":
                // case "user_check_wxmsg":
                // case "rank_init":
                // case "rank_update":
                // case "rank_update_profile":
                // case "rank_get_ranks":
                // case "rank_claim_rewards":
                // case "rank_get_rank_user_info":
                // case "rank_get_user_rank":
                // case "social_view_data":
                // case "social_init":
                // case "social_my_friends":
                // case "social_friends_message":
                // case "social_accept_friend":
                // case "social_decline_friend":
                // case "social_send_gift":
                // case "social_decline_gift":
                // case "social_accept_gift":
                // case "social_recommend_friends":
                // case "social_invite_friend":
                // case "social_search_friend_by_tag":
                // case "social_delete_friend":
                // case "social_confirm_invite":
                // case "social_get_user_info":
                // case "social_handle_all_message":
                // case "social_batch_invite":
                // case "social_check_gift_process":
                // case "social_like_click":
                // case "social_handle_gift_message":
                // case "social_task_refresh":
                // case "social_task_assist":
                // case "manage_cdk":
                // case "manage_get_email":
                // case "manage_del_email":
                // case "manage_feedback_email":
                default:
                    {
                        contentMap["appid"] = MiniGame.MiniGameConfig.appid;
                        contentMap["uuid"] = MiniGame.MiniGameConfig.openId;
                        contentMap["login_code"] = MiniGame.MiniGameConfig.LoginCode;
                        break;
                    }
            }

            if (toGame)
            {
                _CallFunctionToGame(action, JsonConvert.SerializeObject(contentMap), onSuccess, onFail);
            }
            else
            {
                _CallFunctionToSelf(action, JsonConvert.SerializeObject(contentMap), onSuccess, onFail);
            }


        }
    }
}
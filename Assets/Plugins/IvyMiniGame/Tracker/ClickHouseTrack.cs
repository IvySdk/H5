// using System.Collections.Generic;
// using Ivy.MiniGame;
// using UnityEngine;

// namespace Ivy.Event.Tracker
// {
//     public class ClickHouseTrack : TrackInterface
//     {
//         private bool isUnInit = true;
//         private string url;
//         private string uuid;
//         private string gps_adid;
//         public void Init()
//         {
//             url = MiniGame.MiniGameConfig.trackConfig.url;

//             uuid = GenerateUUid();

//             isUnInit = false;
//         }

//         public void report(string eventKey, Dictionary<string, object> paramMap)
//         {
//             TransferToHttpPost(eventKey, paramMap);
//         }

//         private void TransferToHttpPost(string eventKey, Dictionary<string, object> paramMap)
//         {
//             //todo 添加默认信息
//             paramMap["roleId"] = MiniGameConfig.openId;

//             var info = MiniSdk.GetSystemInfo();
//             if (info.TryGetValue("brand", out var brand))
//             {
//                 paramMap["device_manufacturer"] = brand;
//             }
//             if (info.TryGetValue("model", out var model))
//             {
//                 paramMap["device_name"] = model;
//             }
//             if (info.TryGetValue("system", out var system))
//             {
//                 paramMap["os_name"] = system;
//             }
//             paramMap["event_token"] = eventKey;
//             paramMap["app_token"] = "ap" + MiniGameConfig.appid;
//             paramMap["app_version"] = MiniGameConfig.versionName;
//             paramMap["package_name"] = MiniGameConfig.packageName;
//             //paramMap["created_at"] = MiniGameConfig.packageName;
//             Utils.HttpUtil.I.HttpEventPost(GetEventUrl, eventKey, Newtonsoft.Json.JsonConvert.SerializeObject(paramMap), GetInitStatus);
//         }


//         private string GetEventUrl(string eventKey)
//         {
//             var eventUrl = $"{url}?package_name={MiniGame.MiniGameConfig.packageName}&event_token={eventKey}";
//             if (!string.IsNullOrEmpty(uuid))
//             {
//                 eventUrl += $"&android_uuid={uuid}";
//             }
//             if (!string.IsNullOrEmpty(gps_adid))
//             {
//                 eventUrl += $"&gps_adid={gps_adid}";
//             }
//             return eventUrl;
//         }

//         private bool GetInitStatus()
//         {
//             return !isUnInit;
//         }

//         private string GenerateUUid()
//         {
//             //todo 修改uuid的生成办法
//             return Application.identifier;
//         }
//     }
// }


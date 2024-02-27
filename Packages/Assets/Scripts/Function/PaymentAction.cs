// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using Random = UnityEngine.Random;

// namespace Ivy.WeiChat
// {
//     [System.Serializable]
//     internal struct ServiceMessage
//     {
//         public int mType;
//         public string msg;
//         public string client_ip;
//         public string platform;
//     }

//     public class PaymentAction : MonoBehaviour
//     {
//         private void Awake()
//         {
//             //不同的小游戏要查询对应服务器的路由，来分散访问压力
//             Utils.HttpUtil.I.HttpPostWithoutTimeout("https://wxapi.winwingplane.cn:8081/queryip", "", (ret, ip) =>
//             {
//                 if (ret == Utils.SdkErrorCode.Ok)
//                 {
//                     myClientIp = ip;
//                 }
//             });

//         }

//         private void Start()
//         {
//             _btn.onClick.RemoveAllListeners();
//             _btn.onClick.AddListener(LinkToCustomService);
//         }


//         private string myClientIp = "";
//         [SerializeField] private Button _btn;
//         private void LinkToCustomService()
//         {
// #if IVYSDK_WX
//             WeChatWASM.WX.OpenCustomerServiceConversation(new WeChatWASM.OpenCustomerServiceConversationOption
//             {
//                 //显示右下角的截图tip
//                 showMessageCard = true,
//                 sendMessageImg = "https://mmgame.qpic.cn/image/5f9144af9f0e32d50fb878e5256d669fa1ae6fdec77550849bfee137be995d18/0",
//                 sendMessageTitle = "付费$9",
//                 complete = ret =>
//                 {
//                     Utils.Log.Print($"complete:{ret.errMsg}");
//                 },
//                 success = ret =>
//                 {
//                     Utils.Log.Print($"success path:{ret.path}");
//                     Utils.Log.Print($"success message:{ret.errMsg}");
//                     foreach (var kvp in ret.query)
//                     {
//                         Utils.Log.Print($"success query:{kvp.Key} {kvp.Value}");
//                     }
//                 },
//                 sessionFrom = JsonUtility.ToJson(new ServiceMessage
//                 {
//                     mType = Random.Range(0,10),
//                     msg = "test session message",
//                     //需要保证下面两个数值的实时有效
//                     client_ip = myClientIp,
//                     platform = MiniGame.MiniGameConfig.platform
//                 })
//             });
// #else
//             Utils.Log.Print("not IVYSDK_WX");
// #endif


//         }
//     }
// }



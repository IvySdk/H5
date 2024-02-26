using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ivy.WeiChat
{
    public class LoginWeChat : MonoBehaviour
    {
        [SerializeField] private Button btn;
        [SerializeField] private Button btnSet;
        [SerializeField] private Button btnGet;
        void Start()
        {
            // Cloud.CloudStorage.onCloudFunctionResult -= FunctionResult;
            // Cloud.CloudStorage.onCloudFunctionFailed -= FunctionFailed;
            // Cloud.CloudStorage.onCloudFunctionResult += FunctionResult;
            // Cloud.CloudStorage.onCloudFunctionFailed += FunctionFailed;
            MiniGame.MiniSdk.IsLogin();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(MiniGame.MiniSdk.Login);
            btnSet.onClick.RemoveAllListeners();
            btnSet.onClick.AddListener(() =>
            {
#if CN_WX
                Cloud.CloudFunction.SetCloudSetting("focus", UnityEngine.Random.Range(0, 10).ToString(), ret =>
                {
                    Debug.Log($"SetCloudSetting:{ret}");
                });
#endif
            });
            btnGet.onClick.RemoveAllListeners();
            btnGet.onClick.AddListener(() =>
            {
#if CN_WX
                Cloud.CloudFunction.GetCLoudSetting("focus", (ret, content) =>
                {
                    Debug.Log($"GetCLoudSetting:[{ret}] [{content}]");
                });
#endif
            });
        }

        private void OnDestroy()
        {
            // Cloud.CloudStorage.onCloudFunctionResult -= FunctionResult;
            // Cloud.CloudStorage.onCloudFunctionFailed -= FunctionFailed;
        }

        private void FunctionResult(string content)
        {
            Utils.Log.Print($"FunctionResult:{content}");
        }

        private void FunctionFailed(string content)
        {
            Utils.Log.Print($"FunctionFailed:{content}");
        }
    }
}



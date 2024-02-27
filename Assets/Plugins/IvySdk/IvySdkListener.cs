using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;


namespace com.Ivy
{

    public class IvySdkListener : MonoBehaviour
    {

        public static event Action<IvySdk.AdEventType, int, string, int> OnAdEvent;

        public static event Action<string, string> OnTencentCloudFunctionSuccess;

        public static event Action<string> OnTencentCloudFunctionFail;

        public static event Action<IvySdk.PaymentResult, int, string> OnPaymentEvent;

        public static event Action<IvySdk.PaymentResult, int, string, string> OnPaymentWithPayloadEvent;

        public static event Action EventOnStartRecordScreenSuccess;

        public static event Action<int, string> EventOnStartRecordScreenError;

        public static event Action<string> EventOnStartRecordScreenTimeOut;

        public static event Action EventOnStopRecordScreenComplete;

        public static event Action<int, string> EventOnStopRecordScreenError;

        public static event Action<Dictionary<string, object>> EventOnShareVideoSuccess;

        public static event Action<string> EventOnShareVideoFailed;

        public static event Action EventOnShareVideoCancel;

        public static event Action<bool> EventOnCheckSceneSuccess;

        public static event Action EventOnCheckSceneComplete;

        public static event Action<int, string> EventOnCheckSceneError;

        public static event Action EventOnNavigateToSceneSuccess;

        public static event Action EventOnNavigateToSceneComplete;

        public static event Action<int, string> EventOnNavigateToSceneError;

        public static event Action<int, int, string> EventOnReplaceSensitiveWords;

        public static event Action<string> EventOnKeyboardInput;

        public static event Action<string> EventOnKeyboardConfirm;

        public static event Action EventOnKeyboardComplete;

        public static event Action<bool> EventOnClipboardComplete;

        private static IvySdkListener _instance;


        private Dictionary<string, List<IvySdk.TencentCloudFuncData>> tencentCloudFuncTempDict = new Dictionary<string, List<IvySdk.TencentCloudFuncData>>();


        public static IvySdkListener Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(IvySdkListener)) as IvySdkListener;
                    if (!_instance)
                    {
                        var obj = new GameObject("IvySdkListener");
                        _instance = obj.AddComponent<IvySdkListener>();
                        DontDestroyOnLoad(obj);
                    }
                }
                return _instance;
            }
        }

        private IvySdkListener()
        {
        }

        public void onPaymentSuccess(string data)
        {
            if (string.IsNullOrEmpty(data))
                return;
            IvySdk.PaymentData resData;
            try
            {
                resData = JsonConvert.DeserializeObject<IvySdk.PaymentData>(data);
            }
            catch
            {
                return;
            }
            if (string.IsNullOrEmpty(resData.payload))
            {
                OnPaymentEvent?.Invoke(IvySdk.PaymentResult.Success, resData.billId, resData.orderId);
            }
            else
            {
                OnPaymentWithPayloadEvent?.Invoke(IvySdk.PaymentResult.Success, resData.billId, resData.payload,
                    resData.orderId);
            }
        }

        /// <summary>
        /// payload支付成功结果回调，SDK自动调用。
        /// </summary>
        /// <param name="data"></param>
        public void onPaymentSuccessWithPayload(string data)
        {
            if (string.IsNullOrEmpty(data))
                return;
            IvySdk.PaymentData resData;
            try
            {
                resData = JsonConvert.DeserializeObject<IvySdk.PaymentData>(data);
            }
            catch
            {
                return;
            }
            if (string.IsNullOrEmpty(resData.payload))
            {
                OnPaymentEvent?.Invoke(IvySdk.PaymentResult.Success, resData.billId, resData.orderId);
            }
            else
            {
                OnPaymentWithPayloadEvent?.Invoke(IvySdk.PaymentResult.Success, resData.billId, resData.payload,
                    resData.orderId);
            }
        }

        public void onPaymentFail(string billingId)
        {
            OnPaymentEvent?.Invoke(IvySdk.PaymentResult.Failed, int.Parse(billingId), "");
            OnPaymentWithPayloadEvent?.Invoke(IvySdk.PaymentResult.Failed, int.Parse(billingId), "", "");
        }

        public void onReceiveReward(string data)
        {
            if (OnAdEvent != null && OnAdEvent.GetInvocationList().Length > 0)
            {
                bool success = false;
                int id = -1;
                string tag = "Default";
                bool skippedVideo = false;
                if (!string.IsNullOrEmpty(data))
                {
                    string[] results = data.Split('|');
                    if (results != null && results.Length > 1)
                    {
                        success = int.Parse(results[0]) == 0;
                        id = int.Parse(results[1]);
                        if (results.Length > 2)
                        {
                            tag = results[2];
                            if (results.Length > 3)
                            {
                                skippedVideo = int.Parse(results[3]) == 0 ? true : false;
                            }
                        }
                    }
                }

                Debug.Log($"**** onReceiveReward {data}");
                Debug.Log($"**** onReceiveReward {success}");
                if (success)
                {
                    OnAdEvent(IvySdk.AdEventType.RewardAdShowFinished, id, tag, IvySdk.ADTYPE_VIDEO);
                }
                else
                {
                    OnAdEvent(IvySdk.AdEventType.RewardAdShowFailed, id, tag, IvySdk.ADTYPE_VIDEO);
                }
            }
        }


        public void OnTencentCloudFuncSuccess(string result)
        {
            var data = JsonConvert.DeserializeObject<IvySdk.TencentCloudFuncData>(result);
            string funcKey = data.funcKey;
            int count = data.count;
            if (tencentCloudFuncTempDict.TryGetValue(funcKey, out var list))
            {
                list.Add(data);
            }
            else
            {
                list = new List<IvySdk.TencentCloudFuncData>();
                list.Add(data);
                tencentCloudFuncTempDict.Add(funcKey, list);
            }

            if (count <= list.Count)
            {
                list.Sort((a, b) => a.index.CompareTo(b.index));
                StringBuilder sb = new StringBuilder();
                foreach (var val in list)
                {
                    sb.Append(val.data);
                }

                OnTencentCloudFunctionSuccess?.Invoke(funcKey, sb.ToString());
                tencentCloudFuncTempDict.Remove(funcKey);
            }
            else
            {
            }
        }


        public void OnTencentCloudFuncFail(string result)
        {
            OnTencentCloudFunctionFail?.Invoke(result);
        }

        public static event Action<Dictionary<string, object>> EventOnDYShowWithDict;

        public void OnDYShowWithDict(Dictionary<string, object> param)
        {
            EventOnDYShowWithDict?.Invoke(param);
        }

        public void OnStartRecordScreenSuccess()
        {
            EventOnStartRecordScreenSuccess?.Invoke();
        }

        public void OnStartRecordScreenError(int code, string msg)
        {
            EventOnStartRecordScreenError?.Invoke(code, msg);
        }

        public void OnStartRecordScreenTimeOut(string path)
        {
            EventOnStartRecordScreenTimeOut?.Invoke(path);
        }

        public void OnStopRecordScreenComplete()
        {
            EventOnStopRecordScreenComplete?.Invoke();
        }

        public void OnStopRecordScreenError(int code, string msg)
        {
            EventOnStopRecordScreenError?.Invoke(code, msg);
        }

        public void OnShareVideoSuccess(Dictionary<string, object> result)
        {
            EventOnShareVideoSuccess?.Invoke(result);
        }

        public void OnShareVideoFailed(string msg)
        {
            EventOnShareVideoFailed?.Invoke(msg);
        }

        public void OnShareVideoCancel()
        {
            EventOnShareVideoCancel?.Invoke();
        }

        public void OnCheckSceneSuccess(bool result)
        {
            EventOnCheckSceneSuccess?.Invoke(result);
        }

        public void OnCheckSceneComplete()
        {
            EventOnCheckSceneComplete?.Invoke();
        }

        public void OnCheckSceneError(int code, string msg)
        {
            EventOnCheckSceneError?.Invoke(code, msg);
        }

        public void OnNavigateToSceneSuccess()
        {
            EventOnNavigateToSceneSuccess?.Invoke();
        }

        public void OnNavigateToSceneComplete()
        {
            EventOnNavigateToSceneComplete?.Invoke();
        }

        public void OnNavigateToSceneError(int code, string msg)
        {
            EventOnNavigateToSceneError?.Invoke(code, msg);
        }

        // 屏蔽字
        /// code      int: 返回状态码，0为成功，否则为失败
        /// result    int     1: 有敏感内容；2: 无敏感内容
        /// content   string  被替换的内容（无敏感内容则返回值与传入的word相同）
        public void OnReplaceSensitiveWords(int code, int result, string content)
        {
            EventOnReplaceSensitiveWords?.Invoke(code, result, content);
        }

        public void OnKeyboardInput(string value)
        {
            EventOnKeyboardInput?.Invoke(value);
        }
        public void OnKeyboardConfirm(string result)
        {
            EventOnKeyboardConfirm?.Invoke(result);
        }

        public void OnKeyboardComplete()
        {
            EventOnKeyboardComplete?.Invoke();
        }

        public void OnClipboardComplete(bool result)
        {
            EventOnClipboardComplete?.Invoke(result);
        }


    }


}
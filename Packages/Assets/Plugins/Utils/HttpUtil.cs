using System;
using System.Collections;
using System.Text;
using UnityEngine.Networking;

namespace Ivy.Utils
{
    [Serializable]
    internal struct ResponseData
    {
        [UnityEngine.SerializeField] public int code;
        [UnityEngine.SerializeField] public string errMsg;
        [UnityEngine.SerializeField] public string data;
    }

    [Serializable]
    internal struct ResponseCloudData
    {
        [UnityEngine.SerializeField] public int code;
        [UnityEngine.SerializeField] public string message;
    }

    public class HttpUtil : MonoSingleton<HttpUtil>
    {
        private static readonly string LogPrefix = $"[{nameof(HttpUtil)}]: ";
        private const int HttpRequestTimeout = 10;

        public delegate bool InstanceInit();

        public delegate string UrlGet(string key);

        public override void Init()
        {
        }

        public void HttpPost(string url, object postObj, Action<int, string> result)
        {
            StartCoroutine(_HttpPost(url, postObj, result));
        }

        public void HttpPostWithoutTimeout(string url, object postObj, Action<int, string> result)
        {
            StartCoroutine(_HttpPost(url, postObj, result, 0));
        }

        /// <summary>
        /// post数据到指定url
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="postObj">post数据</param>
        /// <param name="result">回调函数，int为服务器返回错误代码,T为想要得到的结果</param>
        /// <param name="timeOut">超时时间，timeout=0时为不设置超时</param>
        /// <returns></returns>
        private IEnumerator _HttpPost(string url, object postObj, Action<int, string> result,
            int timeOut = HttpRequestTimeout)
        {
            var toPostObjectStr = UnityEngine.JsonUtility.ToJson(postObj); //请求带上UID
            var request = new UnityWebRequest
            {
                url = url,
                method = "POST",
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(toPostObjectStr)),
                downloadHandler = new DownloadHandlerBuffer(),
            };
            //设置超时时间
            if (timeOut > 0)
            {
                request.timeout = timeOut;
            }

            request.SetRequestHeader("content-type", "application/json");
            yield return request.SendWebRequest();

            //兼容unity 2019 使用isNetworkError
            //if(request.result == UnityWebRequest.Result.ConnectionError)
            if (request.isNetworkError)
            {
                Log.Error(LogPrefix + $"request path: {url}\ntimeout: {request.error}");
                result?.Invoke(SdkErrorCode.ServerTimeout, default);
            }
            else
            {
                try
                {
                    var respStr = request.downloadHandler.text;
                    //Log.Print(LogPrefix + $"url:{url}\nrequest:{toPostObjectStr}\nresponse <== {respStr}");
                    var resp = UnityEngine.JsonUtility.FromJson<ResponseData>(respStr);
                    try
                    {
                        result?.Invoke(resp.code, resp.data);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.Message);
                        result?.Invoke(SdkErrorCode.SerializationFailed, default);
                    }
                }
                catch (Exception e) //处理返回结果发生未知错误
                {
                    Log.Error(LogPrefix + "request path: " + url + ", error:" + e.StackTrace);
                    result?.Invoke(SdkErrorCode.ServerUnknownError, default);
                }
            }

            request.Dispose();
        }

        public void HttpCloudPost(string url, string postObj, Action<int, string> result)
        {
            StartCoroutine(_httpCloudPost(url, postObj,
                (code, content) => { result?.Invoke(code, content); }, 0));
        }

        private IEnumerator _httpCloudPost(string url, string postObj, Action<int, string> result,
            int timeOut = HttpRequestTimeout)
        {
            var toPostObjectStr = postObj;
            var request = new UnityWebRequest
            {
                url = url,
                method = "POST",
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(toPostObjectStr)),
                downloadHandler = new DownloadHandlerBuffer(),
            };
            //设置超时时间
            //设置超时时间
            if (timeOut > 0)
            {
                request.timeout = timeOut;
            }

            request.SetRequestHeader("content-type", "application/json");
            yield return request.SendWebRequest();

            //兼容unity 2019 使用isNetworkError
#if UNITY_2021_1_OR_NEWER
            if(request.result == UnityWebRequest.Result.ConnectionError)
#else
            if (request.isNetworkError)
#endif
            {
                Log.Error(LogPrefix + $"request path: {url}\ntimeout: {request.error} {request.responseCode}");
                result?.Invoke(-114514001, "UnityWebRequest NetworkError");
            }
            else
            {
                try
                {
                    var respStr = request.downloadHandler.text;
                    var resp = UnityEngine.JsonUtility.FromJson<ResponseCloudData>(respStr);
                    // result?.Invoke(resp.message, respStr);
                    result?.Invoke(resp.code, respStr); // 云函数有回应就是成功，无论云函数回应的消息是什么，由客户端根据code判断
                }
                catch (Exception e) //处理返回结果发生未知错误
                {
                    Log.Error(LogPrefix + "request path: " + url + ", error:" + e.StackTrace);
                    result?.Invoke(-114514002, "json parse error");
                }
            }

            request.Dispose();
        }

        public void HttpEventPost(UrlGet urlGet, string key, string postObj, InstanceInit initFunc = null)
        {
            StartCoroutine(_httpEventPost(urlGet, key, postObj, initFunc));
        }

        private IEnumerator _httpEventPost(UrlGet urlGet, string key, string postObj, InstanceInit initFunc)
        {
            if (initFunc != null)
            {
                yield return new UnityEngine.WaitUntil(() => initFunc());
            }

            var request = new UnityWebRequest
            {
                url = urlGet(key),
                method = "POST",
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(postObj)),
                downloadHandler = new DownloadHandlerBuffer(),
            };
            request.SetRequestHeader("content-type", "application/json");
            yield return request.SendWebRequest();
            request.Dispose();
        }
    }
}
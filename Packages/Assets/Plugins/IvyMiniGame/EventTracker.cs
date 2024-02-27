using System;
using System.Collections.Generic;
using Ivy.Event.Tracker;
using UnityEngine;

namespace Ivy.Event
{
    internal interface TrackInterface
    {
        void Init();
        void report(string eventKey, Dictionary<string, object> paramMap);
    }

    public static class EventTracker
    {
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeInit()
        {
            adjustTrack = null;
            adjustInit = null;
            // var clickHouseInstance = new ClickHouseTrack();
            // adjustTrack += clickHouseInstance.report;
            // adjustInit += clickHouseInstance.Init;
            //todo 未验证接口实例是否可以加入action，并正常调用
#if IVYSDK_DY
            var byteInstance = new ByteTrack();
            adjustTrack += byteInstance.report;
            adjustInit += byteInstance.Init;
#elif IVYSDK_WX
            var wechatInstance = new WeChatTrack();
            adjustTrack += wechatInstance.report;
            adjustInit += wechatInstance.Init;
#endif
        }

        private static Action<string, Dictionary<string, object>> adjustTrack;
        private static Action adjustInit;

        //确保在配置初始化之后调用init
        internal static void InitTrack()
        {
            adjustInit?.Invoke();
        }

        public static void TrackEvent(string eventKey, Dictionary<string, object> paramMap)
        {
            adjustTrack?.Invoke(eventKey, paramMap);
        }

        public static void TrackEvent(string eventKey, string paramContent)
        {
            var ss = paramContent.Split(",");
            var dict = new Dictionary<string, object>();
            if (ss != null && ss.Length > 0)
            {
                for (int i = 0; i < ss.Length - 1; i += 2)
                {
                    var key = ss[i];
                    var value = ss[i + 1];
                    if (!string.IsNullOrEmpty(key))
                    {
                        dict[key] = value;
                    }
                }

            }
            TrackEvent(eventKey, dict);
        }

    }
}

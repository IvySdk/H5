using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ivy.Event.Tracker
{
#if IVYSDK_WX
    public class WeChatTrack : TrackInterface
    {
        public void Init()
        {
        }

        public void report(string eventKey, Dictionary<string, object> paramMap)
        {
            //todo 添加默认信息
            WeChatWASM.WX.ReportEvent(eventKey, paramMap);
        }
    }
#endif
}



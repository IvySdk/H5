using System.Collections.Generic;
using System.Linq;

namespace Ivy.Event.Tracker
{
#if CN_DY
    public class ByteTrack : TrackInterface
    {
        public void Init()
        {
        }

        public void report(string eventKey, Dictionary<string, object> paramMap)
        {
            //todo 添加默认信息
            var tMap = paramMap.ToDictionary(
                    x => x.Key, 
                    x => x.Value.ToString());
            StarkSDKSpace.StarkSDK.API.ReportAnalytics(eventKey, tMap);
        }
    }
#endif
}



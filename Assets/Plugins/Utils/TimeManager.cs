using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ivy.Utils
{
    public static class TimeManager
    {
        //621355968000000000 utc对应的时间偏差
        private const long DiffTick = 621355968000000000;
        //计数周期与秒单位之间的转换量
        private const long DivTick = 10000000;
        
        public static long GetTimeStamp(DateTime time)
        {
            return (time.ToUniversalTime().Ticks - DiffTick) / DivTick;
        }
        
        //获取时间戳，单位 秒
        public static long GetTimeStamp()
        {
            return GetTimeStamp(DateTime.UtcNow);
        }
    }
}


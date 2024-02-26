using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ivy.Utils
{
    public static class SdkErrorCode
    {
        public const int Ok = 0;
        //配置未加载
        public const int ConfigUnloaded = 1001;
        //未找到对应广告
        public const int AdNotFind = 1002;
        //广告加载失败
        public const int AdNotLoad = 1003;
        //广告正在展示
        public const int AdIsShowing = 1004;
        //请求未授权
        public const int RequestNotAuthed = 1005;
        //请求数据无法映射
        public const int RequestDataNotMatch = 1006;
        
        //服务器请求失败
        public const int ServerTimeout = 4003;
        //服务器未知错误
        public const int ServerUnknownError = 4004;
        //序列化失败
        public const int SerializationFailed = 4005;
        //操作因限制而失败
        public const int OptionLimit = 4099;
        
        //code 无效
        public const int CodeInValid = 40029;
        //频率限制，每个用户每分钟100次
        public const int LimitRequestCount = 45011;
        //高风险等级用户，小程序登录拦截
        public const int HighRiskUser = 40226;

    }
}

using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEditor;
using UnityEngine;

namespace Ivy.Test.Editor
{
    public static 
        class ToolTest 
    {
        [MenuItem("IvyTools/代码测试")]
        static void CodeDemo()
        {
            TestClickHouse();
        }

        static void TestClickHouse()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            var url = "http://event.feiliutec.com:8080/event";
            var packageName = "com.ivy.merge.cn.dy";
            var token = "first_open";
            var uuid = "08aefaae-dfdf-42e5-b11b-ef0ff36dbdde";
            var gps = "3da3a213-1d6e-4d24-8cc6-e3cec6fdaf7a";

            var reuqestUrl = $"{url}?package_name={packageName}&event_token={token}&android_uuid={uuid}&gps_adid={gps}";
            Utils.HttpUtil.I.HttpPostWithoutTimeout(reuqestUrl, "{}", (code, msg) =>
            {
                Debug.Log($"code:[{code}] msg:[{msg}]");
            });
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace Ivy.MiniGame
{
    [System.Serializable]
    public class ClickHouseConfig
    {
        [SerializeField] public string url;
    }

    [System.Serializable]
    public class PayIdConfig
    {
        [SerializeField] public string payid;
        [SerializeField] public string currency;
        [SerializeField] public string price;
        [SerializeField] public int amount;
        [SerializeField] public string desc;
        [SerializeField] public int rmb;
        [SerializeField] public string ThumbUrl;
    }

    public static class MiniGameConfig
    {
        //客户端版本号
        public static string versionName;

        public static string versionCode;

        //系统
        public static string os;

        //渠道数据
        public static string channel;

        //函数列表请求链接
        public static string listRequest;

        //应用id
        public static string appid;

        //
        public static ClickHouseConfig trackConfig;

        //包名
        public static string packageName;
#if CN_WX
        //是否使用微信米大师沙箱
        public static bool IsSandBox;

        //微信米大师侧申请的应用 id
        public static string offerId;
#endif

        #region 广告配置

        public static List<Ad.BannerConfig> bannerConfigArray = new();
        public static List<Ad.InterstitialConfig> interConfigArray = new();
        public static List<Ad.RewardedVideoConfig> videoConfigArray = new();

        #endregion

        #region 支付点配置

        //配置
        public static List<PayIdConfig> payConfigArray = new();

        //支付回调url地址
        public static string payNotifyUrl;

        #endregion

        #region 运行时字段

        public static string openId;

        public static string unionId;

        //登录返回的登录码
        public static string LoginCode;

        //手机型号
        public static string platform = "android";

        //客户端登录ip
        public static string clientIp;

        #endregion


        private static bool initFinished = false;

        //todo 修改配置的加载形式
        public static void LoadMiniGameConfig()
        {
            versionName = "1.0.0"; //VersionSetting.GetInstance().mainVersionName;
            versionCode = "1"; //VersionSetting.GetInstance().mainVersionCode;
            trackConfig = new ClickHouseConfig
            {
                url = "http://event.feiliutec.com:8080/event"
            };
#if CN_WX
            appid = "27000";
            os = "wechat";
            channel = "wechat";
            IsSandBox = false;
            offerId = "wxcda99b7d9bba4a79";

            //测试服地址
            // listRequest = "https://service-00fjnzs0-1259425184.gz.tencentapigw.cn/api/list";

            //正式服地址
            listRequest = "https://service-0knqzyio-1259425184.gz.apigw.tencentcs.com/api/list";


            initFinished = true;
#elif CN_DY
            appid = "27003";
            os = "douyin";
            channel = "douyin";
            //正式服地址
            //listRequest = "https://service-g8hgri8a-1259425184.gz.tencentapigw.cn/api/list"
            //测试服地址
            listRequest = "https://service-00fjnzs0-1259425184.gz.tencentapigw.cn/api/list";
            packageName = "com.ivy.merge.cn.dy";
            
            interConfigArray = new List<Ad.InterstitialConfig>
            {
                new Ad.InterstitialConfig
                {
                    tag = "main",
                    adUnitId = "prkfnptfk01d9ae9eg"
                }
            };
            videoConfigArray = new List<Ad.RewardedVideoConfig>
            {
                new Ad.RewardedVideoConfig
                {
                    tag = "main",
                    adUnitId = "2tecitpfelk7iablg4"
                }
            };
            //测试服地址
            payNotifyUrl = "https://service-00fjnzs0-1259425184.gz.tencentapigw.cn/pay/dyNotify";
#endif

            Utils.OperatorDispatcher.I.StartCoroutine(InitService());
        }

        public static bool IsInitFinished()
        {
            return initFinished;
        }

        private static IEnumerator InitService()
        {
            // 加载支付配置
            yield return LoadPayConfig();
            yield return new WaitUntil(() => initFinished);
            //初始化云函数模块
            Cloud.CloudStorage.InitCloudStorage();
            //初始化打点模块
            Event.EventTracker.InitTrack();
            //初始化广告模块
            AdManager.InitAdManager();
            //初始化支付模块
            PaymentManager.InitPaymentManager();
        }

        public static string GetConfig(int configId)
        {
            switch (configId)
            {
                case RiseSdk.CONFIG_KEY_APP_ID:
                    return appid;
                case RiseSdk.CONFIG_KEY_VERSION_NAME:
                    return versionName;
                case RiseSdk.CONFIG_KEY_VERSION_CODE:
                    return versionCode;
                case RiseSdk.CONFIG_KEY_PACKAGE_NAME:
                    return packageName;
                default:
                    Debug.LogError($"configId:{configId} need add MiniGameConfig.GetConfig");
                    return null;
            }
        }

        private static IEnumerator LoadPayConfig()
        {
#if CN_DY || CN_WX
            // yield return AddressableManager.GetInstance().LoadAssetAsync<TextAsset>(
            //     "Assets/Resources_moved/ABConfig/payConfig.json", asset =>
            //     {
            //         Utils.Log.Print(asset.text);
            //         JsonData jsonData = JsonMapper.ToObject(asset.text);
            //         List<PayIdConfig> payIdConfigs = new List<PayIdConfig>();
            //         foreach (string key in jsonData.Keys)
            //         {
            //             var data = jsonData[key];
            //             payIdConfigs.Add(new PayIdConfig()
            //             {
            //                 payid = data["productId"].ToString(),
            //                 currency = data["currency"].ToString(),
            //                 price = data["price"].ToString(),
            //                 rmb = int.Parse(data["rmb"].ToString()),
            //                 amount = int.Parse(data["feeValue"].ToString()),
            //                 desc = data["desc"].ToString(),
            //             });
            //         }

            //         payConfigArray = payIdConfigs;
            //         initFinished = true;
            //     });
            TextAsset asset = Resources.Load<TextAsset>("payConfig");
            JsonData jsonData = JsonMapper.ToObject(asset.text);
            List<PayIdConfig> payIdConfigs = new List<PayIdConfig>();
            foreach (string key in jsonData.Keys)
            {
                var data = jsonData[key];
                payIdConfigs.Add(new PayIdConfig()
                {
                    payid = data["productId"].ToString(),
                    currency = data["currency"].ToString(),
                    price = data["price"].ToString(),
                    rmb = int.Parse(data["rmb"].ToString()),
                    amount = int.Parse(data["feeValue"].ToString()),
                    desc = data["desc"].ToString(),
                });
            }

            payConfigArray = payIdConfigs;
            initFinished = true;
#endif
            yield break;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Ivy.MiniGame.Ad
{
    [System.Serializable]
    public class BannerConfig
    {
        [SerializeField] public string tag;
        [SerializeField] public string adUnitId;
        [SerializeField] public int adIntervals;
    }

    internal class BannerStyle
    {
        internal int left;
        internal int top;
        internal int width;
        internal int height;
    }

    internal interface BannerInterface
    {
        ///显示banner广告
        void ShowBanner(BannerConfig conf, int left, int top, int width, System.Action<BannerAdResult> rlt);

        ///显示banner广告
        void ShowBanner(BannerConfig conf, BannerPosition bp, System.Action<BannerAdResult> rlt);

        ///隐藏banner广告
        void HideBanner();
    }

    internal class BannerManager
    {
        private readonly List<BannerConfig> _bannerConfigs;
        private readonly BannerInterface I;
        public BannerManager(List<BannerConfig> bannerConfigs)
        {
            _bannerConfigs = bannerConfigs;
#if IVYSDK_DY
            I = new ByteBannerAd();
#elif IVYSDK_WX
            I = new WeChatBanner();
#endif
        }

        private BannerConfig GetBannerConfig(string tag)
        {
            BannerConfig ret = null;
            foreach (var ad in _bannerConfigs)
            {
                if (ad == null) continue;
                ret = ad;
                if (ad.tag == tag)
                {
                    break;
                }
            }
            return ret;
        }

        internal void ShowBanner(string tag, BannerPosition bp, System.Action<BannerAdResult> rlt = null)
        {
            var config = GetBannerConfig(tag);
            if (config == null)
            {
                rlt?.Invoke(new BannerAdResult
                {
                    errcode = Utils.SdkErrorCode.AdNotFind,
                    errmsg = $"banner {tag} not find",
                });
                return;
            }
            I?.ShowBanner(config, bp, rlt);
        }


        internal void ShowBanner(string tag, int left, int top, int width, System.Action<BannerAdResult> rlt = null)
        {
            var config = GetBannerConfig(tag);
            if (config == null)
            {
                rlt?.Invoke(new BannerAdResult
                {
                    errcode = Utils.SdkErrorCode.AdNotFind,
                    errmsg = $"banner {tag} not find",
                });
                return;
            }
            I?.ShowBanner(config, left, top, width, rlt);
        }

        internal void HideBanner()
        {
            I?.HideBanner();
        }
    }
}


using UnityEngine;

namespace Ivy.MiniGame.Ad
{
#if IVYSDK_DY
    internal class ByteBannerAd : BannerInterface
    {
        private int px2dp(int px) => (int)(px * (160 / Screen.dpi));
        
        private StarkSDKSpace.StarkAdManager.BannerStyle ToBannerStyle(BannerPosition bp)
        {
            return new StarkSDKSpace.StarkAdManager.BannerStyle {
                top = 100,
                left = 10,
                width = 320,
            };
        }
        
        StarkSDKSpace.StarkAdManager.BannerAd m_bannerAdIns = null;
        ///显示banner广告
        public void ShowBanner(BannerConfig conf, int left, int top, int width, System.Action<BannerAdResult> rlt)
        {
            HideBanner();

            var m_style = new StarkSDKSpace.StarkAdManager.BannerStyle
            {
                top = top,
                left = left,
                width = width,
            };
            m_bannerAdIns = StarkSDKSpace.StarkSDK.API.GetStarkAdManager()
                .CreateBannerAd(conf.adUnitId, m_style, conf.adIntervals,
                    (code, msg) =>
                    {
                        rlt?.Invoke(new BannerAdResult
                        {
                            errcode = Utils.SdkErrorCode.AdNotLoad,
                            errmsg = msg,
                        });
                    },
                    () =>
                    {
                        m_bannerAdIns?.Show();
                        rlt?.Invoke(new BannerAdResult
                        {
                            errcode = Utils.SdkErrorCode.Ok
                        });
                    });
        }

        ///显示banner广告
        public void ShowBanner(BannerConfig conf, BannerPosition bp, System.Action<BannerAdResult> rlt)
        {
            var style = ToBannerStyle(bp);
            ShowBanner(conf, style.left, style.top, style.width, rlt);
        }

        ///隐藏banner广告
        public void HideBanner()
        {
            if (m_bannerAdIns == null) return;
            m_bannerAdIns.Hide();
            m_bannerAdIns.Destroy();
            m_bannerAdIns = null;
        }
    }
#endif
}


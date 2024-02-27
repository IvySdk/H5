using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ivy.MiniGame.Ad
{
#if IVYSDK_WX
    internal class WeChatBanner : BannerInterface
    {
        private BannerStyle ToBannerStyle(BannerPosition bp)
        {
            return new BannerStyle {
                width = 400,
            };
        }
        
        private WeChatWASM.WXBannerAd nBanner = null;
        ///显示banner广告
        public void ShowBanner(BannerConfig config, int left, int top, int width, System.Action<BannerAdResult> rlt)
        {
            if (nBanner != null)
            {
                nBanner.Hide();
                nBanner = null;
            }

            nBanner = WeChatWASM.WX.CreateBannerAd(new WeChatWASM.WXCreateBannerAdParam
            {
                adUnitId = config.adUnitId,
                adIntervals = config.adIntervals,
                style = {
                    left = left,
                    top = top,
                    width = width
                }
            });
            
            nBanner.OnLoad(resp =>
            {
                nBanner.Show();
                rlt?.Invoke(new BannerAdResult
                {
                    errcode = Utils.SdkErrorCode.Ok
                });
            });
            
            nBanner.OnError(resp =>
            {
                rlt?.Invoke(new BannerAdResult
                {
                    errcode = Utils.SdkErrorCode.AdNotLoad,
                    errmsg = $"banner {config.adUnitId} not load",
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
            if (nBanner == null) return;
            nBanner.Hide();
            nBanner = null;
        }
    }
#endif
}




using System.Collections.Generic;
using UnityEngine;

namespace Ivy.MiniGame.Ad
{
    [System.Serializable]
    public class InterstitialConfig
    {
        [SerializeField] public string tag;
        [SerializeField] public string adUnitId;
    }

    internal interface InterstitialInterface
    {
        ///显示插屏广告
        void ShowInterstitial(string id, System.Action<InterstitialAdResult> rlt);
    }

    internal class InterstitialManager
    {
        private readonly List<InterstitialConfig> _interstitialConfigs;
        private readonly InterstitialInterface I;
        internal InterstitialManager(List<InterstitialConfig> interstitialConfigs)
        {
            _interstitialConfigs = interstitialConfigs;
#if IVYSDK_DY
            I = new ByteInterstitialAd();
#elif IVYSDK_WX
            I = new WeChatInterstitial();
#endif
        }

        private InterstitialConfig GetInterstitialConfig(string tag)
        {
            InterstitialConfig ret = null;
            foreach (var ad in _interstitialConfigs)
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

        internal void ShowInterstitial(string tag, System.Action<InterstitialAdResult> rlt = null)
        {
            var config = GetInterstitialConfig(tag);
            if (config == null)
            {
                rlt?.Invoke(new InterstitialAdResult
                {
                    errcode = Utils.SdkErrorCode.AdNotFind,
                    errmsg = $"Interstitial {tag} not find",
                });
                return;
            }

            I?.ShowInterstitial(config.adUnitId, rlt);
        }
    }
}



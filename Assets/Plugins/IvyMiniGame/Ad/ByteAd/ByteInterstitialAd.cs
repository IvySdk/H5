using System;

namespace Ivy.MiniGame.Ad
{
#if CN_DY
    internal class ByteInterstitialAd : InterstitialInterface
    {
        private bool isInterstitialShowing = false;
        public void ShowInterstitial(string id, Action<InterstitialAdResult> rlt)
        {
            if (isInterstitialShowing)
            {
                rlt?.Invoke(new InterstitialAdResult
                {
                    errcode = Utils.SdkErrorCode.AdIsShowing,
                    errmsg = "Interstitial is showing",
                });
                return;
            }
            
            isInterstitialShowing = true;
            StarkSDKSpace.StarkAdManager.InterstitialAd m_InterAdIns = null;
            m_InterAdIns = StarkSDKSpace.StarkSDK.API.GetStarkAdManager().CreateInterstitialAd(id,
                (code, msg) =>
                {
                    m_InterAdIns?.Destroy();
                    isInterstitialShowing = false;
                    rlt?.Invoke(new InterstitialAdResult
                    {
                        errcode = Utils.SdkErrorCode.AdNotFind,
                        errmsg = msg
                    });
                },
                () =>
                {
                    m_InterAdIns?.Destroy();
                    isInterstitialShowing = false;
                    rlt?.Invoke(new InterstitialAdResult
                    {
                        errcode = Utils.SdkErrorCode.Ok
                    });
                    
                },
                () =>
                {
                    m_InterAdIns?.Show();
                });
            m_InterAdIns.Load();
        }
    }
#endif
}

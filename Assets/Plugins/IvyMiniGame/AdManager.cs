using System;

namespace Ivy.MiniGame
{
    public static class AdManager
    {
        private static Ad.BannerManager _bannerManager;
        private static Ad.RewardedVideoManager _rewardedVideoManager;
        private static Ad.InterstitialManager _interstitialManager;

        public static void InitAdManager()
        {
            _bannerManager = new Ad.BannerManager(MiniGameConfig.bannerConfigArray);
            _rewardedVideoManager = new Ad.RewardedVideoManager(MiniGameConfig.videoConfigArray);
            _interstitialManager = new Ad.InterstitialManager(MiniGameConfig.interConfigArray);
        }

        #region 广告

        public static void ShowBanner(string tag, BannerPosition bp, System.Action<BannerAdResult> rlt = null)
        {
            _bannerManager?.ShowBanner(tag, bp, rlt);
        }

        public static void ShowBanner(string tag, int left, int top, int width, System.Action<BannerAdResult> rlt = null)
        {
            _bannerManager?.ShowBanner(tag, left, top, width, rlt);
        }

        public static void HideBanner()
        {
            _bannerManager?.HideBanner();
        }

        public static void ShowRewardedVideo(string tag, Action onSuccess, System.Action<RewardedVideoAdResult> onFail = null)
        {
            _rewardedVideoManager?.ShowRewardedVideo(tag, onSuccess, onFail);
        }

        public static void ShowInterstitial(string tag, System.Action<InterstitialAdResult> rlt = null)
        {
            _interstitialManager?.ShowInterstitial(tag, rlt);
        }

        public static bool HasRewardAd()
        {
            return true;
        }

        public static void ShowRewardedVideo(int rewardId)
        {
            _rewardedVideoManager?.ShowRewardedVideo("main", 
                () => { RiseSdkListener.Instance.onReceiveReward($"0|{rewardId}|main|0"); },
                rewardedVideoAdResult => { RiseSdkListener.Instance.onReceiveReward($"-1|{rewardId}|main|0"); }
            );
        }

        public static bool HasInterstitial()
        {
            return true;
        }

        public static void ShowInterstitial()
        {
            _rewardedVideoManager?.ShowRewardedVideo("main");
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ivy.MiniGame.Ad
{
    [System.Serializable]
    public class RewardedVideoConfig
    {
        [SerializeField] public string tag;
        [SerializeField] public string adUnitId;
    }

    internal interface RewardedVideoInterface
    {
        ///显示视频广告
        void ShowRewardedVideo(string id, System.Action onSuccess, System.Action<RewardedVideoAdResult> onFail);
    }

    internal class RewardedVideoManager
    {
        private readonly List<RewardedVideoConfig> _rewardedConfigs;
        private readonly RewardedVideoInterface I;
        internal RewardedVideoManager(List<RewardedVideoConfig> rewardedConfigs)
        {
            _rewardedConfigs = rewardedConfigs;
#if IVYSDK_DY
            I = new ByteVideoAd();
#elif IVYSDK_WX
            I = new WeChatRewardedVideo();
#endif
        }

        private RewardedVideoConfig GetRewardedConfig(string tag)
        {
            RewardedVideoConfig ret = null;
            foreach (var ad in _rewardedConfigs)
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

        public void ShowRewardedVideo(string tag, System.Action onSuccess = null, System.Action<RewardedVideoAdResult> onFail = null)
        {
            var config = GetRewardedConfig(tag);
            if (config == null)
            {
                onFail?.Invoke(new RewardedVideoAdResult
                {
                    errcode = Utils.SdkErrorCode.AdNotFind,
                    errmsg = $"rewarded video {tag} not find",
                });
                return;
            }

            I?.ShowRewardedVideo(config.adUnitId, onSuccess, onFail);
        }
    }
}


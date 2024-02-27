using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ivy.MiniGame.Ad
{
#if IVYSDK_WX
    internal class WeChatRewardedVideo : RewardedVideoInterface
    {
        private bool IsRewardShowing = false;
        public void ShowRewardedVideo(string id, System.Action onSuccess, System.Action<RewardedVideoAdResult> onFail)
        {
            if (IsRewardShowing)
            {
                onFail?.Invoke(new RewardedVideoAdResult
                {
                    errcode = Utils.SdkErrorCode.AdIsShowing,
                    errmsg = "rewarded video is showing",
                });
                return;
            }

            IsRewardShowing = true;
            var ad = WeChatWASM.WX.CreateRewardedVideoAd(new WeChatWASM.WXCreateRewardedVideoAdParam
            {
                adUnitId = id
            });
            ad.OnLoad(resp =>
            {
                ad.Show();
            });
            ad.OnError(resp =>
            {
                IsRewardShowing = false;
                onFail?.Invoke(new RewardedVideoAdResult
                {
                    errcode = Utils.SdkErrorCode.AdNotLoad,
                    errmsg = $"rewarded video {id} not load",
                });
            });
            ad.OnClose(resp =>
            {
                IsRewardShowing = false;
                if (resp != null && resp.isEnded)
                {
                    onSuccess?.Invoke();
                }
                else
                {
                    onFail?.Invoke(new RewardedVideoAdResult
                    {
                        errcode = Utils.SdkErrorCode.AdNotFullyDisplayed,
                        errmsg = $"rewarded video {id} not fully displayed",
                    });
                }
            });
        }
    }
#endif

}



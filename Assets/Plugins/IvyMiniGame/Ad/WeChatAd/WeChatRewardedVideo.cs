using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ivy.MiniGame.Ad
{
#if CN_WX
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
                // if ((resp == null) || resp.isEnded) {
                //     // 正常播放结束，可以下发游戏奖励
                // }
                // else {
                //     // 播放中途退出，不下发游戏奖励
                // }
                onSuccess?.Invoke();
            });
        }
    }
#endif

}



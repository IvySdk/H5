using System;

namespace Ivy.MiniGame.Ad
{
#if IVYSDK_DY
    internal class ByteVideoAd : RewardedVideoInterface
    {
        private bool IsRewardShowing = false;

        public void ShowRewardedVideo(string id, Action onSuccess, Action<RewardedVideoAdResult> onFail)
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
            StarkSDKSpace.StarkSDK.API.GetStarkAdManager().ShowVideoAdWithId(id,
                isComplete =>
                {
                    IsRewardShowing = false;
                    if (isComplete)
                    {
                        onSuccess?.Invoke();
                    }
                    else
                    {
                        onFail?.Invoke(new RewardedVideoAdResult
                        {
                            errcode = Utils.SdkErrorCode.Ok
                        });
                    }
                }, (code, msg) =>
                {
                    IsRewardShowing = false;
                    onFail?.Invoke(new RewardedVideoAdResult
                    {
                        errcode = Utils.SdkErrorCode.AdNotFind,
                        errmsg = msg
                    });
                });
        }

        private class VideoCallBack : StarkSDKSpace.StarkAdManager.VideoAdCallback
        {
            private readonly Action<bool> rCb;
            private readonly Action<RewardedVideoAdResult> rlt;
            internal VideoCallBack(Action<bool> ret, Action<RewardedVideoAdResult> rlt)
            {
                rCb = ret;
                this.rlt = rlt;
            }

            public void OnVideoLoaded()
            {
                
            }

            public void OnVideoShow(long timestamp)
            {

            }

            public void OnError(int errCode, string errorMessage)
            {

            }

            public void OnVideoClose(int watchedTime, int effectiveTime, int duration)
            {
            }
        }
    }
#endif
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ivy.MiniGame.Ad
{
#if IVYSDK_WX
    internal class WeChatInterstitial : InterstitialInterface
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
            var ad = WeChatWASM.WX.CreateInterstitialAd(new WeChatWASM.WXCreateInterstitialAdParam
            {
                adUnitId = id
            });

            ad.OnLoad(resp =>
            {
                ad.Show();
            });
            ad.OnError(resp =>
            {
                isInterstitialShowing = false;
                rlt?.Invoke(new InterstitialAdResult
                {
                    errcode = Utils.SdkErrorCode.AdNotLoad,
                    errmsg = $"Interstitial {id} not load",
                });
            });
            ad.OnClose(() =>
            {
                isInterstitialShowing = false;
                rlt?.Invoke(new InterstitialAdResult
                {
                    errcode = Utils.SdkErrorCode.Ok
                });
            });
        }
    }
#endif
}


using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using System.Collections;

public class AdsManager : MonoBehaviour
{
    private static BannerView bannerAd;
    private static InterstitialAd interstitialAd;
    private static RewardedAd rewardedAd;

    public static AdsManager i;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (i != null && i != this)
            Destroy(this.gameObject);
        else
            i = this;
    }

    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadInterstitial();
        });
        LoadRewardedAd();
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest();
    }

    public void RequestBanner()
    {
        string adUnitId = "ca-app-pub-5080271606964191/1943441638";
        // Clean up banner ad before creating a new one.
        if (bannerAd != null)
        {
            bannerAd.Destroy();
            bannerAd = null;
        }
        AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        bannerAd = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);
        AdRequest request = new AdRequest();
        // Load a banner ad.
        bannerAd.LoadAd(request);
    }

    public void DestroyBanner()
    {
        if(bannerAd != null)
        {
            bannerAd.Destroy();
            bannerAd = null;
            print("destroyed");
        }
    }

    public void LoadInterstitial()
    {
        string adUnitId = "ca-app-pub-5080271606964191/7219302125";
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
            });
        //RegisterInterEventHandlers(interstitialAd);
    }

    public void ShowInterstitial()
    {
        print("showing interstitial ad");
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            print("showing");
            interstitialAd.Show();
        }
        else
        {
            print("failed to show interstitial ad");
        }
        LoadInterstitial();
    }

    //private void RegisterInterEventHandlers(InterstitialAd ad)
    //{
    //    // Raised when the ad is estimated to have earned money.
    //    ad.OnAdPaid += (AdValue adValue) =>
    //    {
    //        Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
    //            adValue.Value,
    //            adValue.CurrencyCode));
    //    };
    //    // Raised when an impression is recorded for an ad.
    //    ad.OnAdImpressionRecorded += () =>
    //    {
    //        Debug.Log("Interstitial ad recorded an impression.");
    //    };
    //    // Raised when a click is recorded for an ad.
    //    ad.OnAdClicked += () =>
    //    {
    //        Debug.Log("Interstitial ad was clicked.");
    //    };
    //    // Raised when an ad opened full screen content.
    //    ad.OnAdFullScreenContentOpened += () =>
    //    {
    //        Debug.Log("Interstitial ad full screen content opened.");
    //    };
    //    // Raised when the ad closed full screen content.
    //    ad.OnAdFullScreenContentClosed += () =>
    //    {
    //        //LoadInterstitial();
    //        Debug.Log("Interstitial ad full screen content closed.");
    //    };
    //    // Raised when the ad failed to open full screen content.
    //    ad.OnAdFullScreenContentFailed += (AdError error) =>
    //    {
    //        //LoadInterstitial();
    //        Debug.LogError("Interstitial ad failed to open full screen content " +
    //                       "with error : " + error);
    //    };
    //}

    public void LoadRewardedAd()
    {
        string _adUnitId = "ca-app-pub-5080271606964191/1330079999";
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        print("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    //Debug.LogError("Rewarded ad failed to load an ad " +
                    //               "with error : " + error);
                    return;
                }

                //Debug.Log("Rewarded ad loaded with response : "
                //          + ad.GetResponseInfo());

                rewardedAd = ad;
            });
    }

    public bool ShowRewardedAd()
    {
        //const string rewardMsg =
        //    "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                //Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
            this.LoadRewardedAd();
            return true;
        }
        this.LoadRewardedAd();
        return false;
    }
}

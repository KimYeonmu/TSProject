using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : SingletonBase<AdsManager>
{
    public RewardBasedVideoAd RewardVideo;

    private string _testId = "ca-app-pub-3940256099942544/5224354917";
    private string _appId = "ca-app-pub-1353248049666322~8103106444";

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(_appId);

        RewardVideo = RewardBasedVideoAd.Instance;

        var adReq = new AdRequest.Builder().Build();
        RewardVideo.LoadAd(adReq, _testId);
    }

    public void ShowAd()
    {
        if (RewardVideo.IsLoaded())
        {
            Debug.Log("ads");
            RewardVideo.Show();
        }
        else
        {
            var adReq = new AdRequest.Builder().Build();
            RewardVideo.LoadAd(adReq, _testId);
        }
    }
}



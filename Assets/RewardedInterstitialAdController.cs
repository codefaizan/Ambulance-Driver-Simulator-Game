using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardedInterstitialAdController : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text timerText;

    private void OnEnable()
    {
        if(GameController.state == GameState.LevelComplete)
        {
            titleText.text = "Level Completed!";
            descriptionText.text = "Watch an ad to get 2x coins";
        }
        else if(GameController.state == GameState.GameOver)
        {
            titleText.text = "Game Over!";
            descriptionText.text = "Watch an ad to extend timer and continue without loosing the progress";
        }

        StartCoroutine(StartRewardedInterstitialAdTimer());
    }

    IEnumerator StartRewardedInterstitialAdTimer()
    {
        timerText.text = "5";
        int t = 6;
        while (t > 0)
        {
            t--;
            timerText.text = $"Ad will play in: {t}s";
            yield return new WaitForSeconds(1f);
        }

        // show rewarded interstitial ad
        if (!AdsManager.i.ShowRewardedInterstitialAd())
        {
            timerText.text = "Sorry! Ad is not available right now.";
        }
        else
            this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

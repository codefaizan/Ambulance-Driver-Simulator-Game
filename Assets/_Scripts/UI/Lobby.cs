using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject levelsPanel;
    [SerializeField] GameObject loadingPanel;
    public Button buyButton;
    public Button startButton;
    public TMP_Text buyButtonText;
    [SerializeField] TMP_Text coinsTextMainScreen;
    [SerializeField] TMP_Text coinsTextLevelScreen;
    public static Lobby i;

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        levelsPanel.SetActive(false);
        loadingPanel.SetActive(false);
        buyButton.gameObject.SetActive(false);
        RefreshCoinsText();
        AdsManager.i.RequestBanner();
    }

    internal void RefreshCoinsText()
    {
        coinsTextMainScreen.text = RewardsManager.GetCurrentRewardCount().ToString("N0");
        coinsTextLevelScreen.text = RewardsManager.GetCurrentRewardCount().ToString("N0");
    }

    public void OnStartButtonClicked()
    {
        PlayerPrefs.SetInt("selected car", CarsHolder._selectedCarIndex);
        lobbyPanel.SetActive(false);
        levelsPanel.SetActive(true);
    }

    internal void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }

    public void CoinRewardOnWatchAd()
    {
        if (AdsManager.i.ShowRewardedAd())
        {
            RewardsManager.UpdateReward(Random.Range(100, 500));
            RefreshCoinsText();
        }
    }
}

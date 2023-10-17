using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject doorOpenBtn;
    [SerializeField] GameObject doorCloseBtn;
    [SerializeField] GameObject nextLevelBtn;
    [SerializeField] GameObject instructionsPanel;
    [SerializeField] GameObject levelCompletePanel;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] Text passengerCounter;
    [SerializeField] Animator screenFade;
    [SerializeField] GameObject pausePanel;
    [SerializeField] internal GameObject popupRewardPanel; // rewarded interstitial AD panel
    public static UIController i;

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        doorOpenBtn.SetActive(false);
        doorCloseBtn.SetActive(false);
        levelCompletePanel.SetActive(false);
        loadingPanel.SetActive(false);
        pausePanel.SetActive(false);
        popupRewardPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        if(PlayerPrefs.GetInt("first play", 1) == 0)
        {
            instructionsPanel.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("first play", 0);
            instructionsPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void StartGame()
    {
        instructionsPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UpdatePassengerCounter(int count)
    {
        passengerCounter.text = (int.Parse(passengerCounter.text)+count).ToString();
    }

    public void ActivateOpenDoorButton(bool active)
    {
        doorOpenBtn.SetActive(active);
    }

    public void ActivateCloseDoorButton(bool active)
    {
          doorCloseBtn.SetActive(active);
    }

    public void FadeScreen()
    {
        screenFade.SetTrigger("fade");
    }

    internal void ActivateLevelCompletePanel()
    {
        levelCompletePanel.SetActive(true);
        Invoke("ActivePopupRewardPanel", 1.5f);
    }

    internal void UpdateRewardText(int coinsReward)
    {
        coinsText.text = (coinsReward).ToString("N0");
    }

    public void Reload()
    {
        if (GameController.state == GameState.LevelComplete)
        {
            if (LevelSelector.currentLevel >= GameController.i.levels.levelsDetail.Length-1)
            {
                nextLevelBtn.SetActive(false);
                Time.timeScale = 1f;
                return;
            }
            LevelSelector.selectedLevel++;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Siren.ActiveSiren(false);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Siren.ActiveSiren(true);
        Time.timeScale = 1f;
    }

    public void ActiveGameOverPanel(bool state)
    {
        gameOverPanel.SetActive(state);
        if(state)
            Invoke("ActivePopupRewardPanel", 0.7f);
    }

    public void ActivePopupRewardPanel()
    {
        popupRewardPanel.SetActive(true);
    }

    public void LoadMainMenuScene()
    {
        loadingPanel.SetActive(true);
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}

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
    [SerializeField] Button doubleRewardBtn;
    [SerializeField] GameObject nextLevelBtn;
    [SerializeField] GameObject instructionsPanel;
    [SerializeField] GameObject levelCompletePanel;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] Text passengerCounter;
    [SerializeField] Animator screenFade;
    [SerializeField] GameObject pausePanel;
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
    }

    internal void UpdateRewardText(int coinsReward)
    {
        coinsText.text = (coinsReward).ToString("N0");
    }

    public void Reload()
    {
        if (GameController.levelCompleted)
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

    public void SetGameOverPanelState(bool state)
    {
        gameOverPanel.SetActive(state);
    }

    internal void OnDoubleRewardAdSuccessfullyPlayed()
    {
        doubleRewardBtn.interactable = false;
    }

    public void LoadMainMenuScene()
    {
        loadingPanel.SetActive(true);
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}

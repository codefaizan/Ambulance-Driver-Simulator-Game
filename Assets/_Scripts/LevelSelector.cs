using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] Button[] levelButtons;
    public static int currentLevel; // highest unlocked level
    public static int selectedLevel; // level selected by player

    private void Start()
    {
        foreach(Button button in levelButtons)
        {
            button.interactable = false;
        }
        currentLevel = PlayerPrefs.GetInt("current level", 0);
        for(int i=0; i<=currentLevel; i++)
        {
            levelButtons[i].interactable = true;
        }
    }

    public void SelectLevel(int id)
    {
        selectedLevel = id;
        SceneManager.LoadScene(1);
        Lobby.i.ShowLoadingPanel();
    }
}

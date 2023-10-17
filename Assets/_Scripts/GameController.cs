using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] public Levels levels;
    int passengerCount;
    public static GameController i;
    public List<GameObject> passengers = new List<GameObject>();
    private Transform charactersParent;
    GameObject currentLevelObj; // current active (spawned) level
    GameObject currentCarObj;  // current active (spawned) vehicle
    internal GameObject currentPerson; // current person that player have to pick up
    [SerializeField] GameObject[] dropPoints;
    internal static GameState state;

    private void Awake()
    {
        i = this;
        //levelCompleted = false;
        state = GameState.Playing;

        LevelsDetail selectedLevelDetails;
        selectedLevelDetails = levels.levelsDetail[LevelSelector.selectedLevel];
        currentLevelObj =  Instantiate(selectedLevelDetails.levelPrefab);
        currentCarObj = Instantiate(levels.carsDetail[CarsHolder._selectedCarIndex].carPrefab, selectedLevelDetails.spawnPoint.position, selectedLevelDetails.spawnPoint.rotation);
        ActivateNextPerson();
    }

    private void Start()
    {
        charactersParent = Ambulance.i.npcParent;
        SetTimer();
        AdsManager.i.DestroyBanner();
        //AdsManager.i.ShowInterstitial();
    }

    private void SetTimer()
    {
        Timer.i.SetTimer(0, levels.levelsDetail[LevelSelector.selectedLevel].timeLimit);
    }

    void ActivateNextPerson()
    {
        currentPerson = null;
        if(currentLevelObj.transform.childCount > 0)
        {
        currentPerson = currentLevelObj.transform.GetChild(0).gameObject;
        currentPerson.SetActive(true);
        }
        if (currentPerson != null)
            PlayerPathRenderer.i.endPosition = currentPerson.transform;
        // if all persons are picked up then set line-renderer target to drop point
        else if(currentPerson == null)
        {
            float distance = Vector3.Distance(dropPoints[0].transform.position, Ambulance.i.transform.position);
            PlayerPathRenderer.i.endPosition = dropPoints[0].transform;
            for(int i = 1; i<dropPoints.Length; i++)
            {
                if (Vector3.Distance(dropPoints[i].transform.position, Ambulance.i.transform.position) < distance)
                {
                    distance = Vector3.Distance(dropPoints[i].transform.position, Ambulance.i.transform.position);
                    PlayerPathRenderer.i.endPosition = dropPoints[i].transform;
                }
            }
        }
    }

    public void OnEnterVehicle(GameObject obj)
    {
        obj.transform.SetParent(charactersParent);
        obj.SetActive(false);
        passengers.Add(obj); 
        UIController.i.UpdatePassengerCounter(1);
        UIController.i.ActivateCloseDoorButton(true);
        passengerCount++;
        ActivateNextPerson();
    }

    public void OnExitVehicle()
    {
        if (passengerCount > 0)
        {
            UIController.i.UpdatePassengerCounter(-1);
            passengerCount--;
            UIController.i.ActivateCloseDoorButton(true);
            if (passengerCount == 0 && currentLevelObj.transform.childCount == 0)
            {
                OnLevelComplete();
            }
        }
    }




    private IEnumerator ActivateNpcObjects()
    {
        foreach (GameObject obj in passengers)
        {
            yield return new WaitForSeconds(1f);
            if(obj != null)
            {
                obj.SetActive(true);
                Destroy(obj, 2f);
            }
        }
    }

    public void OpenDoor()
    {
        if (Ambulance.currentStopPoint == "pickup point")
            currentPerson.GetComponent<controller>().walking = true;
        else if (Ambulance.currentStopPoint == "drop point" && passengerCount > 0)
        {
            UIController.i.FadeScreen();
            StartCoroutine(ActivateNpcObjects());
        }

        Ambulance.i.ToggleVehicleKinematicState();
    }

    public void CloseDoor()
    {
        Ambulance.i.ToggleVehicleKinematicState();
    }

    ///////////////////////////////
    ///Level Complete
    ///////////////////////////////

    void OnLevelComplete()
    {
        Siren.sirenAudio.Stop();
        UIController.i.ActivateLevelCompletePanel();
        state = GameState.LevelComplete;
        if (LevelSelector.selectedLevel >= LevelSelector.currentLevel)
        {
            UIController.i.UpdateRewardText(levels.levelsDetail[LevelSelector.selectedLevel].coinsReward);
            RewardsManager.UpdateReward(levels.levelsDetail[LevelSelector.selectedLevel].coinsReward);
            PlayerPrefs.SetInt("current level", PlayerPrefs.GetInt("current level", 0) + 1);
        }
        AdsManager.i.ShowInterstitial();
    }

    public void DoubleReward()
    {
        //if (AdsManager.i.ShowRewardedAd())
        //{
            if (LevelSelector.selectedLevel >= LevelSelector.currentLevel)
            {
                RewardsManager.UpdateReward(levels.levelsDetail[LevelSelector.selectedLevel].coinsReward);
                UIController.i.UpdateRewardText(levels.levelsDetail[LevelSelector.selectedLevel].coinsReward * 2);
            }
        //}
    }

    ///////////////////////////////
    // Game Over 
    ///////////////////////////////

    public void OnGameOver()
    {
        state = GameState.GameOver;
        Siren.ActiveSiren(false);
        UIController.i.ActiveGameOverPanel(true);
        Ambulance.i.ToggleVehicleKinematicState();
        AdsManager.i.ShowInterstitial();
    }

    public void ContinueGameAfterGameOver()
    {
        state = GameState.Playing;
        Siren.ActiveSiren(true);
        // called when rewarded-inter ad is successfully closed
        //if (AdsManager.i.ShowRewardedAd())
        //{
            UIController.i.ActiveGameOverPanel(false);
            Ambulance.i.ToggleVehicleKinematicState();
            SetTimer();
        //}
    }
}

public enum GameState
{
    Waiting,
    Playing,
    GameOver,
    LevelComplete
}

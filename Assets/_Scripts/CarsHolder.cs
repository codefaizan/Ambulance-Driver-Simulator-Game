using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarsHolder : MonoBehaviour
{
    [SerializeField] Levels levels;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] List<GameObject> cars = new List<GameObject>();
    public static int _selectedCarIndex;
    public CarsBluePrint[] carsDetail;
    public CarsBluePrint currentCar;


    private void Start()
    {
        _selectedCarIndex = PlayerPrefs.GetInt("selected car", 0);
        foreach (GameObject car in cars)
        {
            //car.GetComponent<Rigidbody>().isKinematic = true;
            car.SetActive(false);
        }
        cars[_selectedCarIndex].SetActive(true);

        // unlocking purchased cars
        //foreach (CarsBluePrint car in carsDetail)
        //{
        //    if (car.price == 0)
        //    {
        //        car.isUnlocked = true;
        //    }
        //    else
        //        car.isUnlocked = PlayerPrefs.GetInt("car"+car.ID.ToString(), 0) == 0 ? false : true;
        //}
        foreach (CarsDetail car in levels.carsDetail)
        {
            if (car.price == 0)
            {
                car.isUnlocked = true;
            }
            else
                car.isUnlocked = PlayerPrefs.GetInt("car" + car.ID.ToString(), 0) == 0 ? false : true;
        }
    }

    void Update()
    {
        transform.Rotate(new Vector3(0f, rotateSpeed, 0f)*Time.deltaTime);
        
    }

    public void ToggleNextCar()
    {
        // select next car
        if (_selectedCarIndex < cars.Count - 1)
        {
            _selectedCarIndex++;
            UpdateUI();
            for (int i = 0; i < cars.Count; i++)
            {
                cars[i].SetActive(false);
            }
            cars[_selectedCarIndex].SetActive(true);
        }
    }

    public void TogglePreviousCar()
    {
        // select previous car
        if (_selectedCarIndex > 0)
        {
            _selectedCarIndex--;
            UpdateUI();
            for (int i = 0; i < cars.Count; i++)
            {
                cars[i].SetActive(false);
            }
            cars[_selectedCarIndex].SetActive(true);
        }
    }

    public void BuyCar()
    {
        //if (carsDetail[_selectedCarIndex].price <= RewardsManager.currentCoins)
        if (levels.carsDetail[_selectedCarIndex].price <= RewardsManager.currentCoins)
        {
            RewardsManager.UpdateReward(-carsDetail[_selectedCarIndex].price);
            //carsDetail[_selectedCarIndex].isUnlocked = true;
            levels.carsDetail[_selectedCarIndex].isUnlocked = true;
            PlayerPrefs.SetInt("car"+levels.carsDetail[_selectedCarIndex].ID.ToString(), 1);
            UpdateUI();
            Lobby.i.RefreshCoinsText();
        }
    }

    void UpdateUI()
    {
        //Lobby.i.buyButtonText.text = carsDetail[_selectedCarIndex].price.ToString();
        Lobby.i.buyButtonText.text = levels.carsDetail[_selectedCarIndex].price.ToString();
        //if (!carsDetail[_selectedCarIndex].isUnlocked)
        if (!levels.carsDetail[_selectedCarIndex].isUnlocked)
        {
            Lobby.i.buyButton.gameObject.SetActive(true);
            Lobby.i.startButton.interactable = false;

            //if (carsDetail[_selectedCarIndex].price <= RewardsManager.currentCoins)
            if (levels.carsDetail[_selectedCarIndex].price <= RewardsManager.currentCoins)
            {
                Lobby.i.buyButton.interactable = true;
            }
            else
            {
                Lobby.i.buyButton.interactable = false;
            }
        }
        else
        {
            Lobby.i.buyButton.gameObject.SetActive(false);
            Lobby.i.startButton.interactable = true;
        }
    }
}

[System.Serializable]
public class CarsBluePrint
{
    public string name;
    public int ID;
    public int price;
    public int requiredLevel;
    public bool isUnlocked;
}
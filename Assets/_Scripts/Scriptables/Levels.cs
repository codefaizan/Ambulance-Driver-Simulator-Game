using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="MySO/Levels")]
public class Levels : ScriptableObject
{
    public LevelsDetail[] levelsDetail;
    public CarsDetail[] carsDetail;
}

[System.Serializable]
public class LevelsDetail
{
    public GameObject levelPrefab;
    public int coinsReward;
    public int timeLimit;
    public Transform spawnPoint;
}

[System.Serializable]
public class CarsDetail
{
    public GameObject carPrefab;
    public int ID;
    public int price;
    public int requiredLevel;
    public bool isUnlocked;
}

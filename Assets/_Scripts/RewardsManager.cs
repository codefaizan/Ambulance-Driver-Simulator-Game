using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsManager : MonoBehaviour
{
    public static int currentCoins;
    void Awake()
    {
        currentCoins = PlayerPrefs.GetInt("reward", 0);
    }

    internal static int GetCurrentRewardCount()
    {
        return currentCoins;
    }

    internal static void UpdateReward(int reward)
    {
        currentCoins += reward;
        PlayerPrefs.SetInt("reward", currentCoins);
    }
}

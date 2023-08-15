using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Siren : MonoBehaviour
{
    internal static AudioSource sirenAudio;
    internal static bool sirenOn;
    [SerializeField] GameObject muteImg;

    private void Awake()
    {
        sirenOn = PlayerPrefs.GetInt("siren", 1) == 1 ? true : false;
        sirenAudio = GetComponent<AudioSource>();

        if (sirenOn)
        {
            sirenAudio.mute = false;
            muteImg.SetActive(false);
        }
        else
        {
            sirenAudio.mute = true;
            muteImg.SetActive(true);
        }
    }

    public void ToggleSirenOnButton()
    {
        sirenOn = !sirenOn;
        PlayerPrefs.SetInt("siren", sirenOn ? 1 : 0);
        if (sirenOn)
        {
            sirenAudio.mute = false;
            muteImg.SetActive(false);
        }
        else
        {
            sirenAudio.mute = true;
            muteImg.SetActive(true);
        }
    }

    internal static void ActiveSiren(bool sirenState)
    {
        if (sirenState)
        {
            if(sirenOn)
                sirenAudio.mute = false;
        }
        else
        {
            if(sirenOn)
                sirenAudio.mute = true;
        }
    }
}

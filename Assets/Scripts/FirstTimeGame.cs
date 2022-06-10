using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeGame : MonoBehaviour
{
    void Start()
    {
        StartFirstTime();
    }

    public void StartFirstTime()
    {
        if (PlayerPrefs.GetInt("FirstTimeOpening", 1) == 1)
        {
            PlayerPrefs.SetFloat("musicaVol", -20);
            PlayerPrefs.SetFloat("gameVol", -20);
            Debug.Log("First Time Opening");
            PlayerPrefs.GetInt("firstTime", 0);
            PlayerPrefs.GetInt("Clip", 0);
            PlayerPrefs.SetInt("FirstTimeOpening", 0); // seta pra falso
        }
        else
        {
            Debug.Log("NOT First Time Opening");
            PlayerPrefs.SetInt("firstTime", 1);
        }
    }
}

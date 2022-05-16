using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController sc;
    void Awake() { sc = this; }

    public void ChangeScene(string nome)
    {
        Resume();
        SceneManager.LoadScene(nome);
    }

    public void FirstTimeGame()
    {
        if (PlayerPrefs.GetInt("firstTime") == 0)
        {
            SceneManager.LoadScene("Cutscene");
        }
        else
        {
            SceneManager.LoadScene("Jogo");
        }
    }

    // PAUSE
    public GameObject pausePanel;
    private bool pause = false;
    public void Pause()
    {
        if (!pause)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            pause = true;
        }
    }
    public void Resume()
    {
        if (pause)
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            pause = false;
        }
    }

    // CONFIGURAÇÕES
    public GameObject configPanel, menu;
    public void OpenConfig()
    {
        configPanel.SetActive(true);
        menu.SetActive(false);
    }
    public void CloseConfig()
    {   
        configPanel.SetActive(false);
        menu.SetActive(true);
    }

    // CREDITS
    public GameObject credits;
    public void OpenCredits()
    {
        credits.SetActive(true);
        menu.SetActive(false);
    }
    public void CloseCredits()
    {   
        credits.SetActive(false);
        menu.SetActive(true);
    }
}

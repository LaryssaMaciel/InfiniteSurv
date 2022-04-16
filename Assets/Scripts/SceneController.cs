using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ChangeScene(string nome)
    {
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
}

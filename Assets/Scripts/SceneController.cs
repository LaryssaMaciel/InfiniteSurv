using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController sc;
    void Awake() { sc = this; }

    public Animator animFade;

    public void ChangeScene(string nome)
    {
        Resume();
        animFade.SetTrigger("go");
        StartCoroutine(Wait(nome));
    }
    IEnumerator Wait(string nome) 
    {
        yield return new WaitForSeconds(1f);
        //Resume();
        SceneManager.LoadScene(nome);
    }

    public void FirstTimeGame()
    {
        if (PlayerPrefs.GetInt("firstTime") == 0)
        {
            ChangeScene("Cutscene");
        }
        else
        {
            ChangeScene("Jogo");
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

    [Header("Todos os sons")]
    public AudioClip[] som; // sons
    public AudioClip[] musicasCena;
    public AudioSource audioSource; // audio

    [Header("Mixers")]
    public AudioMixer MusMixer;
    public AudioMixer GamMixer;

    [Header("Sliders config. audio")]
    public Slider sliMus, sliGam;

    private float musVol = 1, gameVol = 1; // volumes
    private string sceneName; // nome da cena

    void Start()
    {   // configura variaveis
        audioSource = GetComponent<AudioSource>();
        sceneName = SceneManager.GetActiveScene().name.ToString();
        switch (sceneName)
        {   // define musica principal da cena
            case "Menu":
                AudioManager(0); // som de menu
                break;
            case "Jogo":
                AudioManager(1);
                break;
            case "GameOver":
                AudioManager(2); // som de gameplay
                break;
        }
        // pega valores salvos das configuracoes de audio
        musVol = PlayerPrefs.GetFloat("musicaVol");
        SetMusVolume(musVol);
        if (sliMus != null) { sliMus.value = musVol; }
        gameVol = PlayerPrefs.GetFloat("gameVol");
        SetGamVolume(gameVol);
        if (sliGam != null) {sliGam.value = gameVol; }
    }

    public void SetMusVolume(float vol) // define e salva configuracoes do volume da musica principal
    {
        MusMixer.SetFloat("volume", vol);
        PlayerPrefs.SetFloat("musicaVol", vol);
    }

    public void SetGamVolume(float vol) // define e salva configuracoes do volume de audio da gameplay
    {
        GamMixer.SetFloat("volume", vol);
        PlayerPrefs.SetFloat("gameVol", vol);
    }

    public void AudioManager(int audio)
    {
        audioSource.clip = musicasCena[audio];
        audioSource.Play();
    }
}

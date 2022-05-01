using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    public VideoClip[] video;
    public int clip;

    void Start()
    {
        clip = PlayerPrefs.GetInt("Clip");

        this.gameObject.GetComponent<VideoPlayer>().clip = video[clip];

        this.gameObject.GetComponent<VideoPlayer>().Play();
    }
    
    void Update()
    {
        

        // se chegou no último frame
        if ((ulong)GetComponent<VideoPlayer>().frame == (ulong)GetComponent<VideoPlayer>().frameCount - 3)
        {
            ChangeScene("Jogo");
        }
    }

    public void ChangeScene(string nome)
    {
        SceneManager.LoadScene(nome);
    }
}

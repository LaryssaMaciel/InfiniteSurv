using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    public VideoClip[] video; 
    private VideoPlayer vp;
    public int clip; // numero do video (qual video)

    void Start()
    {
        clip = PlayerPrefs.GetInt("Clip");
        vp = GetComponent<VideoPlayer>();
        this.gameObject.GetComponent<VideoPlayer>().clip = video[clip];
        this.gameObject.GetComponent<VideoPlayer>().Play(); 

        vp.SetDirectAudioVolume(0, (PlayerPrefs.GetFloat("musicaVol")/100) + 0.4f);
    }
    
    void Update()
    {
        // se chegou no último frame
        if ((ulong)GetComponent<VideoPlayer>().frame == (ulong)GetComponent<VideoPlayer>().frameCount - 3)
        {
            SceneController.sc.ChangeScene("Jogo");
        }
    }
}

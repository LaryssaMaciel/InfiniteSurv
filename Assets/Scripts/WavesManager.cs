using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public GameObject[] inimigos;
    float timer = 5; // tempo entre waves 
    float spawns = 3; // tanto de inimigos pra spawnar
    int tipo = 0; // tipo de inimigo baseado no array inimigos
    public float timeAtual, wave = 1;
    
    void Start()
    {
        foreach (Transform child in transform) { spawnPoints.Add(child); } // pega spawns
        timeAtual = timer;
    }

    void Update()
    {
        WavesManager_();
        timeAtual -= Time.deltaTime;

        Spawnar(inimigos[tipo]);
    }

    void Spawnar(GameObject inimigo)
    {
        if (timeAtual <= 0)
        {
            wave++;
            timeAtual = timer;
            for (int i = 0; i < spawns; i++)
            {
                int num = Random.Range(0, spawnPoints.Count - 1);
                Instantiate(inimigo, spawnPoints[num].transform.position, Quaternion.identity);
            }
        }
    }

    void WavesManager_() // gerencia tanto de inimigos e tipo por wave
    {
        switch(wave)
        {
            case 3-1: // nº q quer - 1 (resolver isso)
                spawns = 2;
                break;
            case 4-1:
                tipo = 1;
                break;
        }
    }
}

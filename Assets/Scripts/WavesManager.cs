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
    public float timeAtual, wave = 2;
    
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
        /*
        tipo 1 = gnominho
        tipo 2 = gnomo
        tipo 3 = gnomao
        tipo 4 = antignomo
        */
        switch(wave)
        {   // nº q quer - 1 (resolver isso)
            // wave 1
            case 2 -1: 
                tipo = 0; spawns = 2; break;
            // wave 2 a 4
            case 3 -1: 
                spawns = 3; break;
            // wave 5
            case 4 -1: 
                spawns = 5; break;
            // wave 6
            case 5 -1: 
                spawns = 2; timeAtual = 0; break; // continuação de wave
                case 6 -1: tipo = 1; spawns = 3; break;
            // wave 7
            case 7 -1: 
                tipo = 0; spawns = 4; timeAtual = 0; break; // continuação de wave
                case 8 -1: tipo = 1; spawns = 2; break;
            // wave 8
            case 9 -1: 
                tipo = 1; spawns = 4; break;
            // wave 9
            case 10 -1: 
                tipo = 0; spawns = 2;  timeAtual = 0; break; // continuação de wave
                case 11 -1: tipo = 1; spawns = 3; break;
            // wave 10
            case 12 -1: /* nova  parte do mapa */
                tipo = 0; spawns = 3; timeAtual = 0; break; // continuação de wave
                case 13 -1: tipo = 1; spawns = 5; break;
            // wave 11
            case 14 -1: 
                tipo = 0; spawns = 7; break;
            // wave 12
            case 15 -1: 
                tipo = 0; spawns = 1; break; // continuação de wave
                case 16 -1: tipo = 1; spawns = 5; break;
            // wave 13
            case 17 -1:
                tipo = 1; spawns = 6; break;
            // wave 14
            case 18 -1:
                tipo = 0; spawns = 3; timeAtual = 0; break; // continuação de wave
                case 19 -1: tipo = 1; spawns = 5; break;
            // wave 15
            case 20 -1: 
                tipo = 0; spawns = 3; timeAtual = 0; break; // continuação de wave
                case 21 -1: tipo = 1; spawns = 4; timeAtual = 0; break;
                case 22 -1: tipo = 2; spawns = 1; break;
            // wave 16
            case 23 -1:
                tipo = 1; spawns = 6; break;
            // wave 17
            case 24 -1: 
                tipo = 0; spawns = 5; timeAtual = 0; break; // continuação de wave
                case 25 -1: tipo = 1; spawns = 4; break;
            // wave 18
            case 26 -1: 
                tipo = 0; spawns = 5; timeAtual = 0; break; // continuação de wave
                case 27 -1: tipo = 2; spawns = 1; break;
            // wave 19
            case 28 -1:
                tipo = 1; spawns = 6; break;
            // wave 20
            case 29 -1: 
                tipo = 0; spawns = 4; timeAtual = 0; break; // continuação de wave
                case 30 -1: tipo = 1; spawns = 5; timeAtual = 0; break;
                case 31 -1: tipo = 2; spawns = 1; break;
            // wave 21
            case 32 -1:
                tipo = 1; spawns = 7; break;
            // wave 22
            case 33 -1:
                tipo = 0; spawns = 10; break;
            // wave 23
            case 34 -1:
                tipo = 0; spawns = 7; timeAtual = 0; break; // continuação de wave
                case 35 -1: tipo = 1; spawns = 3; timeAtual = 0; break;
                case 36 -1: tipo = 2; spawns = 1; break;
            // wave 24
            case 37 -1:
                tipo = 1; spawns = 7; timeAtual = 0; break;
                case 38 -1: tipo = 3; spawns = 1; break;
            // wave 25
            case 39 -1:
                tipo = 0; spawns = 4; timeAtual = 0; break;
                case 40 -1: tipo = 1; spawns = 5; timeAtual = 0; break;
                case 41 -1: tipo = 2; spawns = 3; break;
            // wave 26
            case 42 -1:
                tipo = 1; spawns = 2; timeAtual = 0; break;
                case 43 -1: tipo = 3; spawns = 3; break;
            // wave 27
            case 44 -1:
                tipo = 0; spawns = 4; timeAtual = 0; break;
                case 45 -1: tipo = 3; spawns = 5; break;
            // wave 28
            case 46 -1:
                tipo = 1; spawns = 2; timeAtual = 0; break;
                case 47 -1: tipo = 3; spawns = 7; break;
            // wave 29
            case 48 -1:
                // wave 33
                case 54 -1:
                // wave 35
                case 56 -1:
                tipo = 3; spawns = 10; break;
            // wave 30
            case 49 -1:
                tipo = 2; spawns = 3; timeAtual = 0; break;
                case 50 -1: tipo = 3; spawns = 8; break;
            // wave 31
            case 51 -1:
                tipo = 3; spawns = 7; break;
            // wave 32
            case 52 -1:
                tipo = 1; spawns = 3; timeAtual = 0; break;
                case 53 -1: tipo = 3; spawns = 6; break;
            // wave 34:
            case 55 -1:
                tipo = 3; spawns = 8; break;
            // wave 36
            case 57 -1:
                tipo = 0; spawns = 4; timeAtual = 0; break;
                case 58 -1: tipo = 3; spawns = 6; break;
            // wave 37
            case 59 -1:
                tipo = 1; spawns = 1; timeAtual = 0; break;
                case 60 -1: tipo = 3; spawns = 7; break;
            // wave 38
            case 61 -1:
                tipo = 0; spawns = 3; timeAtual = 0; break;
                case 62 -1: tipo = 3; spawns = 7; break;
            // wave 39
            case 63 -1:
                tipo = 1; spawns = 2; timeAtual = 0; break;
                case 64 -1: tipo = 3; spawns = 8; break;
            // wave 40
            case 65 -1:
                tipo = 0; spawns = 4; timeAtual = 0; break;
                case 66 -1: tipo = 2; spawns = 3; timeAtual = 0; break;
                case 67 -1: tipo = 3; spawns = 10; break;
        }
    }
}

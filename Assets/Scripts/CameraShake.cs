using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public bool shake = false;

    CinemachineVirtualCamera cvc;
    public float shakeTimer, shakeTimerTotal, startInt = 3;

    void Awake()
    {
        cvc = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (shake)
        {
            ShakeCamera(3f, .2f);
        }

        
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cbm = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cbm.m_AmplitudeGain = Mathf.Lerp(startInt, 0f, (1 - (shakeTimer / shakeTimerTotal)));
            shake = false;
        }
    }

    void ShakeCamera(float intensidade, float time)
    {
        CinemachineBasicMultiChannelPerlin cbm = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbm.m_AmplitudeGain = intensidade;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    public IEnumerator Shake(float duracao, float magnitude)
    {
        Vector3 originalPos = new Vector3(0,0,-40.98025f);
        float timePas = 0;

        while (timePas < duracao)
        {
            float xOffset = Random.Range(-.5f, .5f) * magnitude;
            float yOffset = Random.Range(-.5f, .5f) * magnitude;

            transform.localPosition = new Vector3(xOffset, yOffset, originalPos.z);
            timePas += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        shake = false;
    }
}

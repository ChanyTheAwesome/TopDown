using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlin;
    private float shakeTimeRemaining;
    bool isInit = false;
    private void Awake()
    {
        if (isInit == false) Init();
    }
    void Init()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        isInit = true;
    }

    public void ShakeCamera(float duration, float amplitude, float frequency)
    {
        if (isInit == false) Init();

        if (shakeTimeRemaining > duration)
            return;

        shakeTimeRemaining = duration;

        perlin.m_AmplitudeGain = amplitude;
        perlin.m_FrequencyGain = frequency;
    }

    private void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;
            if (shakeTimeRemaining <= 0)
            {
                StopShake();
            }
        }
    }

    public void StopShake()
    {
        shakeTimeRemaining = 0;
        perlin.m_AmplitudeGain = 0;
        perlin.m_FrequencyGain = 0;
    }
}
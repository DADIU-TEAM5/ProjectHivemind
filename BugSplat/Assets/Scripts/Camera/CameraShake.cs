using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCam;
    private CinemachineBasicMultiChannelPerlin _noise;
    
    // Start is called before the first frame update
    void Start()
    {
        if (VirtualCam != null)
        {
            _noise = VirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    public void AddCamNoise(CameraShakeArguments shakeArguments)
    {
        if (VirtualCam != null)
        {
            _noise.m_AmplitudeGain = shakeArguments.ShakeAmplitude;
            _noise.m_FrequencyGain = shakeArguments.ShakeFrequency;
        }

        StartCoroutine(RunCamNoise(shakeArguments.ShakeDuration));

    }

    private void StopCamNoise()
    {
        if (VirtualCam != null)
        {
            _noise.m_AmplitudeGain = 0f;
            _noise.m_FrequencyGain = 0f;
        }
    }

    private IEnumerator RunCamNoise(float shakeDuration)
    {
        while(true)
        {
            yield return new WaitForSeconds(shakeDuration);
            StopCamNoise();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public float ShakeDuration;
    public float ShakeAmplitude;
    public float ShakeFrequency;

    private float _shakeElapsedTime;

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

    public void AddCamNoise()
    {
        if (VirtualCam != null)
        {
            _noise.m_AmplitudeGain = ShakeAmplitude;
            _noise.m_FrequencyGain = ShakeFrequency;
        }

        StartCoroutine(RunCamNoise());

    }

    private void StopCamNoise()
    {
        if (VirtualCam != null)
        {
            _noise.m_AmplitudeGain = 0f;
            _noise.m_FrequencyGain = 0f;
        }
    }

    private IEnumerator RunCamNoise()
    {
        while(true)
        {
            yield return new WaitForSeconds(ShakeDuration);
            StopCamNoise();
        }
    }
}

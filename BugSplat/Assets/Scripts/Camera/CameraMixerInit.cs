using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMixerInit : MonoBehaviour
{

    public CinemachineMixingCamera MixerCam;
    public Transform PlayerGameObject;
    public Transform InitTarget;
    public Transform DestTarget;

    private float _initDist;
    private float _currentDist;
    private float _finalDist;

    private float _currentTime = 0f;

    public bool activeEnterCam = true;


    // Start is called before the first frame update
    void Start()
    {
        _initDist = Vector3.Distance(InitTarget.position, DestTarget.position);
        //Debug.Log("INITDIST: " + _initDist);

        MixerCam.m_Weight0 = 1;
        MixerCam.m_Weight1 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeEnterCam)
        {
            _currentDist = (Vector3.Distance(PlayerGameObject.position, DestTarget.position) * 0.9f) / _initDist;
        }
        else
        {
            _currentDist = 0;
        }


        //Debug.Log("CurrentDist: " + _currentDist);

        if (_currentDist < 1f)
        {
            if (_currentDist > 0.1f)
            {
                MixerCam.m_Weight0 = _currentDist;
                MixerCam.m_Weight1 = 1 - _currentDist;

                _finalDist = _currentDist;
            }
            else
            {
                for (_currentTime = 0; _currentTime < 1f; _currentTime += Time.deltaTime)
                {
                    MixerCam.m_Weight0 = Mathf.Lerp(_finalDist, 0, _currentTime);
                    MixerCam.m_Weight1 = Mathf.Lerp(1-_finalDist, 1, _currentTime);
                }
            }

        }
    }
}

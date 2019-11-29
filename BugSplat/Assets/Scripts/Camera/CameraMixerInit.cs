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
        _currentDist = Vector3.Distance(PlayerGameObject.position, DestTarget.position)/_initDist;

        //Debug.Log("CurrentDist: " + _currentDist);

        if (_currentDist < 1)
        {
            MixerCam.m_Weight0 = _currentDist;
            MixerCam.m_Weight1 = 1 - _currentDist;
        }
    }
}

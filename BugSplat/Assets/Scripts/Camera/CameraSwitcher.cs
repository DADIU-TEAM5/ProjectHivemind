using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine.Examples
{
    public class CameraSwitcher : MonoBehaviour
    {
        //public List<CinemachineVirtualCamera> VirtualCameras;
        //public GameObjectVariable CurrentEnemySO;
        //public FloatVariable PlayerCurrentSpeedSO;
        //public Vector3Variable PlayerDirectionSO;
        //private bool _activate = true;
        //private bool _deactivate = false;
        public CinemachineVirtualCamera ZoomCamera;
        public float WaitInitTime = 1f;
        public float SlowDownTimer = 1f;
        public float SlowDownRate = 0.2f;


        public void switchCamera(GameObject target)
        {

            ZoomCamera.m_Follow = target.transform;
            ZoomCamera.m_LookAt = target.transform;
            StartCoroutine(WaitInit(WaitInitTime));

        }

        private IEnumerator WaitInit(float waitTime)
        {
            yield return new WaitForSecondsRealtime(waitTime);

            StartCoroutine(SlowDownTime(ZoomCamera, SlowDownTimer, SlowDownRate));

            yield return null;

        }

        private IEnumerator SlowDownTime(CinemachineVirtualCamera camera, float slowDownSec, float slowDown)
        {
            Time.timeScale = slowDown;

            camera.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(slowDownSec);

            camera.gameObject.SetActive(false);

            Time.timeScale = 1f;

            yield return null;

        }



    }
}

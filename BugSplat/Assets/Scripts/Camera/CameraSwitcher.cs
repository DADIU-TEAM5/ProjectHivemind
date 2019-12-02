using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine.Examples
{
    public class CameraSwitcher : MonoBehaviour
    {
        //public List<CinemachineVirtualCamera> VirtualCameras;
        public GameObjectVariable CurrentEnemySO;
        public GameObject Targeter;
        //public FloatVariable PlayerCurrentSpeedSO;
        //public Vector3Variable PlayerDirectionSO;
        //private bool _activate = true;
        //private bool _deactivate = false;
        public CinemachineVirtualCamera ZoomCamera;
        public GameObject PlayerGameObject;
        public GameObject PlayerGraphics;
        public Vector3Variable PlayerDirectionSO;
        public FloatVariable PlayerCurrentSpeedSO;
        public BoolVariable PlayerControlOverrideSO;
        public float WaitInitTime = 1f;
        public float SlowDownTimer = 1f;
        public float SlowDownRate = 0.2f;


        public void switchCamera(GameObject target)
        {
            
            if (Targeter == null)
            {
                if (CurrentEnemySO == null)
                {
                    StartCoroutine(WaitInit(WaitInitTime, target));
                }
                else
                {
                    StartCoroutine(WaitInit(WaitInitTime, CurrentEnemySO.Value));
                }


            }
            else
            {
                if (CurrentEnemySO == null)
                {
                    Targeter.transform.position = target.transform.position;
                }
                else
                {
                    Targeter.transform.position = CurrentEnemySO.Value.transform.position;
                }

                StartCoroutine(WaitInit(WaitInitTime, Targeter));
            }



        }

        private IEnumerator WaitInit(float waitTime, GameObject target)
        {
            yield return new WaitForSeconds(waitTime);

            StartCoroutine(SlowDownTime(ZoomCamera, SlowDownTimer, SlowDownRate, target));

            yield return null;

        }

        private IEnumerator SlowDownTime(CinemachineVirtualCamera camera, float slowDownSec, float slowDown, GameObject target)
        {
            PlayerControlOverrideSO.Value = true;

            PlayerCurrentSpeedSO.Value = 0f;

            Vector3 newHeading = target.transform.position - PlayerGraphics.transform.position;

            PlayerDirectionSO.Value = new Vector3(newHeading.x, 0, newHeading.z);

            PlayerGraphics.transform.rotation = Quaternion.LookRotation(PlayerDirectionSO.Value, Vector3.up);

            if(CurrentEnemySO == null)
            {
                ZoomCamera.m_Follow = target.transform;
                ZoomCamera.m_LookAt = target.transform;
            }
            else
            {
                ZoomCamera.m_LookAt = target.transform;
            }


            Time.timeScale = slowDown;

            camera.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(slowDownSec);

            camera.gameObject.SetActive(false);

            Time.timeScale = 1f;

            PlayerControlOverrideSO.Value = false;

            yield return null;

        }



    }
}

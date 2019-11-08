using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine.Examples
{
    public class CameraSwitcher : MonoBehaviour
    {
        public List<CinemachineVirtualCamera> VirtualCameras;
        public Transform TankBeetle;
        public FloatVariable PlayerCurrentSpeedSO;
        public Vector3Variable PlayerDirectionSO;
        private bool _activate = true;
        private bool _deactivate = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void switchCamera(int cameraIndex)
        {
            VirtualCameras[cameraIndex].gameObject.SetActive(_activate);

            for (int i = 0; i < VirtualCameras.Count; i++)
            {
                if (i != cameraIndex)
                {
                    VirtualCameras[i].gameObject.SetActive(_deactivate);
                }
            }

            if (VirtualCameras[cameraIndex].name == "KillCam")
            {
                PlayerCurrentSpeedSO.Value = 0;
                PlayerDirectionSO.Value = Vector3.zero;
                Time.timeScale = 0.2f;
                VirtualCameras[cameraIndex].m_LookAt = TankBeetle;

                StartCoroutine(SlowDownTime(cameraIndex, 1f));

            }
        }

        private IEnumerator SlowDownTime(int cameraIndex, float WaitTime)
        {
            yield return new WaitForSeconds(WaitTime);

            //Destroy(TankBeetle.gameObject);
            Time.timeScale = 1f;
            VirtualCameras[0].gameObject.SetActive(_activate);
            VirtualCameras[cameraIndex].gameObject.SetActive(_deactivate);
        }
    }
}

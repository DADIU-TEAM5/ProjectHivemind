using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine.Examples
{
    public class CameraSwitcher : MonoBehaviour
    {
        public List<CinemachineVirtualCamera> VirtualCameras;
        //public GameObjectVariable CurrentEnemySO;
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

            /*if (VirtualCameras[cameraIndex].name == "KillCam")
            {
                VirtualCameras[cameraIndex].m_LookAt = CurrentEnemySO.Value.transform;
                Debug.Log(CurrentEnemySO.Value.name);
            }*/
        }
    }
}

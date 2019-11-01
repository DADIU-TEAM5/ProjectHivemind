using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public List<GameObject> VirtualCameras;
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

    public void switchCamera (int cameraIndex)
    {
        VirtualCameras[cameraIndex].SetActive(_activate);

        for (int i = 0; i < VirtualCameras.Count; i++)
        {
            if (i != cameraIndex)
            {
                VirtualCameras[i].SetActive(_deactivate);
            }
        }
    }
}

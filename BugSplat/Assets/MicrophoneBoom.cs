using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneBoom : MonoBehaviour
{

    public Transform micTarget;
    public Vector3 offset;


    void Update()
    {
        //Right now it doesn't work because it's getting the position of the prefab (which will always be 0,0,0)
        //It needs to get the gameobject in the scene
        gameObject.transform.position = micTarget.position + offset;
    }
}

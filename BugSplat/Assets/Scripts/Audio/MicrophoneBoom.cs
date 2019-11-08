using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneBoom : MonoBehaviour
{

    public Vector3Variable playerPosition;
    public Vector3 offset;


    void Update()
    {
        gameObject.transform.position = playerPosition.Value + offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneBoom : MonoBehaviour
{

    public Transform micTarget;
    public Vector3 offset;


    void Update()
    {
        //lige nu virker den ikke fordi den prøver at finde position på prefab, ikke den i scenen
        gameObject.transform.position = micTarget.position + offset;
    }
}

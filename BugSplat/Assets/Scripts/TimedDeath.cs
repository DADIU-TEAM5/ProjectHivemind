using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDeath : MonoBehaviour
{

    public float Time =2;
    private void OnEnable()
    {
        Destroy(gameObject, Time);
    }
}

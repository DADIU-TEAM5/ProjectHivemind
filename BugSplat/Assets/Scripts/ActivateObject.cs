using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{
    public GameObject[] Target;


    public void Activate(int index)
    {
        Target[index].SetActive(true);
    }

    public void Deactivate(int index)
    {
        Target[index].SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeParentObject : MonoBehaviour
{
    public GameObject[] ObjectChilds;

    public void DeParent(int index)
    {
        Vector3 currentWorldPos = ObjectChilds[index].transform.position;

        Debug.Log("PArent: " + ObjectChilds[index].transform.parent.position);
        Debug.Log("Child: " + ObjectChilds[index].transform.position);

        ObjectChilds[index].transform.position = currentWorldPos;

        ObjectChilds[index].transform.parent = null;
    }
}

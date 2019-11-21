using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeParentObject : MonoBehaviour
{
    public GameObject[] ObjectChilds;

    public void DeParent(int index)
    {
        ObjectChilds[index].transform.parent = null;
    }
}

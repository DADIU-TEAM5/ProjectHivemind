using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleEnabled : MonoBehaviour
{
    public GameObject Target;

    public void Toggle() {
        Target.SetActive(Target.activeInHierarchy ? false : true);
    }
}

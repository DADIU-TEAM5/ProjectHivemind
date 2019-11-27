using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadHideFood : MonoBehaviour
{

    public GameObject[] parts;
    public void ShowBugParts()
    {
        for (int pa = 0; pa < parts.Length; pa++)
        {
            parts[pa].SetActive(true);
        }
    }

    public void HideBugParts()
    {
        for (int pa = 0; pa < parts.Length; pa++)
        {
            parts[pa].SetActive(false);
        }
    }
}

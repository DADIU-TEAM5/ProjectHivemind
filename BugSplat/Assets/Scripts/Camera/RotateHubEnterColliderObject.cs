using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHubEnterColliderObject : MonoBehaviour
{
    public GameObject PlayerControlOverride;

    private void Start()
    {
            StartCoroutine(RotateCollider());
    }

    IEnumerator RotateCollider()
    {
        yield return new WaitForEndOfFrame();
        transform.rotation = GameObject.Find("middle").transform.rotation;
        if (PlayerControlOverride != null)
        {
            PlayerControlOverride.SetActive(true);
        }
        yield break;
    }
}

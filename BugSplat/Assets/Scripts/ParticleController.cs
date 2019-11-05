using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public GameObject AfterParts;

    public void MoveTo(GameObject gameObject) {
        this.transform.position = gameObject.transform.GetChild(0).position;
        this.transform.rotation = gameObject.transform.GetChild(0).rotation;
    }

    public void InstantiateAfterParts() {
        if (AfterParts == null) return;
        var gameO = Instantiate(AfterParts, this.transform.position, this.transform.rotation);
        gameO.SetActive(true);
    }
}

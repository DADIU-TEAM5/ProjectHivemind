using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public GameObject AfterParts;

    public void MoveTo(GameObject gameObject) {
        var actualGO = gameObject.transform.GetChild(0);
        this.transform.position = new Vector3(actualGO.position.x, 0, actualGO.position.z);
        this.transform.rotation = actualGO.rotation;
    }

    public void InstantiateAfterParts() {
        if (AfterParts == null) return;
        var gameO = Instantiate(AfterParts, this.transform.position, this.transform.rotation);
        gameO.SetActive(true);
    }
}

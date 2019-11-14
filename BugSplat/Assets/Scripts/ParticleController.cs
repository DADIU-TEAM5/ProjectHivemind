using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleController : MonoBehaviour
{
    public GameObject AfterParts;

    [SerializeField]
    private UnityEvent PlayEvent;

    public void MoveTo(GameObject gameObject) {
        var actualGO = gameObject.transform.GetChild(0);
        this.transform.position = new Vector3(actualGO.position.x, 0, actualGO.position.z);
        this.transform.rotation = actualGO.rotation;
    }

    public void Play() {
        PlayEvent?.Invoke();
    }

    public void InstantiateAfterParts() {
        if (AfterParts == null) return;
        var gameO = Instantiate(AfterParts, this.transform.position, this.transform.rotation);
        gameO.SetActive(true);
    }
}

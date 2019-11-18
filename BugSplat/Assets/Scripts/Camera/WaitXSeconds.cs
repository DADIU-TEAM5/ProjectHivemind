using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitXSeconds : MonoBehaviour
{
    public float SecondsToWait;
    public GameObject ScriptObjectToActivate;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitXSec(SecondsToWait, ScriptObjectToActivate));
    }

    // Update is called once per frame
    IEnumerator WaitXSec(float seconds, GameObject scriptObject)
    {
        yield return new WaitForSeconds(seconds);

        ScriptObjectToActivate.SetActive(true);
    }
}

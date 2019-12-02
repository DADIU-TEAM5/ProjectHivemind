using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRefRence : MonoBehaviour
{

    public Camera cam;
    float camSize = 20;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        cam.orthographicSize = camSize;

        while (Screen.height / cam.WorldToScreenPoint(transform.position).y > 1.10f) 
        {
            camSize -= 0.1f;

            cam.orthographicSize = camSize;
        }
        yield return null;
    }



}

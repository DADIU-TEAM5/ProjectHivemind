using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public int Rings = 1;

    GameObject hexagon;
    // Start is called before the first frame update
    void Start()
    {
        GameObject hex =  Instantiate(hexagon);

        hex.transform.position = Vector3.zero;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

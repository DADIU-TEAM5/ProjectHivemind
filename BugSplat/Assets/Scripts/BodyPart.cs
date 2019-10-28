using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public Rigidbody Body;

    public float enableTime = 1;
    float _time = 0;
    
    // Start is called before the first frame update
  
    private void OnEnable()
    {
        //print("BODY PART!");

        Body.AddForce(new Vector3(Random.Range(-15, 15), Random.Range(300,400), Random.Range(-15, 15)));
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if(_time >= enableTime)
        {

        }
    }

}

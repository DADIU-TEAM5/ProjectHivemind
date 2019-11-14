using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(BodyPart))]
[CanEditMultipleObjects]
public class BodypartEditor : Editor
{
    BodyPart part;

    private void OnEnable()
    {
        part = (BodyPart)target;

        part.Collider = part.gameObject.GetComponent<Collider>();
        part.Body = part.gameObject.GetComponent<Rigidbody>();


        
    }


}

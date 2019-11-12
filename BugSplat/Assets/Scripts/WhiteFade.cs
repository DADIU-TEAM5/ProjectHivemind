using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteFade : MonoBehaviour
{
    public Animator Anim;
    public bool In;
    
    // Start is called before the first frame update
    void Start()
    {
        Anim.SetBool("In",In);
    }
}

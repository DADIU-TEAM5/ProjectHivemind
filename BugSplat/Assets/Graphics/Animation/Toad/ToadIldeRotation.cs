using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadIldeRotation : MonoBehaviour
{

    Animator anim;


    private void OnEnable()
    {
        anim = GetComponent<Animator>();
    }
    public void NewIdle()
    {

        int idle = Random.Range(0, 3);

        print("playing ilde " + idle);
        anim.SetInteger("IdleToPlay", idle);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{

    public Spitter SpitterScript;

    public void Burrow()
    {
        //print("worm is under ground");
        SpitterScript.FullyUnderground = true;
    }


    public void Emerge()
    {
        //print("worm is over ground");
        SpitterScript.FullyUnderground = false;
    }
}

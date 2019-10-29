using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManagerScript : MonoBehaviour
{

    public AK.Wwise.Event ArenaAmbience;
    public AK.Wwise.Event HubAmbience;


    // Should play appropriate ambiences when entering the right areas

    void Start()
    {
        //Placeholder
        ArenaAmbience.Post(this.gameObject);
    }



}

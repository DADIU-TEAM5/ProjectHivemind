using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eggAudio : MonoBehaviour
{
    [Header("Wwise events")]
    public AK.Wwise.Event eggCrack;



    public void CrackTheEgg(GameObject source)
    {
        eggCrack.Post(source);
    }

}

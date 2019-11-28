using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaBigWallAudioPlayer : MonoBehaviour
{
    public AK.Wwise.Event WallCloseOpen;


    public void WallCloseOpenEvent(GameObject source)
    {
        WallCloseOpen.Post(source);
    }
}

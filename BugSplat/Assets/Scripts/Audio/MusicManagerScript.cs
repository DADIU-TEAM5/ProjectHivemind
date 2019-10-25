using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{

    public AK.Wwise.Event MainScore;

    // Start is called before the first frame update
    void Start()
    {
        MainScore.Post(this.gameObject);
    }

}

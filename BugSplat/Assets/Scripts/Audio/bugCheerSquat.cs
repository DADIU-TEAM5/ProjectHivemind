using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bugCheerSquat : MonoBehaviour
{
    [Header("Wwise events")]
    public AK.Wwise.Event WinningCheer;
    public AK.Wwise.Event LosingCheer;
    public AK.Wwise.Event ComboCheer;

    public void HasWon()
    {
        WinningCheer.Post(this.gameObject);
    }

    public void HasLost()
    {
        LosingCheer.Post(this.gameObject);
    }

    public void Combo()
    {
        ComboCheer.Post(this.gameObject);
    }
}

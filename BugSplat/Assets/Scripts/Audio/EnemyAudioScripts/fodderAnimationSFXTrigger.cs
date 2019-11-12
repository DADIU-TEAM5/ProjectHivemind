using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fodderAnimationSFXTrigger : MonoBehaviour
{
    public GameEvent footstep;

    public void takeStep()
    {
        footstep.Raise(this.gameObject);
    }

}

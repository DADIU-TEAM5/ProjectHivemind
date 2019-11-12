using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boomerAnimationSFXTrigger : MonoBehaviour
{
    public GameEvent footstep;

    public void takeStep()
    {
        footstep.Raise(this.gameObject);
    }


}


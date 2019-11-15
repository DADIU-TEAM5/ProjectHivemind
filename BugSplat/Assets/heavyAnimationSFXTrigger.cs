using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heavyAnimationSFXTrigger : MonoBehaviour
{
    public GameEvent footstep;
    public GameEvent footstepLight;

    public void takeStep()
    {
        footstep.Raise(this.gameObject);
    }

    public void takeStepLight()
    {
        footstepLight.Raise(this.gameObject);
    }
}

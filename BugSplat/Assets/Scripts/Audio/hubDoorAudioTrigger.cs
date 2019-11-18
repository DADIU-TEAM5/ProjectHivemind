using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hubDoorAudioTrigger : MonoBehaviour
{

    [Header("Game events")]
    public GameEvent doorRolling;
    public GameEvent doorStop;

    public void doorRollingAnim()
    {
        doorRolling.Raise(this.gameObject);
    }

    public void doorStopAnim()
    {
        doorStop.Raise(this.gameObject);
    }


}

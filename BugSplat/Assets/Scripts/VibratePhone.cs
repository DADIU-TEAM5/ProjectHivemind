using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibratePhone : MonoBehaviour
{


    bool _soShaltTheeVibrate;

   

    public void Vibrate(float Seconds)
    {
        if (Seconds > 0)
        {
            _soShaltTheeVibrate = true;
            StartCoroutine(VibrateThee());
            StartCoroutine(HaltThenStopThee(Seconds));
        }
        else
        Handheld.Vibrate();
    }

    IEnumerator VibrateThee()
    {
        while (_soShaltTheeVibrate)
        {

            Handheld.Vibrate();
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    IEnumerator HaltThenStopThee(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _soShaltTheeVibrate = false;
        yield return null;
    }


}

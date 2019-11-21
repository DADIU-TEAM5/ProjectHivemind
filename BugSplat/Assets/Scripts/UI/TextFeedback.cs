using System.Collections;
using System.Collections.Generic;
using BrunoMikoski.TextJuicer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextFeedback : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Title, Subtitle;

    public UnityEvent Play, Stop;

    public float TimeoutTime = 5;

    public void SetTitle(string title) {
        Title.text = title;
    }

    public void SetLevelTitle(IntVariable level) {
        Title.text = $"Level {level.Value}";
    }

    public void SetSubtitle(string subtitle) {
        Subtitle.text = subtitle;
    }

    public void SetFeedbackActive(bool active) {
        Play.Invoke();
        StartCoroutine(Timeout());
    }

    private IEnumerator Timeout() {
        yield return new WaitForSeconds(TimeoutTime);

        Stop.Invoke();
    }
}
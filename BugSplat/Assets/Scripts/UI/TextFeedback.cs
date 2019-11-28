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

    public void SetTitle(GameText title) {
        Title.text = title.GetText();
    }

    public void SetLevelTitle(IntVariable level) {
        Title.text = $"Level {level.Value + 1}";
    }

    public void SetSubtitle(GameText subtitle) {
        Subtitle.text = subtitle.GetText();
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
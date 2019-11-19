using System.Collections;
using System.Collections.Generic;
using BrunoMikoski.TextJuicer;
using UnityEngine;
using UnityEngine.UI;

public class TextFeedback : MonoBehaviour
{
    public JuicedText Title, Subtitle;

    public float TimeoutTime = 5;

    public void SetTitle(string title) {
        Title.TextComponent.text = title;
    }

    public void SetLevelTitle(IntVariable level) {
        Title.TextComponent.text = $"Level {level.Value}";
    }

    public void SetSubtitle(string subtitle) {
        Subtitle.TextComponent.text = subtitle;
    }

    public void SetFeedbackActive(bool active) {
        Title.gameObject.SetActive(active);
        Subtitle.gameObject.SetActive(active);

        StartCoroutine(Timeout(TimeoutTime));
    }

    private IEnumerator Timeout(float timeoutTime) {
        yield return new WaitForSeconds(timeoutTime);

        SetFeedbackActive(false);
    }
}
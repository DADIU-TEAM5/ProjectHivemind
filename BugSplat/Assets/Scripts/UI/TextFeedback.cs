using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TextFeedback : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Title, Subtitle;

    public UnityEvent Play, Stop;

    public float TimeoutTime = 5;

    public void SetTitle(GameText title) {
        Title.text = title.GetText();
    }

    public void SetLevelTitle(LevelText levelText) {
        Title.text = levelText.Text.GetText() + (levelText.Level.Value + 1);
    }

    public void SetSubtitle(GameText subtitle) {
        Subtitle.text = subtitle.GetText();
    }

    public void SetFeedbackActive() {
        Play.Invoke();
        StartCoroutine(Timeout());
    }

    private IEnumerator Timeout() {
        yield return new WaitForSeconds(TimeoutTime);

        Stop.Invoke();
    }
}
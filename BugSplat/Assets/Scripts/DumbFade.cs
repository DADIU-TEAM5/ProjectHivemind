using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DumbFade : MonoBehaviour
{
    public float FadeTime;

    public Image FadeImage;

    public AnimationCurve Curve;

    public IEnumerator Start() {
        var currentColor = FadeImage.color;
        var endColor = currentColor - new Color(0, 0, 0, 1);

        for (var time = 0f; time < FadeTime; time += Time.deltaTime) {
            var animTime = Curve.Evaluate(time / FadeTime);

            FadeImage.color = Color.Lerp(currentColor, endColor, animTime);

            yield return null;
        }
    }
}

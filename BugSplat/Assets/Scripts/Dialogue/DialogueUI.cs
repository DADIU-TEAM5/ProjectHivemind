using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class DialogueUI : MonoBehaviour
{
    public DialoguePool Pool;

    public TMPro.TextMeshProUGUI Text;

    private GameText _dialogue;

    public void GetDialogue() {
        _dialogue = Pool.GetDialogue();
    }

    public void DisplayDialogue() {
        StopAllCoroutines();
        Text.text = _dialogue?.GetText() ?? "";

        Destroy(gameObject, 5);
    }

    public void AnimatedDisplayDialogue() {
       StopAllCoroutines();
       StartCoroutine(LetterAtATime());
    }

    private IEnumerator LetterAtATime() {
        var dialogueText = _dialogue?.GetText();
        if (dialogueText == null) yield break;

        var builder = new StringBuilder();

        foreach(var character in dialogueText) {
            builder.Append(character);

            Text.text = builder.ToString();

            var waitSeconds = Random.Range(Time.deltaTime * 4, Time.deltaTime * 8);
            yield return new WaitForSeconds(waitSeconds);
        }

        Destroy(gameObject, 3);
    }
}

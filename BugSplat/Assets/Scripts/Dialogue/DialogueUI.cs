using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour 
{
    public DialoguePool Pool;

    public TMPro.TextMeshProUGUI Text;

    private GameText _dialogue;

    void Start() {
        var image = GetComponent<Image>();
    }

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

            var waitSeconds = Random.Range(Time.deltaTime, Time.deltaTime * 3);
            yield return new WaitForSeconds(waitSeconds);
        }

        Destroy(gameObject, 3);
    }

    private IEnumerator WordAtATime() {
        var dialogueText = _dialogue?.GetText();
        if (dialogueText == null) yield break;

        var builder = new StringBuilder();

        var words = dialogueText.Split(null);

        foreach(var word in words) {
            builder.Append(word);
            builder.Append(' ');

            Text.text = builder.ToString();

            var waitSeconds = Random.Range(Time.deltaTime * 4, Time.deltaTime * 15);
            yield return new WaitForSeconds(waitSeconds);
        }

        Destroy(gameObject, 3);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueInvoker : MonoBehaviour
{
    public GameObject SmallDialogue, MediumDialogue, LargeDialogue;
    public int CharacterThresholdMedium, CharacterThresholdLarge;

    public Transform DialogueParent;

    public DialoguePool Pool;

    private GameText _dialogue;

    private Text _smallDialogueBox, _mediumDialogueBox, _largeDialogueBox;

    void Start() {
        _smallDialogueBox = SmallDialogue.GetComponentInChildren<Text>();
        _mediumDialogueBox = MediumDialogue.GetComponentInChildren<Text>();
        _largeDialogueBox = LargeDialogue.GetComponentInChildren<Text>();
    }
    
    public void GetDialogue() {
        _dialogue = Pool.GetDialogue();

        var sentence = _dialogue.GetText();
        _smallDialogueBox.text = sentence;
        _mediumDialogueBox.text = sentence;
        _largeDialogueBox.text = sentence;
    }

    public void InvokeDialogue() {
        var sentence = _dialogue.GetText();

        var sentenceLength = sentence.Length;
        Debug.Log($"Sentence length: {sentenceLength}");

        GameObject dialogueBox;
        if (sentenceLength < CharacterThresholdMedium) {
            // Instantiate small box
            dialogueBox = Instantiate(SmallDialogue, DialogueParent);
        } else if (sentenceLength < CharacterThresholdLarge){
            // Instantiate medium box
            dialogueBox = Instantiate(MediumDialogue, DialogueParent);
        } else {
            // Instantiate large box
            dialogueBox = Instantiate(LargeDialogue, DialogueParent);
        }

    }
}
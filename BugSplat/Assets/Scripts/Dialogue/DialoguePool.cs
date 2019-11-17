using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class DialoguePool : ScriptableObject {
    public GameText[] Dialogues;

    private bool[] Seen;

    public GameText GetDialogue() {
        if (Dialogues == null || Dialogues.Length == 0) {
          Debug.LogError("No dialogues");
          return null;
        }


        // Make a list of the indexes of the dialogues that hasn't been seen yet.
        var indexes = new List<int>();

        for (var i = 0; i < Dialogues.Length; i++) {
            if (!Seen[i]) {
                indexes.Add(i);
            }
        }

        if (indexes.Count == 0) {
            Debug.LogError("No more unique dialogues in the DialoguePool");
            return null;
        }

        // Choose at random
        var selectedIndex = indexes[Random.Range(0, indexes.Count)];
        var selectedDialogue = Dialogues[selectedIndex];
        Seen[selectedIndex] = true;

        return selectedDialogue;
    }

    void OnEnable() {
       Seen = new bool[Dialogues.Length];

    }
}

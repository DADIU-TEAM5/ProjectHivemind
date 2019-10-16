using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTextUpdater : MonoBehaviour
{
    public GameText GameText;
    public Text TextObject;

    public void Start() {
        UpdateText();
    }

    public void UpdateText() {
        TextObject.text = GameText.GetText();
    }

    private void OnEnable()
    {
        UpdateText();
    }
}
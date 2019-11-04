using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTextMeshProUpdater : MonoBehaviour
{
    public GameText GameText;
    public TMPro.TextMeshProUGUI TextObject;

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

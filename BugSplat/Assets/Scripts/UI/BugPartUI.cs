using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BugPartUI : GameLoop
{
    public IntVariable BugParts;

    public TMPro.TextMeshProUGUI TextToChange;

    public override void LoopLateUpdate(float deltaTime) {}
    
    public override void LoopUpdate(float deltaTime)
    {
        TextToChange.text = BugParts.Value.ToString();
    }
}

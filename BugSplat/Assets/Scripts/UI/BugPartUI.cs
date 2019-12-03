using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BugPartUI : GameLoop
{
    public IntVariable BugParts;

    public TMPro.TextMeshProUGUI TextToChange;


    private void Start()
    {
        //BugParts.Value = 0;
        //_startText = TextToChange.text;
    }

    public override void LoopLateUpdate(float deltaTime) {}
    
    public override void LoopUpdate(float deltaTime)
    {
        TextToChange.text = "" + BugParts.Value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bodyPartText : GameLoop
{
    

    public IntVariable BodyParts;
    public Text TextToChange;
    string _startText;

    private void Start()
    {
        BodyParts.Value = 0;
        _startText = TextToChange.text;
    }

    public override void LoopLateUpdate(float deltaTime)
    {
        

    }
    public override void LoopUpdate(float deltaTime)
    {
        TextToChange.text = _startText + " " + BodyParts.Value;

    }
}

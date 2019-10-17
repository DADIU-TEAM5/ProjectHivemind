using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : GameLoop
{
    private float _fps = 0f;

    private GUIStyle _overlayStyle = new GUIStyle();

    // Start is called before the first frame update
    void Start()
    {
        _overlayStyle.fontSize = 40;        
        _overlayStyle.normal.textColor = Color.yellow;
    }

    void OnGUI()
    {
        //GUILayout.Label($"FPS: ");
        GUI.Label(new Rect(40, 40, 200, 80), $"FPS: {_fps}", _overlayStyle);
    }

    public override void LoopUpdate(float deltaTime)
    {
        _fps = 1f / Time.deltaTime;
    }

    public override void LoopLateUpdate(float deltaTime)
    {
    }
}

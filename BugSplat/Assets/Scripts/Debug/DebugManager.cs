using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : GameLoop
{
    public int FPSUpdateRate = 60;
    private float _fps = 0f;
    private float _averageFPS = 0f;
    private float[] _fpsArray;
    private int _index = 0;

    private GUIStyle _overlayStyle = new GUIStyle();

    // Start is called before the first frame update
    void Start()
    {
        _fps = 1f / Time.deltaTime;
        _fpsArray = new float[FPSUpdateRate];

        for (int i = 0; i < _fpsArray.Length; i++)
        {
            _fpsArray[i] = _fps;
        }
        _averageFPS = _fps;


        _overlayStyle.fontSize = 40;        
        _overlayStyle.normal.textColor = Color.yellow;
    }

    void OnGUI()
    {
        //GUILayout.Label($"FPS: ");
        GUI.Label(new Rect(40, 180, 200, 80), $"FPS: {_averageFPS}", _overlayStyle);
    }

    public override void LoopUpdate(float deltaTime)
    {

        _fps = 1f / Time.deltaTime;
        _fpsArray[_index] = _fps;

        _index++;

        if (_index==FPSUpdateRate)
        {
            _index = 0;
            float temp = 0f;

            for (int i = 0; i < FPSUpdateRate; i++)
            {
                temp += _fpsArray[i];
            }

            _averageFPS = (int)(temp / FPSUpdateRate);

        }
    }

    public override void LoopLateUpdate(float deltaTime)
    {
    }
}

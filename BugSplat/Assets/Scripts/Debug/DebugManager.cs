using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{

    private GUIStyle _overlayStyle = new GUIStyle();

    // Start is called before the first frame update
    void Start()
    {
        _overlayStyle.fontSize = 40;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        //GUILayout.Label($"FPS: ");
        GUI.Label(new Rect(40, 40, 200, 80), $"FPS: {1f / Time.deltaTime}", _overlayStyle);
    }
}

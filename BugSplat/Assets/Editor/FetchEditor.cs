using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ClipsFetch))]
public class FetchEditor : Editor
{
    private ClipsFetch _clipsFetch;

    void OnEnable()
    {
        _clipsFetch = (ClipsFetch)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _clipsFetch.value = EditorGUILayout.Slider("start frame", _clipsFetch.value, 0f, 0.9999f);
        _clipsFetch.GetFrame();

        if (GUILayout.Button("Cut Animation"))
        {
            _clipsFetch.CutAnimation();
        }

        if (GUILayout.Button("Test"))
        {
            for(int i = 0; i < 100; i++)
            {
                _clipsFetch.value = i * 0.001f;
                _clipsFetch.GetFrame();
            }
        }
    }
}

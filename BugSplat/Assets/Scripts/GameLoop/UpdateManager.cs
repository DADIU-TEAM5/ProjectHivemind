using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public GameLoopInvoker[] GameLoopHandlers;

    public bool CappedFPS = true;
    private float _time;


    private void OnEnable()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }


    void Start() {
        if (CappedFPS) Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        for (var i = 0; i < GameLoopHandlers.Length; i++) {
            var handler = GameLoopHandlers[i];
            handler?.UpdateGameLoop(_time);
        }        
    }

    void LateUpdate() {
        for (var i = 0; i < GameLoopHandlers.Length; i++) {
            var handler = GameLoopHandlers[i];
            handler?.LateUpdateGameLoop(_time);
        }        
    }
}

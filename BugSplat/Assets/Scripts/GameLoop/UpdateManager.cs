using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public GameLoopInvoker[] GameLoopHandlers;

    private float _time;

    void Start() {
        Application.targetFrameRate = 60;
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

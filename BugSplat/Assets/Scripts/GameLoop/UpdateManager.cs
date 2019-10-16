using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public GameLoopInvoker[] GameLoopHandlers;

    private float _time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        for (var i = 0; i < GameLoopHandlers.Length; i++) {
            var handler = GameLoopHandlers[i];
            if (handler.UpdateGameLoop(_time)) {
                handler.LateUpdateGameLoop();
            }
        }        
    }
}

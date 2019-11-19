using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameLoopParticipant : GameLoop
{
    public GameLoopInvoker GameLoopHandler;

    public GameLoop[] GameLoops;

    private void Awake() {
        this.IsPaticipant = true;

        var gameLoops = this.gameObject.GetComponents<GameLoop>();
        var filteredGameLoops = gameLoops.Where(x => x != null && x.enabled && !x.IsPaticipant).ToArray();

        GameLoops = filteredGameLoops;
    }

    private void OnEnable()
    {
        GameLoopHandler.RegisterGameLoop(this);
    }

    private void OnDisable()
    {
        GameLoopHandler.UnregisterGameLoop(this);
    }

    public override void LoopUpdate(float deltaTime)
    {
        for (var i = 0; i < GameLoops.Length; i++) {
            GameLoops[i].LoopUpdate(deltaTime);
        }
    }

    public override void LoopLateUpdate(float deltaTime)
    {
        for (var i = 0; i < GameLoops.Length; i++) {
            GameLoops[i].LoopLateUpdate(deltaTime);
        }
    }

   

}

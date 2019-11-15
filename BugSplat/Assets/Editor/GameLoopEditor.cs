using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;



[CustomEditor(typeof(GameLoopParticipant))]
[CanEditMultipleObjects]
public class GameLoopEditor : Editor
{

    GameLoopParticipant loopDeLoop;


    List<GameLoop> GameLoops;

    GameLoop[] gameLoopArray;


    private void OnEnable()
    {
        loopDeLoop = (GameLoopParticipant)target;
        loopDeLoop.IsPaticipant = true;

        GameLoops = new List<GameLoop>();

        gameLoopArray = loopDeLoop.gameObject.GetComponents<GameLoop>();


        for (int i = 0; i < gameLoopArray.Length; i++)
        {

            if(gameLoopArray[i].isActiveAndEnabled && !gameLoopArray[i].IsPaticipant)
            {
                GameLoops.Add(gameLoopArray[i]);
            }

        }

        gameLoopArray = GameLoops.ToArray();

        loopDeLoop.GameLoops = gameLoopArray;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(GameLoopParticipant))]
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

            if(!gameLoopArray[i].IsPaticipant)
            {
                GameLoops.Add(gameLoopArray[i]);
            }

        }

        gameLoopArray = new GameLoop[GameLoops.Count];

        for (int i = 0; i < GameLoops.Count; i++)
        {
            gameLoopArray[i] = GameLoops[i];

        }


        loopDeLoop.GameLoops = gameLoopArray;


    }

    /*
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();


    }
    */

}

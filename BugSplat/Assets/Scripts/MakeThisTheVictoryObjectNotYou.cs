using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeThisTheVictoryObjectNotYou : GameLoop
{

    public GameObjectVariable victoryObj;



    public override void LoopLateUpdate(float deltaTime)
    {
        

    }

    public override void LoopUpdate(float deltaTime)
    {

        if(victoryObj.Value != gameObject)
        {
            victoryObj.Value = gameObject;
        }
        
    }
}

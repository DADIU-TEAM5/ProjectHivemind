using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMakeNewGamePls : MonoBehaviour
{

    public IntVariable CurrentLevel;

    public SaveLoadEnforcer saveLoadEnf;


    void Start()
    {
        if(CurrentLevel.Value >= CurrentLevel.Max)
        {
            saveLoadEnf.NewGame();
            saveLoadEnf.Save();
        }
    }

   
}

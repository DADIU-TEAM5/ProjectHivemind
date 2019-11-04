using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Ability : ScriptableObject
{
    public List<GameEvent> Triggers;
    public string AbilityName;
    public string Dev_Description;
    public GameEventListener EventListener;

    // Sound Stuff

    // Visual Stuff

    public abstract void OnTrigger(GameObject GameObj);
    public abstract void Initialize(GameObject GameObj);

    //public void InitializeGameEventLListeners()
    //{
    //    for (int i = 0; i < Triggers.Count; i++)
    //    {
    //        Triggers[i]
    //    }
    //}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Variables/Lists/GameObjectList")]
public class GameObjectList : RuntimeSet<GameObject>
{
    public GameEvent HasWon;
    public GameEvent EnemyDied;

    private void OnEnable()
    {
        Items = new List<GameObject>();
    }
    public override void Remove(GameObject t)
    {
        
            if (Items.Contains(t))
            {
                EnemyDied.Raise(t);
                Items.Remove(t);
            }
        if(Items.Count <=0)
        {
            HasWon.Raise();
        }
    }

}

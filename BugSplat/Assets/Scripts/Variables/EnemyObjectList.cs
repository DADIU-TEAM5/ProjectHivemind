using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Variables/Lists/GameObjectList")]
public class EnemyObjectList : RuntimeSet<Enemy>
{
    public GameEvent HasWon;
    public GameEvent EnemySpawned;
    

    private void OnEnable()
    {
        Items = new List<Enemy>();
    }
    public override void Remove(Enemy t)
    {
        
        if (Items.Contains(t))
        {
            
            Items.Remove(t);
        }

        if(Items.Count <= 0)
        {
            HasWon.Raise();
        }
    }

    public override void Add(Enemy t)
    {
        if (!Items.Contains(t))
        {
            Items.Add(t);
            EnemySpawned?.Raise();
        }
    }
}

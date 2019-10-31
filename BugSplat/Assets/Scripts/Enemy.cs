using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : GameLoop
{
    public GameObjectList EnemyList;
    public BoolVariable NoVisibleEnemies;

    private void OnEnable()
    {
        Debug.Log(name + " spawned");
        EnemyList.Add(gameObject);
    }


    public abstract void TakeDamage(float damage);
    

    
    
}

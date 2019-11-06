using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "EnemySpawner/Check List")]
public class EnemyCheckOff : ScriptableObject

{

    


    public EnemyList Enemies;

    [HideInInspector]
    public bool[] Checks;




    
}

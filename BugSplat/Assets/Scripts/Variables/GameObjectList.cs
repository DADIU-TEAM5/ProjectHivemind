using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Variables/Lists/GameObjectList")]
public class GameObjectList : RuntimeSet<GameObject>
{

    private void OnEnable()
    {
        Items = new List<GameObject>();
    }

}

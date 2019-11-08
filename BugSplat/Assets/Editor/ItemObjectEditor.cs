using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ItemObject))]
[CanEditMultipleObjects]
public class ItemObjectEditor : Editor
{

    ItemObject _itemO;
    bool playerStatsIsOpen = true;

    private void OnEnable()
    {
        _itemO = (ItemObject)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        playerStatsIsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(playerStatsIsOpen, "Flat Stat Changes");

        if (playerStatsIsOpen)
        {
            if (_itemO.FlatStatChanges == null || _itemO.FlatStatChanges.Length == 0)
                _itemO.FlatStatChanges = new float[_itemO.PlayerStats.Value.Count];


            for (int i = 0; i < _itemO.FlatStatChanges.Length; i++)
            {
                _itemO.FlatStatChanges[i] = EditorGUILayout.FloatField(_itemO.PlayerStats.Value[i].name, _itemO.FlatStatChanges[i]);
            }
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
    }

}

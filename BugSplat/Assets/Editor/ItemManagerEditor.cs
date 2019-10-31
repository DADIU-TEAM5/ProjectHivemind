using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemManager))]
[CanEditMultipleObjects]
public class ItemManagerEditor : Editor
{

    ItemManager itemManager;
    bool testWindowIsOpen = false;
    bool playerStatsIsOpen = false;

    private void OnEnable()
    {
        itemManager = (ItemManager)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        playerStatsIsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(playerStatsIsOpen, "Player Stats");
        if (playerStatsIsOpen)
        {
            for (int i = 0; i < itemManager.ModifiedStats.Count; i++)
            {
                itemManager.ModifiedStats[i].Value = EditorGUILayout.FloatField(itemManager.ModifiedStats[i].name, itemManager.ModifiedStats[i].Value);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();
        testWindowIsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(testWindowIsOpen, "Item Tests");

        if (testWindowIsOpen)
        {
            ItemObject testItem = itemManager.AllItems[0];

            if (GUILayout.Button("Add Item"))
            {
                Debug.Log("Clicked Add Item");
                itemManager.AddItem(testItem);
            }

            if (GUILayout.Button("Reset Items"))
            {
                Debug.Log("Clicked Reset");
                itemManager.ResetItems();
            }

        }
        EditorGUILayout.EndFoldoutHeaderGroup();


    }
}


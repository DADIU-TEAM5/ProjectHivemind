using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemManager))]
[CanEditMultipleObjects]
public class ItemManagerEditor : Editor
{

    ItemManager itemManager;
    bool _testWindowIsOpen = true;
    bool _playerStatsIsOpen = true;
    bool _inventoryIsOpen = true;

    private void OnEnable()
    {
        itemManager = (ItemManager)target;
    }

    public override void OnInspectorGUI()
    {

        if (_testWindowIsOpen)
        {
            ItemObject testItem = itemManager.AllItems[0];

            if (GUILayout.Button("Add Item"))
            {
                Debug.Log("Clicked Add Item");
                itemManager.PlayerInventory.AddItem(testItem);
            }

            if (GUILayout.Button("Reset Items"))
            {
                Debug.Log("Clicked Reset");
                itemManager.PlayerInventory.ResetItems();
            }

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        base.OnInspectorGUI();


        EditorGUILayout.Space();

        _playerStatsIsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(_playerStatsIsOpen, "Player Stats");
        if (itemManager.PlayerStats.Value.Count > 0 && _playerStatsIsOpen)
        {

            for (int i = 0; i < itemManager.PlayerStats.Value.Count; i++)
            {
                itemManager.PlayerStats.Value[i].Value = EditorGUILayout.FloatField(itemManager.PlayerStats.Value[i].name, itemManager.PlayerStats.Value[i].Value);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();

        _inventoryIsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(_inventoryIsOpen, "Inventory");
        if (itemManager.PlayerInventory.Items.Count > 0 && _inventoryIsOpen)
        {

            for (int i = 0; i < itemManager.PlayerInventory.Items.Count; i++)
            {
                EditorGUILayout.TextField(itemManager.PlayerInventory.Items[i].name);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space();

        _testWindowIsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(_testWindowIsOpen, "Item Tests");




    }
}


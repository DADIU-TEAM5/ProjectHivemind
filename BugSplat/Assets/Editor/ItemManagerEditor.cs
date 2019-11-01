using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemManager))]
[CanEditMultipleObjects]
public class ItemManagerEditor : Editor
{

    ItemManager itemManager;
    bool _shopWindowIsOpen = true;
    bool _playerStatsIsOpen = true;
    bool _inventoryIsOpen = true;

    private void OnEnable()
    {
        itemManager = (ItemManager)target;
    }

    public override void OnInspectorGUI()
    {
        ItemObject testItem = null;

        if (itemManager.AllItems != null && itemManager.AllItems.Count > 0)
            testItem = itemManager.AllItems[0];

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



        EditorGUILayout.Space();
        if (itemManager.BodyParts != null)
            itemManager.BodyParts.Value = EditorGUILayout.IntField("Body Parts", itemManager.BodyParts.Value);

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

        //_shopWindowIsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(_shopWindowIsOpen, "Shop Slots");
        //if (itemManager.PlayerInventory.Items.Count > 0 && _shopWindowIsOpen)
        //{

        //    for (int i = 0; i < itemManager.PlayerInventory.Items.Count; i++)
        //    {
        //        EditorGUILayout.TextField(itemManager.PlayerInventory.Items[i].name);
        //    }
        //}
        //EditorGUILayout.EndFoldoutHeaderGroup();




    }
}


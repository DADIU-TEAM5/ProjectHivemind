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

    private void OnEnable()
    {
        itemManager = (ItemManager)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        testWindowIsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(testWindowIsOpen, "Item Tests");

        if (testWindowIsOpen)
        {
            ItemObject testItem = new ItemObject();
            
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


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Item")]

public class ItemObject : ScriptableObject
{
    public string DevelopmentNotes;
    public FloatVariableList PlayerStats;

    [HideInInspector]
    public float[] FlatStatChanges;

    [Header("Displayed In Game")]
    public Tier Tier;
    public int Price;
    public string Description;
    public Sprite Icon;


    [Header("Tags")]
    public bool IsStackable;

    [Header("Abilities")]
    public List<Ability> abilities;

    public void OnEnable()
    {
        if(FlatStatChanges==null && PlayerStats != null)
            FlatStatChanges = new float[PlayerStats.Value.Count];
    }

}

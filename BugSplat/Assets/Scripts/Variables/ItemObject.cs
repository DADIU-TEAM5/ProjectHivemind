using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Item")]

public class ItemObject : ScriptableObject
{
    public string DevelopmentNotes;
    
    [Header("Displayed In Game")]
    public string Name;
    public int Tier;
    public int Price;
    public string Description;


    //[Header("Triggers used")]
    //public bool Attack_OnHit;
    //public bool Attack_OnAttack;
    //public bool Attack_OnHit_Debuff;

    // Add the rest

    [Header("Flat Stat Changes")]
    public int Flat_MovementSpeed;
    public int Flat_AttackSpeed;
    public int Flat_AttackDamage;
    public int Flat_DashSpeed;
    public int Flat_Dash_Length;
    public int Flat_Health;
    public int Flat_Damage_Reduction;

    [Header("Percentage Stat Changes")]

    public float Percentage_MovementSpeed;
    public float Percentage_AttackSpeed;
    public float Percentage_AttackDamage;
    public float Percentage_DashSpeed;
    public float Percentage_Dash_Length;
    public float Percentage_Health;
    public float Percentage_Damage_Reduction;
}

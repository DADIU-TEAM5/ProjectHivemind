using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class ItemInfo 
{
    [Required]
    public Tier Tier;

    [Min(0)]
    public int Price;

    [Required]
    public GameText Title;
    [Required]
    public GameText Description;

    [Required]
    public GameObject ItemPrefab;
}
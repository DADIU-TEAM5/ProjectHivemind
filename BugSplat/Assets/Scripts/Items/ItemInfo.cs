using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInfo 
{
    public Tier Tier;
    public int Price;

    public GameText Title;
    public GameText Description;
    public GameObject ItemPrefab;

}
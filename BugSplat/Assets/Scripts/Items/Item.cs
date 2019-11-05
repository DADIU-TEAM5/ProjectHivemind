using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Item/New Item")]
public class Item : ScriptableObject
{
    public ItemInfo Info;
    public Effect[] Effects;
}

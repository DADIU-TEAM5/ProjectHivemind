using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName="Item/New Item")]
public class Item : ScriptableObject
{
    public ItemInfo Info;
    public Effect[] Effects;

    public bool Purchasable() => Effects.Sum(x => x.CanBeApplied()) > 0;
}

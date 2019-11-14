using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName="Shop/AbilitySlot")]
public class ShopAbilitySlot : ShopSlot
{
    public Effect Ability;

    public ItemInfo Info;

    public override GameObject GetItemPrefab() => Info?.ItemPrefab;

    public override int GetPrice() => Info?.Price ?? 0;

    public override bool OnPurchase()
    {
        Ability?.Trigger();
        return true;
    }

    public override void Init()
    {
    }

    public override void Reset()
    {
    }

    public override string GetTitle() => Ability?.name;

    public override string GetDescription() => Info?.Description;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Shop/AbilitySlot")]
public class ShopAbilitySlot : ShopSlot
{
    public Ability Ability;

    public int Price;

    public override void OnPurchase()
    {
        // Subtract price from player total

        Ability.OnTrigger();
    }
}

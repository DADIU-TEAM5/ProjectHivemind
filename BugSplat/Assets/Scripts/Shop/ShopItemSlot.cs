using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName="Shop/ItemSlot")]
public class ShopItemSlot : ShopSlot
{
    public ItemPool Pool;
    public ShopLevels ShopLevels;
    public IntVariable CurrentLevel;

    [SerializeField]
    private ItemObject Item;

   
    // TODO: NEED REFERENCE TO PLAYER ITEMS INVENTORY THING

    public override void OnPurchase()
    {
        if (Item == null) return;

        // Add item to player's inventory

        // Remove item    
        Item = null;
    }

    public void GetItemFromItemPool() {
        if (Item != null) {
            return;
        } 

        var shopLevel = ShopLevels.LevelTierPicker[CurrentLevel.Value];
        var decidedTier = shopLevel.ChooseTier();
        var decidedItem = Pool.GetItem(decidedTier);

        Item = decidedItem;
    }

    public ItemObject GetItem() => Item;

    void OnDisable() {
        if (Item != null) {
            Reset();
        }
    }

    public override Sprite GetSprite() => Item.Icon;

    public override int GetPrice() => Item.Price;

    public override void Init()
    {
        GetItemFromItemPool();
    }

    public override void Reset()
    {
        Pool.ReplenishOnce(Item);
        Item = null;
    }
}
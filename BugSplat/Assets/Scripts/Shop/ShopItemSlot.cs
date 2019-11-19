using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Shop/ItemSlot")]
public class ShopItemSlot : ShopSlot
{
    public ItemPool Pool;
    public ShopLevels ShopLevels;
    public IntVariable CurrentLevel;
    public Inventory PlayerInventory;

    [SerializeField]
    private Item Item;

    [SerializeField]
    private int _previousLevel = -1;


    public override bool OnPurchase()
    {
        if (Item == null) return false;

        if(PlayerInventory == null)
        {
            Debug.LogError("No Refference to player inventory!");
            return false;
        }

        if (!Item.Purchasable()) {
            Debug.Log("Item is not purchasable, as none of its effects can be applied");
            return false;
        }

        PlayerInventory.AddItem(Item);

        Item = null;

        return true;
    }

    public void GetItemFromItemPool()
    {
        if (Item != null)
        {
            return;
        }

        var shopLevel = ShopLevels.LevelTierPicker[CurrentLevel.Value];
        var decidedTier = shopLevel.ChooseTier();
        var decidedItem = Pool.GetItem(decidedTier);

        Item = decidedItem;
    }

    public Item GetItem() => Item;

    void OnDisable()
    {
        if (Item != null)
        {
            Reset();
        }
    }

    public override GameObject GetItemPrefab() => Item?.Info?.ItemPrefab;

    public override int GetPrice() => Item?.Info?.Price ?? 0;

    public override void Init()
    {
        if (_previousLevel != CurrentLevel.Value) {
            if (Item != null) Reset();
            GetItemFromItemPool();
        }

        _previousLevel = CurrentLevel.Value;
    }

    public override void Reset()
    {
        Pool.ReplenishOnce(Item);
        Item = null;
    }

    public override string GetTitle() => Item.name;

    public override string GetDescription() => Item?.Info?.Description;
}
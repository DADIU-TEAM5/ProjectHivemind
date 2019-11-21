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
        if (Item != null) return;

        var shopLevel = ShopLevels.LevelTierPicker[CurrentLevel.Value];
        var firstTier = shopLevel.ChooseTier();
        var tier = firstTier;
        Item decidedItem = null;
        do
        {
            decidedItem = Pool.GetItem(tier);
            if (decidedItem == null) {
                tier = shopLevel.NextTier();
            } else break;
        } while (firstTier != tier);

        Item = decidedItem;
    }

    public Item GetItem() => Item;

    public override GameObject GetItemPrefab() => Item?.Info?.ItemPrefab;

    public override int GetPrice() => Item?.Info?.Price ?? 0;

    public override void Init()
    {
        Debug.Log($"Previous level: {_previousLevel}, Current: {CurrentLevel.Value}");
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

    public override void Reroll()
    {
        if (Item != null) {
            var temp = Item;
            GetItemFromItemPool();
            Pool.ReplenishOnce(temp);
        } else {
            if (!ConsumeReroll) {
                GetItemFromItemPool();
            }
        }
    }
}
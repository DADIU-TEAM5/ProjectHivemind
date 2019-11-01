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
    private ItemObject Item;


    public override void OnPurchase()
    {
        if (Item == null) return;
        if(PlayerInventory== null)
        {
            Debug.LogError("No Refference to player inventory!");
        }

        if (PlayerInventory.AddItem(Item))
            // Remove item   
            Item = null;
        else
        {
            Debug.LogError("Player did not add item from shop!");
        }
         

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

    public ItemObject GetItem() => Item;

    void OnDisable()
    {
        if (Item != null)
        {
            Reset();
        }
    }

    public override Sprite GetSprite() => Item?.Icon;

    public override int GetPrice() => Item?.Price ?? 0;

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
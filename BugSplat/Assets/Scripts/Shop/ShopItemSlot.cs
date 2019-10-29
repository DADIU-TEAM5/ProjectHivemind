using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Shop/ItemSlot")]
public class ShopItemSlot : ShopSlot
{
    public ItemPool Pool;
    public ShopLevels ShopLevels;
    public IntVariable CurrentLevel;

    private ItemObject Item = null;

    [SerializeField]
    private GameEvent NotEnoughMoney;
    
    [SerializeField]
    private GameEvent PurchasedItem;
    
    // TODO: NEED REFERENCE TO PLAYER ITEMS INVENTORY THING

    public override void OnPurchase()
    {
        if (Item == null) return;

        // If player does not have enough money, raise NotEnoughMoney event, and return; 

        // Add item to player's inventory

        // Subtract the price of the from the player currency total

        // Remove item    
        Item = null;

        PurchasedItem.Raise();
    }

    private void GetItemFromItemPool() {
        var shopLevel = ShopLevels.LevelTierPicker[CurrentLevel.Value];
        var decidedTier = shopLevel.ChooseTier();
        var decidedItem = Pool.GetItem(decidedTier);

        Item = decidedItem;
    }

    public ItemObject GetItem() => Item;

    void OnEnable() {
        GetItemFromItemPool();
    } 


    void OnDisable() {
        if (Item != null) {
            Pool.AddItem(Item);
        }
    }
}

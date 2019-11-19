using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ShopSlot : ScriptableObject
{
    public IntVariable PlayerCurrency;    

    [SerializeField]
    private GameEvent NotEnoughMoney;
    
    [SerializeField]
    private GameEvent PurchasedItem;
 
    public void Buy() {
        var price = GetPrice();

        if (PlayerCurrency.Value < price) {
            NotEnoughMoney.Raise();
            return;            
        }

        if (OnPurchase()) {
            PlayerCurrency.Value -= price;
            PurchasedItem.Raise();
        } 
    }

    public abstract void Init();

    public abstract void Reset();

    public abstract bool OnPurchase(); 

    public abstract GameObject GetItemPrefab(); 

    public abstract string GetTitle();

    public abstract string GetDescription();

    public abstract int GetPrice();
}

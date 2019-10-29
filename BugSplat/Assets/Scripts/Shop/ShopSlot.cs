using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShopSlot : ScriptableObject
{
    // TODO: NEED REFERENCE TO PLAYER CURRENCY TOTAL

    public abstract void OnPurchase(); 
}

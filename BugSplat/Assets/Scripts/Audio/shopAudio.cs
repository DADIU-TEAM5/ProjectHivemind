using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopAudio : MonoBehaviour
{
    [Header("Wwise events")]
    public AK.Wwise.Event inspectItem;
    public AK.Wwise.Event putBackItem;
    public AK.Wwise.Event purchaseItem;
    public AK.Wwise.Event insufficientFunds;
    public AK.Wwise.Event toadSpeak;

    public void InspectItemEvent()
    {
        inspectItem.Post(this.gameObject);
    }

    public void PutBackItemEvent()
    {
        putBackItem.Post(this.gameObject);
    }

    public void PurchaseItemEvent()
    {
        purchaseItem.Post(this.gameObject);
    }

    public void InsufficientFundsEvent()
    {
        insufficientFunds.Post(this.gameObject);
    }

    public void EnterShopEvent()
    {
        toadSpeak.Post(this.gameObject);
    }
}

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
    public AK.Wwise.Event shopAmbience;
    public AK.Wwise.Event enterShop;
    public AK.Wwise.Event exitShop;

    [Header("Location")]
    public GameObject shoplocation;

    public void InspectItemEvent()
    {
        inspectItem.Post(this.gameObject);
    }

    public void ToadTalkEvent()
    {
        toadSpeak.Post(this.gameObject);
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
        enterShop.Post(this.gameObject);
    }

    public void ExitShopEvent()
    {
        exitShop.Post(this.gameObject);
    }

    public void shopAmbienceTrigger()
    {
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(2);
        shopAmbience.Post(shoplocation);

    }
}

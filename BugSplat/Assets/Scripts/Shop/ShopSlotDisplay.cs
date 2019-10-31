using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotDisplay : GameLoop
{
    public ShopSlot Slot;

    [SerializeField]
    private Image SlotImage;

    [SerializeField]
    private TMPro.TextMeshProUGUI PriceText;

    public void Start() {
        Slot.Init();
    }

    public override void LoopUpdate(float deltaTime)
    {
        SlotImage.sprite = Slot.GetSprite();
        PriceText.text = Slot.GetPrice().ToString();
    }

    public override void LoopLateUpdate(float deltaTime)
    {
    }

    public void Buy() {
        Slot.Buy();
    }
}

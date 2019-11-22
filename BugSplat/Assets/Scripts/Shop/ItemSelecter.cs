using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemSelecter : GameLoop
{
    public Camera ShopCamera;

    public ShopSlotDisplay ShopDisplay1, ShopDisplay2, ShopDisplay3, Consumable;
    public Transform Slot1, Slot2, Slot3, ConsumableSlot;

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void LoopLateUpdate(float deltaTime) {}

    public override void LoopUpdate(float deltaTime) {
        if (Input.GetMouseButtonUp(0)) {
            var mousePos = Input.mousePosition;

            RaycastHit hit;
            var ray = ShopCamera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit, 100f)) {
                var parent = hit.transform.parent;

                if (parent == Slot1) {
                    ShopDisplay2.DeselectItem();
                    ShopDisplay3.DeselectItem();
                    Consumable.DeselectItem();

                    ShopDisplay1.ToggleItem();
                } else if (parent == Slot2) {
                    ShopDisplay1.DeselectItem();
                    ShopDisplay3.DeselectItem();
                    Consumable.DeselectItem();

                    ShopDisplay2.ToggleItem();
                } else if (parent == Slot3) {
                    ShopDisplay2.DeselectItem();
                    ShopDisplay1.DeselectItem();
                    Consumable.DeselectItem();

                    ShopDisplay3.ToggleItem();
                } else if (parent == ConsumableSlot) {
                    ShopDisplay2.DeselectItem();
                    ShopDisplay3.DeselectItem();
                    ShopDisplay1.DeselectItem();

                    Consumable.ToggleItem();
                } else {
                    ShopDisplay2.DeselectItem();
                    ShopDisplay3.DeselectItem();
                    ShopDisplay1.DeselectItem();
                    Consumable.DeselectItem();
                }
            }
        }
    }
}

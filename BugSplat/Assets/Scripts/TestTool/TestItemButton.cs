using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestItemButton : MonoBehaviour
{
    public Button buttonComponent;
    public Item TestItem;
    public Inventory PlayerInventory;
    public Text ButtonText;
    public int FontSize;


    public void Setup(Item item)
    {
        TestItem = item;

        SetButtonText();
    }

    public void ToggleButton()
    {

        Debug.Log("WeAddingItems");

        PlayerInventory.AddItem(TestItem);
        //SetButtonText();

    }

    public void SetButtonText()
    {

        ButtonText.text = TestItem.name;
        ButtonText.fontSize = FontSize;

    }
}

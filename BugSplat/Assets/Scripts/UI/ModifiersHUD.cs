using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiersHUD : MonoBehaviour
{
    public Inventory PlayerIventory;
    public TMPro.TextMeshProUGUI PageNumber;
    public List<GameObject> ItemList;

    public GameObject NextArrow;
    public GameObject PreviousArrow;

    private int _pageNR = 1;

    public void OnEnable()
    {
        PreviousArrow.SetActive(false);
        if (PlayerIventory.Items.Count < 10)
        {
            NextArrow.SetActive(false);
        }

        UpdateItemsShown();
    }

    public void NextPage()
    {
        _pageNR++;
        PageNumber.text = _pageNR.ToString();
        PreviousArrow.SetActive(true);

        if (_pageNR * 9 >= PlayerIventory.Items.Count)
            NextArrow.SetActive(false);

        UpdateItemsShown();
    }

    public void PreviousPage()
    {
        _pageNR--;
        NextArrow.SetActive(true);
        PageNumber.text = _pageNR.ToString();
        if (_pageNR == 1)
            PreviousArrow.SetActive(false);

        UpdateItemsShown();
    }

    public void UpdateItemsShown()
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            RemoveItem(i);
            AddItem(i);
        }
    }

    public void RemoveItem(int index)
    {
        Transform model = ItemList[index].transform.GetChild(0);
        if(model.childCount == 1)
        {
            Destroy(model.GetChild(0).gameObject);
            Debug.Log("Childhood ruined");
        }
    }

    public void AddItem(int index)
    {
        Transform model = ItemList[index].transform.GetChild(0);
        //if(model.childCount > 0)
        //{
        //    Debug.LogError("No. I am your father");
        //    return;
        //}
        int i = (_pageNR - 1) * 9;
        Debug.Log("i, ItemsCount: " + i + ", " + PlayerIventory.Items.Count);

        if (PlayerIventory.Items.Count > i+index)
        {
            GameObject prefab = PlayerIventory.Items[i+index].Info.ItemPrefab;
            GameObject item = Instantiate(prefab);
            item.SetActive(true);
            item.transform.SetParent(model);
            item.transform.position = model.position;
            item.transform.localScale = new Vector3(1,1,1);
           

        }
      
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : MonoBehaviour
{
    public BoolVariable IsShopOpenSO;
    public GameObject ShopDoor;
    public GameObject ShopCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        if (IsShopOpenSO.Value == true)
        {
            ShopDoor.SetActive(false);
            ShopCollider.SetActive(true);
        } else
        {
            ShopDoor.SetActive(true);
            ShopCollider.SetActive(false);
        }
    }
}

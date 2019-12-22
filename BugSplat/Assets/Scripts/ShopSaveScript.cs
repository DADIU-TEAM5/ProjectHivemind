using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSaveScript : MonoBehaviour
{
    public SaveLoadEnforcer saveLoad;

    private void Start()
    {
        saveLoad.Save();
    }
}

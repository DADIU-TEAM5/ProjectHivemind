using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankLoaderScript : MonoBehaviour
{

    public AK.Wwise.Bank Arena;
    public AK.Wwise.Bank Hub;
    public AK.Wwise.Bank Music;
    public AK.Wwise.Bank UI;
    public AK.Wwise.Bank Player;

    void Awake()
    {
        Music.Load(true, false);
        UI.Load(true, false);
    }

    public void OnEnterGame()
    {
        Hub.Load(true, false);
        Player.Load(true, false);
    }

    public void OnEnterArena()
    {
        Hub.Unload();
        Arena.Load(true, false);
    }

    public void OnEnterHub()
    {
        Arena.Unload();
        Hub.Load(true, false);
    }




}

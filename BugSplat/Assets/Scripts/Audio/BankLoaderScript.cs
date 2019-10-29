using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankLoaderScript : MonoBehaviour
{
    [Header("Wwise banks")]
    public AK.Wwise.Bank Arena;
    public AK.Wwise.Bank Hub;
    public AK.Wwise.Bank Music;
    public AK.Wwise.Bank UI;
    public AK.Wwise.Bank Player;

    void Awake()
    {
        Music.Load(false, false);
        UI.Load(false, false);
        //This section, "start", is only a placeholder for the playable
        Player.Load(false, false);
        Arena.Load(false, false);
    }

    public void OnEnterGame()
    {
       // Hub.Load(false, false);
        //Player.Load(false, false);
    }

    public void OnEnterArena()
    {
        Hub.Unload();
        Arena.Load(false, false);
    }

    public void OnEnterHub()
    {
        Arena.Unload();
        Hub.Load(false, false);
    }




}

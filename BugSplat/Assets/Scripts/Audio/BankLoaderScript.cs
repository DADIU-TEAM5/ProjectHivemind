using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankLoaderScript : MonoBehaviour
{
    [Header("Wwise banks")]
    public AK.Wwise.Bank AreaArena;
    public AK.Wwise.Bank AreaHub;
    public AK.Wwise.Bank AreaShop;
    public AK.Wwise.Bank Music;
    public AK.Wwise.Bank UI;
    public AK.Wwise.Bank Player;
    public AK.Wwise.Bank EnemyInsects;

    public string currentScenePlaceholderSO;

    void Awake()
    {
        Music.Load(false, false);
        UI.Load(false, false);
        //This section, "start", is only a placeholder for the playable
        Player.Load(false, false);
        AreaArena.Load(false, false);
        EnemyInsects.Load(false, false);
    }

    public void OnEnterGame()
    {
       // AreaHub.Load(false, false);
        //Player.Load(false, false);
    }




}

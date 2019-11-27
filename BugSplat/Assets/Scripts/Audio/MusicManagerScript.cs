using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{

    [Header("Wwise events")]
    public AK.Wwise.Event IntroMusic;
    public AK.Wwise.Event BattleMusic;
    public AK.Wwise.Event HubMusic;
    public AK.Wwise.Event ShopMusic;
    public AK.Wwise.Event StopBattleMusic;

    [Header("Wwise states")]
    public AK.Wwise.State BattleMusicUnengaged;
    public AK.Wwise.State BattleMusicEngaged;
    public AK.Wwise.State BattleMusicWon;
    public AK.Wwise.State BattleMusicLost;

    [Header("Variables")]
    public IntVariable EnemiesLeft;
    public IntVariable EnemiesAgroed;
    public BoolVariable NoEnemiesOnScreen;
    public FloatVariable MaxHealth;
    public FloatVariable CurrentHealth;
    public FloatVariable DistanceToClosestBug;

    [Header("Wwise RTPCs")]
    public AK.Wwise.RTPC EnemiesLeftRTPC;
    public AK.Wwise.RTPC EnemiesAgroedRTPC;
    public AK.Wwise.RTPC HealthRTPC;
    public AK.Wwise.RTPC DistanceToClosestBugRTPCs;

    [Header("Parameters")]
    public float IntenseOMeter;
    public float EnemiesLeftMeter;
    public BoolVariable isShopOpen;

    [Header("Gameobjects")]
    public GameObject FrogShop;

    void Start()
    {
        IntroMusic.Post(this.gameObject);
    }

    void Update()
    {
        IntenseOMeter = EnemiesAgroed.Value;
        EnemiesLeftMeter = EnemiesLeft.Value;
        EnemiesAgroedRTPC.SetGlobalValue(EnemiesAgroed.Value);
        EnemiesLeftRTPC.SetGlobalValue(EnemiesLeft.Value);
        HealthRTPC.SetGlobalValue(CurrentHealth.Value);
    }

    public void EnemyAggroedEvent()
    {
        
    }

    public void ArenaLoad()
    {
        BattleMusic.Post(this.gameObject);
        ShopMusic.Stop(FrogShop);
    }

    public void HubLoad()
    {
        HubMusic.Post(this.gameObject);
        BattleMusic.Stop(this.gameObject);
        StopBattleMusic.Post(this.gameObject);

    }

    public void playShopMusic()
    {
        //Make sure no duplicates
        ShopMusic.Stop(FrogShop);

        if (isShopOpen.Value == true)
        {
            //post event
            ShopMusic.Post(FrogShop);
        }
    }

    public void PlayerWonEvent()
    {
        //StopBattleMusic.Post(this.gameObject);
        BattleMusicWon.SetValue();
    }

    public void CloseWallEvent()
    {
        BattleMusicEngaged.SetValue();
    }

}

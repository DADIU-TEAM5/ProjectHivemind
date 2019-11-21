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

    [Header("Variables")]
    public IntVariable EnemiesLeft;
    public IntVariable EnemiesAgroed;
    public BoolVariable NoEnemiesOnScreen;
    public FloatVariable MaxHealth;
    public FloatVariable CurrentHealth;

    [Header("Parameters")]
    public float IntenseOMeter;
    public float IntensityOffset;

    [Header("Gameobjects")]
    public GameObject FrogShop;

    void Start()
    {
        IntroMusic.Post(this.gameObject);
    }

    void Update()
    {
        IntenseOMeter = (EnemiesAgroed.Value - CurrentHealth.Value)+IntensityOffset;
    }

    public void ArenaLoad()
    {
        BattleMusic.Post(this.gameObject);
        ShopMusic.Stop(FrogShop);
    }

    public void HubLoad()
    {
        HubMusic.Post(this.gameObject);

    }

    public void playShopMusic()
    {
        //Make sure no duplicates
        ShopMusic.Stop(FrogShop);

        //post event
        ShopMusic.Post(FrogShop);
    }

}

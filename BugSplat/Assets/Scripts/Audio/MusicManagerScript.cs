using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{

    [Header("Wwise events")]
    public AK.Wwise.Event MainScore;

    [Header("Wwise states")]
    public AK.Wwise.State Test;

    [Header("Variables")]
    public IntVariable EnemiesLeft;
    public IntVariable EnemiesAgroed;
    public BoolVariable NoEnemiesOnScreen;
    public FloatVariable MaxHealth;
    public FloatVariable CurrentHealth;

    [Header("Parameters")]
    public float IntenseOMeter;
    public float IntensityOffset;


    void Start()
    {
        MainScore.Post(this.gameObject);
    }


    void Update()
    {
        IntenseOMeter = EnemiesAgroed.Value - CurrentHealth.Value;
    }

}

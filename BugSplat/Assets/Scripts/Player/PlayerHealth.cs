using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public FloatVariable MaxHealth;

    public GameObjectVariable HexMapParent;

    [SerializeField]
    private FloatVariable CurrentHealth;


    [SerializeField]
    private GameEvent TookDamageEvent;

    public void Start()
    {
        CurrentHealth.Value = MaxHealth.Value;
    }

    public void TakeDamage(float damage)
    {
        //print("The player took " + damage);
        CurrentHealth.Value -= damage;

        if (TookDamageEvent != null)
        {
            if (damage > 0f)
            {
                TookDamageEvent.Raise();
            }
        }

        if(CurrentHealth.Value <= 0)
        {
            Destroy(HexMapParent.Value);

            OverallSceneWorker.LoadScene("Death Scene");
            

        }
    }
}

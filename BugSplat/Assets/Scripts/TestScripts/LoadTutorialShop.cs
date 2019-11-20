using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoadTutorialShop : MonoBehaviour
{

    public BoolVariable TutorialIsActive;
    public BoolVariable PlayerControlOverrideSO;
    public Transform Player;
    public GameObject Cage;
    public BoolVariable ShopIsOpenSO;
    public IntVariable CurrentLevel;

    void Start()
    {
        if (CurrentLevel.Value == 0)
        {
            ShopIsOpenSO.Value = false;
            TutorialIsActive.Value = true;
        } else
        {
            ShopIsOpenSO.Value = true;
            TutorialIsActive.Value = false;
        }

        if (TutorialIsActive.Value == false)
        {
            PlayerControlOverrideSO.Value = false;
            Player.GetComponent<NavMeshAgent>().enabled = true;
        } else
        {
            Cage.SetActive(true);
        }
    }

}

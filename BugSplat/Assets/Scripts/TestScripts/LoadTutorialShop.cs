using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoadTutorialShop : MonoBehaviour
{

    public BoolVariable TutorialIsActiveSO;
    public BoolVariable PlayerControlOverrideSO;
    public Transform Player;
    public GameObject Cage;
    public BoolVariable ShopIsOpenSO;
    public IntVariable CurrentLevelSO;
    public StringVariable LastSceneSO;

    void Start()
    {
        if (CurrentLevelSO.Value == 0)
        {
            ShopIsOpenSO.Value = false;
            TutorialIsActiveSO.Value = true;
            LastSceneSO.Value = "";
        } else
        {
            ShopIsOpenSO.Value = true;
            TutorialIsActiveSO.Value = false;
        }

        if (TutorialIsActiveSO.Value == false)
        {
            PlayerControlOverrideSO.Value = false;
            Player.GetComponent<NavMeshAgent>().enabled = true;
        } else
        {
            Cage.SetActive(true);
        }
    }

}

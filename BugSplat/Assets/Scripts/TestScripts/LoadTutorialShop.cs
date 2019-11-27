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
    public Animator CharacterCutSceneAnimController;
    public GameObject TutorialEggs;



    private void Start()
    {
        ShopIsOpenSO.Value = true;
        TutorialEggs.SetActive(false);
    }
    
    public void  StartGame()
    {
        if (TutorialIsActiveSO.Value == true)
        {
            ShopIsOpenSO.Value = false;
            Cage.SetActive(true);
            //
            LastSceneSO.Value = "";
            CharacterCutSceneAnimController.enabled = true;
            TutorialEggs.SetActive(true);
        } else
        {
            ShopIsOpenSO.Value = true;
            TutorialIsActiveSO.Value = false;
            TutorialEggs.SetActive(false);
            PlayerControlOverrideSO.Value = false;
            Player.GetComponent<NavMeshAgent>().enabled = true;
        }

    }

}

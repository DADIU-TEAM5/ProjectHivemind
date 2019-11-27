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
    public GameObject[] LevelLights;

    void Start()
    {
        if (CurrentLevelSO.Value == 0)
        {
            ShopIsOpenSO.Value = false;
            TutorialIsActiveSO.Value = true;
            LastSceneSO.Value = "";
            CharacterCutSceneAnimController.enabled = true;
            TutorialEggs.SetActive(true);
        } else
        {
            ShopIsOpenSO.Value = true;
            TutorialIsActiveSO.Value = false;
            TutorialEggs.SetActive(false);
        }

        if (TutorialIsActiveSO.Value == false)
        {
            PlayerControlOverrideSO.Value = false;
            Player.GetComponent<NavMeshAgent>().enabled = true;
        } else
        {
            Cage.SetActive(true);
        }

        for (int i = LevelLights.Length; i >= CurrentLevelSO.Value; i--)
        {
            LevelLights[i].SetActive(false);
        }
    }

}

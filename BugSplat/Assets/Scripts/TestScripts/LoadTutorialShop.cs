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
    public GameObject ShopCollider;
    public BoolVariable ArenaOpenSO;
    public GameObject ArenaCollider;
    public IntVariable CurrentLevelSO;
    public StringVariable LastSceneSO;
    public Animator CharacterCutSceneAnimController;
    public GameObject TutorialEggs;
    public GameObject[] LevelLights;
    public GameObject ShopSign;
    public GameObject SpawnParticle;
    public Vector3Variable PlayerDirectionSO;





    private void Start()
    {
        ShopIsOpenSO.Value = true;
        TutorialEggs.SetActive(false);

        if (CurrentLevelSO.Value == CurrentLevelSO.Max)
        {
            ArenaCollider.SetActive(false);
            ShopCollider.SetActive(false);
        }

        if (LastSceneSO.Value == "Death Scene")
        {
            StartGame();
        }

        for (int i = 0; i <= CurrentLevelSO.Max; i++)
        {
            if (i > 0)
            {
                if (CurrentLevelSO.Value >= i)
                {
                    LevelLights[i - 1].SetActive(true);
                }
                else
                {
                    LevelLights[i - 1].SetActive(false);
                }
            }
        }
    }
    
    public void  StartGame()
    {

        

        if (TutorialIsActiveSO.Value == true)
        {
            Player.gameObject.SetActive(true);
            Player.GetComponent<NavMeshAgent>().enabled = false;
            ShopIsOpenSO.Value = false;
            Cage.SetActive(true);
            //
            LastSceneSO.Value = "";
            CharacterCutSceneAnimController.enabled = true;
            TutorialEggs.SetActive(true);
        } else
        {
            if (LastSceneSO.Value == "_PreloadScene")
            {
                SpawnParticle.SetActive(true);
                SpawnParticle.GetComponent<WaitXSeconds>().enabled = true;
            } else
            {
                SpawnParticle.SetActive(true);
                Player.gameObject.SetActive(true);
            }

            ShopIsOpenSO.Value = true;
            TutorialIsActiveSO.Value = false;
            TutorialEggs.SetActive(false);
            PlayerControlOverrideSO.Value = false;
        }

        if (ShopIsOpenSO.Value == true)
        {
            ShopSign.SetActive(false);
        } else
        {
            ShopSign.SetActive(true);
        }
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControlOverride : MonoBehaviour
{
    public Transform[] Target;
    public Transform Player;
    public FloatVariable PlayerCurrentSpeedSO;
    public BoolVariable PlayerControlOverrideSO;
    public BoolVariable IsShopOpenSO;
    public Vector3Variable PlayerExitPos;
    public BoolVariable IsExiting;
    public float TimeScale;
    public Animator Anim;
    public GameObject[] ShopColliders;
    public Vector3Variable PlayerDirectionSO;
    public Transform PlayerGraphics;


    // Start is called before the first frame update
    void Start()
    {
        if (IsShopOpenSO.Value == true)
        {
            if (IsExiting.Value != true)
            {
                for (int i = ShopColliders.Length - 1; i >= 0; i--)
                {
                    ShopColliders[i].SetActive(true);
                }
                Target[1].gameObject.SetActive(false);
            }
            else
            {
                PlayerControlOverrideSO.Value = true;
                Vector3 heading = Target[2].position - Target[1].position;
                Debug.Log(heading.normalized);
                PlayerDirectionSO.Value = heading.normalized;
                Player.position = Target[1].position;
                IsExiting.Value = false;
            }
        }

    }


    public void GoToTarget(int index)
    {

        PlayerGraphics.localRotation = Quaternion.LookRotation(PlayerDirectionSO.Value, Vector3.up);
        Debug.Log(PlayerDirectionSO.Value);
        IsExiting.Value = true;
        PlayerControlOverrideSO.Value = true;

        if (IsExiting.Value == false)
        {
            Time.timeScale = TimeScale;
        } else
        {
            Time.timeScale = 0.75f;
        }

        Player.GetComponent<TouchControls>().enabled = false;
        Player.GetComponent<PlayerMovement>().enabled = false;
        PlayerCurrentSpeedSO.Value = PlayerCurrentSpeedSO.InitialValue;
        Anim.SetBool("Running", true);
  
        PlayerExitPos.Value = Target[index].position;

        Player.GetComponent<NavMeshAgent>().SetDestination(Target[index].position);
        Player.GetComponent<NavMeshAgent>().updateRotation = false;

    }

    public void ResetPlayerControl()
    {
        PlayerControlOverrideSO.Value = false;
        Player.GetComponent<TouchControls>().enabled = true;
        Player.GetComponent<PlayerMovement>().enabled = true;

        Time.timeScale = 1f;

        for (int i = ShopColliders.Length-1; i >= 0; i--)
        {
            ShopColliders[i].SetActive(true);
        }

        for (int k = Target.Length-1; k > 1; k--)
        {
            Target[k].gameObject.SetActive(false);
        }

        Player.GetComponent<NavMeshAgent>().ResetPath();

        PlayerCurrentSpeedSO.Value = 0f;

        IsExiting.Value = false;

    }

}

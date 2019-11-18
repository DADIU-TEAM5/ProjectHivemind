using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControlOverride : MonoBehaviour
{
    [SerializeField]
    private GameObject[] EnterColliders;

    [SerializeField]
    private Transform[] ExitTargets;

    [SerializeField]
    private BoolVariable IsAreaOpenSO;

    [SerializeField]
    private BoolVariable IsExitingScene;

    public Transform Player;
    public FloatVariable PlayerCurrentSpeedSO;
    public BoolVariable PlayerControlOverrideSO;
    public float TimeScale;
    public Vector3Variable PlayerDirectionSO;
    public Transform PlayerGraphics;
    public GameObject WhiteFadeIn;


    // Start is called before the first frame update
    void Start()
    {
        if (IsAreaOpenSO != null)
        {
            if (IsAreaOpenSO == true)
            {
                LoadColliders();
            }
        }
        else
        {
            LoadColliders();
        }
    }

    private void LoadColliders()
    {
        if (IsExitingScene.Value != true)
        {
            for (int i = EnterColliders.Length - 1; i >= 0; i--)
            {
                EnterColliders[i].SetActive(true);
            }

            for (int k = ExitTargets.Length - 1; k >= 0; k--)
            {
                ExitTargets[k].gameObject.SetActive(false);
            }

            PlayerControlOverrideSO.Value = false;
        }
        else
        {
            LoadPlayerPos();

            for (int i = EnterColliders.Length - 1; i >= 0; i--)
            {
                EnterColliders[i].SetActive(false);
            }

            for (int k = ExitTargets.Length - 1; k >= 0; k--)
            {
                ExitTargets[k].gameObject.SetActive(true);
            }
        }
    }

    public void LoadPlayerPos()
    {
        if (Player != null)
        {
            PlayerControlOverrideSO.Value = true;
            Vector3 heading = ExitTargets[ExitTargets.Length - 1].position - ExitTargets[0].position;
            PlayerDirectionSO.Value = new Vector3(heading.normalized.x, 0, heading.normalized.z);
            Debug.Log("Target: " + ExitTargets[0].position);
            Player.GetComponent<NavMeshAgent>().enabled = false;
            Player.position = ExitTargets[0].position;
            Player.GetComponent<NavMeshAgent>().enabled = true;
            PlayerCurrentSpeedSO.Value = PlayerCurrentSpeedSO.InitialValue;

            if (WhiteFadeIn != null)
            {
                WhiteFadeIn.SetActive(true);
            }
        }
    }

    public void EnterArea()
    {
        IsExitingScene.Value = true;
    }

    public void ExitArea()
    {
        IsExitingScene.Value = false;
    }

    public void GoToTarget(Transform target)
    {

        PlayerControlOverrideSO.Value = true;
        PlayerGraphics.localRotation = Quaternion.LookRotation(PlayerDirectionSO.Value, Vector3.up);

        PlayerCurrentSpeedSO.Value = PlayerCurrentSpeedSO.InitialValue;

        Player.GetComponent<NavMeshAgent>().SetDestination(target.position);
        Player.GetComponent<NavMeshAgent>().updateRotation = false;
    }

    public void ResetPlayerControl()
    {
        PlayerControlOverrideSO.Value = false;

        Time.timeScale = 1f;

        for (int i = EnterColliders.Length-1; i >= 0; i--)
        {
            EnterColliders[i].SetActive(true);
        }

        for (int k = ExitTargets.Length-1; k >= 0; k--)
        {
            ExitTargets[k].gameObject.SetActive(false);
        }

        Player.GetComponent<NavMeshAgent>().ResetPath();

        PlayerCurrentSpeedSO.Value = 0f;

        ExitArea();
    }

}

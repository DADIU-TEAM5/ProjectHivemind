using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerControlOverride : MonoBehaviour
{
    [SerializeField]
    private GameObject[] EnterColliders;

    [SerializeField]
    private Transform[] ExitTargets;

    [SerializeField]
    private BoolVariable IsAreaOpenSO;

    [SerializeField]
    private string IsExitingScene;

    public StringVariable LastSceneSO;

    public Transform Player;
    public FloatVariable PlayerCurrentSpeedSO;
    public BoolVariable PlayerControlOverrideSO;
    public float TimeScale;
    public Vector3Variable PlayerDirectionSO;
    public Transform PlayerGraphics;
    public GameObject WhiteFadeIn;
    public float PlayerSpeed;


    // Start is called before the first frame update
    void Start()
    {

        // SHOULD ONLY BE NECESSARY UNTIL BUILD
        if (SceneManager.GetActiveScene().name == "ArenaGeneration")
        {
            LastSceneSO.Value = "Hub Scene";
        }

        if (IsAreaOpenSO != null)
        {
            if (IsAreaOpenSO.Value == true)
            {
                LoadColliders();
            } else
            {

            }
        }
        else
        {
            LoadColliders();
        }
    }

    private void LoadColliders()
    {
        if (IsExitingScene != LastSceneSO.Value)
        {
            for (int i = EnterColliders.Length - 1; i >= 0; i--)
            {
                EnterColliders[i].SetActive(true);
            }

            for (int k = ExitTargets.Length - 1; k >= 0; k--)
            {
                ExitTargets[k].gameObject.SetActive(false);
            }

            //PlayerControlOverrideSO.Value = false;
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
            //Debug.Log("Target: " + ExitTargets[0].position);
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
        //IsExitingScene.Value = true;
        //LastSceneSO.Value = SceneManager.GetActiveScene().name;
    }

    public void ExitArea()
    {
        //IsExitingScene.Value = false;
    }

    public void GoToTarget(Transform target)
    {

        PlayerControlOverrideSO.Value = true;

        PlayerDirectionSO.Value = new Vector3(target.position.x - Player.transform.position.x, 0, target.position.z - Player.transform.position.z).normalized;

        if (PlayerSpeed != 0)
        {
            PlayerCurrentSpeedSO.Value = PlayerSpeed;
        }
        else
        {
            PlayerCurrentSpeedSO.Value = PlayerCurrentSpeedSO.InitialValue;
        }

        Player.GetComponent<NavMeshAgent>().SetDestination(target.position);
        Player.GetComponent<NavMeshAgent>().updateRotation = false;
    }

    public void ControlOverride()
    {
        PlayerControlOverrideSO.Value = true;
    }

    public void PlayerStop()
    {
        PlayerCurrentSpeedSO.Value = 0;
        ControlOverride();
    }

    public void ResetPlayerControl(bool collidersTargets)
    {
        PlayerControlOverrideSO.Value = false;

        Time.timeScale = 1f;

        if (collidersTargets)
        {
            ResetCollidersTargets();
        }

        Player.GetComponent<NavMeshAgent>().enabled = false;
        Player.GetComponent<NavMeshAgent>().enabled = true;
        Player.GetComponent<NavMeshAgent>().ResetPath();

        PlayerCurrentSpeedSO.Value = 0f;

        ExitArea();
    }

    public void ResetCollidersTargets()
    {
        for (int i = EnterColliders.Length - 1; i >= 0; i--)
        {
            EnterColliders[i].SetActive(true);
        }

        for (int k = ExitTargets.Length - 1; k >= 0; k--)
        {
            ExitTargets[k].gameObject.SetActive(false);
        }
    }

}

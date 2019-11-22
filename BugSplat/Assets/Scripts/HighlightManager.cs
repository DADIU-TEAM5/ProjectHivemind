using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    public float HighlightTime;
    public Material WhiteMaterial;
    public Color HighLightColor = Color.white;
    public GameObject PlayerObject;

    private GameObject _enemy;

    private Material _enemyMat;
    private Material _playerMat;

    private bool canHighlightEnemy = true;

    public void OnEnable()
    {
        if(PlayerObject == null)
        {
            PlayerObject = GameObject.Find("Char_geo");
        } 
        GetPlayerMaterial();
    }

    public void GetPlayerMaterial()
    {
        _playerMat = PlayerObject.GetComponent<Renderer>().material;
    }


    public void HighLightPlayer()
    {
        PlayerObject.GetComponent<Renderer>().material = WhiteMaterial;
        StartCoroutine(PlayerLerpColor(HighlightTime));
    }


    public void HighLightEnemy(GameObject enemy)
    {
        _enemy = enemy;

        if (_enemy != null)
        {
            Enemy enemyScript = _enemy.GetComponent<Enemy>();
            enemyScript.HighlightThisBitch();
        }
        else
            Debug.LogError("No Enemy From Event");

    }


    private IEnumerator PlayerLerpColor(float lerpTime)
    {
        while (true)
        {
            
            yield return new WaitForSeconds(lerpTime);
            PlayerObject.GetComponent<Renderer>().material = _playerMat;
            break;
        }
        
    }



}

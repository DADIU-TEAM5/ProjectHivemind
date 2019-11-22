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
        GetPlayerMaterial();
    }

    public void GetPlayerMaterial()
    {
        _playerMat = PlayerObject.GetComponent<Renderer>().material;
    }

    public void GetEnemyColor()
    {
        
        _enemyMat = _enemy.GetComponentInChildren<Renderer>().material;
    }


    public void HighLightPlayer()
    {
        PlayerObject.GetComponent<Renderer>().material = WhiteMaterial;
        StartCoroutine(PlayerLerpColor(HighlightTime));
    }

    public void HighLightEnemy(GameObject enemy)
    {
        if (!canHighlightEnemy)
            return;
        canHighlightEnemy = false;
        Debug.Log("Highlight Enemy");
        
       
        _enemy = enemy;

        if (_enemy != null)
        {
            Debug.Log("Enemy NOt null");
            GetEnemyColor();
            if (_enemyMat == null)
                Debug.LogError("No Enemy Material found");
            else
                _enemy.GetComponentInChildren<Renderer>().material = WhiteMaterial;

            StartCoroutine(EnemyLerpColor(HighlightTime));
        } else
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
    private IEnumerator EnemyLerpColor(float lerpTime)
    {
        yield return new WaitForSeconds(lerpTime);
        canHighlightEnemy = true;
        if(_enemy != null)
            _enemy.GetComponentInChildren<Renderer>().material = _enemyMat;
        
    }


}

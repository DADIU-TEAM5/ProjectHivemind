using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameHUDController : MonoBehaviour
{
    public GameObject OptionsPanel;
    public BoolVariable GameIsPaused;

    GameObject uM;

    public void OnEnable()
    {
        uM = GameObject.Find("UpdateManager");
        SetupAnimators(this.gameObject);
       
    }


    public void Pause()
    {
        Time.timeScale = 0;
        GameIsPaused.Value = true;
        uM.SetActive(false);
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        GameIsPaused.Value = false;
        uM.SetActive(true);
    }

    public void SetupAnimators(GameObject go)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            SetupAnimators(go.transform.GetChild(i).gameObject);
        }
            
            foreach (Animator anim in go.GetComponentsInChildren<Animator>())
            {
                
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;
                Debug.Log("Found animator: Updatemode = " + anim.updateMode);
        }
    }

    public void QuitButton()
    {
        Unpause();
        SceneManager.LoadScene("MainMenu");
    }

}

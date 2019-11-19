using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameHUDController : MonoBehaviour
{
    public GameObject OptionsMenu;
    public GameObject MainMenu;
    public GameObject ModifiersMenu;
    public GameObject PauseMenu;
    public GameObject InGameHUD;

    public BoolVariable InMenu;


    //public GameObject OptionsPanel;
    public BoolVariable GameIsPaused;

    GameObject uM;

    public void OnEnable()
    {
        uM = GameObject.Find("UpdateManager");
        SetupAnimators(this.gameObject);

        if (InMenu.Value)
            EnterMainMenu();
        else
            EnterInGameHUD();


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

    public void NewGame()
    {
        InMenu.Value = false;
        
    }

    public void BackButton()
    {
        if (InMenu.Value)
            EnterMainMenu();
    }

    // Change Menu
    public void EnterMainMenu()
    {
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        ModifiersMenu.SetActive(false);
        PauseMenu.SetActive(false);
        InGameHUD.SetActive(false);
    }
    public void EnterOptionsMenu()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        ModifiersMenu.SetActive(false);
        PauseMenu.SetActive(false);
        InGameHUD.SetActive(false);
    }
    public void EnterModifiersMenu()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        ModifiersMenu.SetActive(true);
        PauseMenu.SetActive(false);
        InGameHUD.SetActive(false);
    }
    public void EnterPauseMenu()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        ModifiersMenu.SetActive(false);
        PauseMenu.SetActive(true);
        InGameHUD.SetActive(false);
    }

    public void EnterInGameHUD()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        ModifiersMenu.SetActive(false);
        PauseMenu.SetActive(false);
        InGameHUD.SetActive(true);
    }

}

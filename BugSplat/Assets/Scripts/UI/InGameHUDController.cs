using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameHUDController : MonoBehaviour
{
    public TMPro.TMP_FontAsset TextFont;
    public Color ImageColor;
    [Header("Color Objects")]
    public GameObject HealthBar;
    public GameObject MenuIcon;
    public GameObject CurrencyIcon;
    public GameObject KilledIcon;

    public Animator[] HpAnimations;

    public GameObject OptionsMenu;
    public GameObject MainMenu;
    public GameObject ModifiersMenu;
    public GameObject PauseMenu;
    public GameObject InGameHUD;
    public StringVariable LastSceneSO;

    public BoolVariable InMainMenu;
    public SceneHandler SH;

    public GameEvent UIEnterMenuEvent;
    public GameEvent UIExitMenuEvent;
    public GameEvent UIClickButtonEvent;
    public GameEvent UIClickBackEvent;
    public GameEvent UIQuitEvent;

    //public GameObject OptionsPanel;
    public BoolVariable GameIsPaused;

    GameObject uM;

    public void OnEnable()
    {
        //Debug.Log("InMainMenu Value: " + InMainMenu.Value);
        //string sceneName = "";
        //if (SceneManager.GetActiveScene() != null)
        //{
        //    sceneName = SceneManager.GetActiveScene().name;
        //}
        //sceneName = SceneManager.GetActiveScene().name;
        //Debug.Log("SceneName: " + sceneName);

        //// sceneName.Contains("Hub")
        //InMainMenu.Value = (sceneName.Contains("Menu"));

        uM = GameObject.Find("UpdateManager");
        SetupAnimators(this.gameObject);



        //SetupColors(HealthBar);
        //SetupColors(MenuIcon);
        //SetupColors(CurrencyIcon);
        //SetupColors(KilledIcon);


        if (InMainMenu.Value)
        {
            //Debug.Log("Im Not null.....");
            EnterMainMenu(false);
        }
        else
            EnterInGameHUD();

        Debug.Log("OnEnable Done");

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

    public void SetupColors(GameObject go)
    {
        if (go.transform.childCount > 0)
            for (int i = 0; i < go.transform.childCount; i++)
            {

                SetupColors(go.transform.GetChild(i).gameObject);
            }

        foreach (Image img in go.GetComponentsInChildren<Image>())
        {

            img.color = ImageColor;
            //Debug.Log("Found Image");
        }
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
            //Debug.Log("Found animator: Updatemode = " + anim.updateMode);
        }
    }

    public void SetupFonts(GameObject go) {
            
        foreach (TMPro.TextMeshPro text in go.GetComponentsInChildren<TMPro.TextMeshPro>())
        {

            text.font = TextFont;

        }
    }

    public void QuitButton()
    {
        UIExitMenuEvent.Raise();
        UIQuitEvent.Raise();
        LastSceneSO.Value = "_PreloadScene";
        Unpause();
        SceneManager.LoadScene("Hub Scene");
        InMainMenu.Value = true;
        
    }

    // TrashCode
    public void NewGame()
    {
        InMainMenu.Value = false;
        EnterInGameHUD();
        //SH.ChangeScene("ArenaGeneration");
    }

    public void BackButton()
    {
        UIClickBackEvent.Raise();
        if (InMainMenu.Value)
            EnterMainMenu(false);
        else
            EnterPauseMenu(false);
    }

    // Change Menu
    public void EnterMainMenu()
    {
        UIEnterMenuEvent.Raise();
        Debug.Log("Enter Main Menu");
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        ModifiersMenu.SetActive(false);
        PauseMenu.SetActive(false);
        InGameHUD.SetActive(false);
    }

    // Used for not raising additional events
    public void EnterMainMenu(bool b)
    {

        Debug.Log("Enter Main Menu");
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        ModifiersMenu.SetActive(false);
        PauseMenu.SetActive(false);
        InGameHUD.SetActive(false);
    }
    public void EnterOptionsMenu()
    {
        UIClickButtonEvent.Raise();
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        ModifiersMenu.SetActive(false);
        PauseMenu.SetActive(false);
        InGameHUD.SetActive(false);
    }


    public void EnterModifiersMenu()
    {
        UIClickButtonEvent.Raise();
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        ModifiersMenu.SetActive(true);
        PauseMenu.SetActive(false);
        InGameHUD.SetActive(false);
    }
    public void EnterPauseMenu()
    {
        UIEnterMenuEvent.Raise();
        Debug.Log("Enter Pause Menu");
        Pause();
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        ModifiersMenu.SetActive(false);
        PauseMenu.SetActive(true);
        InGameHUD.SetActive(false);
    }

    // Used for not raising additional events
    public void EnterPauseMenu(bool b)
    {

        Debug.Log("Enter Pause Menu");
        Pause();
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        ModifiersMenu.SetActive(false);
        PauseMenu.SetActive(true);
        InGameHUD.SetActive(false);
    }

    public void EnterInGameHUD()
    {
        UIExitMenuEvent.Raise();
        Unpause();
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        ModifiersMenu.SetActive(false);
        PauseMenu.SetActive(false);
        InGameHUD.SetActive(true);
    }

    public void PlayHPAnimation()
    {
        //for (int i = 0; i < HpAnimations.Length; i++)
        //{
        //    HpAnimations[i].Play("TakeDamage");
        //}

        HealthBar.SetActive(false);
        HealthBar.SetActive(true);
    }


    
}

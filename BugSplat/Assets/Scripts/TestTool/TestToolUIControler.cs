using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestToolUIControler : MonoBehaviour
{
    public BoolVariable GameIsPaused;
    public GameObject TestMenu;
    public GameObject UM;

    public void Awake()
    {
        TestMenu.SetActive(false);
        if(UM == null)
        {
            UM = GameObject.Find("UpdateManager");
        }
    }
    public void MenuButton()
    {
        if (GameIsPaused.Value)
        {
            Time.timeScale = 1;
            GameIsPaused.Value = false;
            TestMenu.SetActive(false);
            UM.SetActive(true);
        }

        else
        {
            Time.timeScale = 0;
            GameIsPaused.Value = true;
            TestMenu.SetActive(true);
            UM.SetActive(false);
        }
        
    }

  
}

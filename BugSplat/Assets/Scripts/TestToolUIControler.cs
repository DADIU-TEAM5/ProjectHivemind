using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestToolUIControler : MonoBehaviour
{
    public BoolVariable GameIsPaused;
    public GameObject TestMenu;

    public void Awake()
    {
        TestMenu.SetActive(false);
    }
    public void MenuButton()
    {
        if (GameIsPaused.Value)
        {
            Time.timeScale = 1;
            GameIsPaused.Value = false;
            TestMenu.SetActive(false);
        }

        else
        {
            Time.timeScale = 0;
            GameIsPaused.Value = true;
            TestMenu.SetActive(true);
        }
        
    }
}

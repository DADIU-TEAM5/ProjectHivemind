using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public BoolVariable IsGamePaused;

    [Header("Events")]
    public GameEvent QuitEvent;
    public GameEvent ExitMenuEvent;

    [SerializeField]
    private UpdateManager UpdateManager;

    public void Pause()
    {
        Time.timeScale = 0;
        IsGamePaused.Value = true;
        UpdateManager.gameObject.SetActive(false);
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        IsGamePaused.Value = false;
        UpdateManager.gameObject.SetActive(true);
    }
}

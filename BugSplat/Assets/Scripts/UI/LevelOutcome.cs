using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelOutcome : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Title, TimeRusult, EnemiesKilled, MoneyEarned;
    public Image ShowImage;

    public void SetImage(Sprite sprite)
    {
        ShowImage.sprite = sprite;
    }

    public void SetTitle(string title)
    {
        //Debug.Log("wat");
        Title.text = title;
    }
    public void SetTimeUsed(FloatVariable timeUsed)
    {
        //Debug.Log("wat");
        TimeRusult.text = $"{timeUsed.Value}";
    }

    public void SetKilledEnemies(IntVariable enemiesKilled)
    {
        EnemiesKilled.text = $"{enemiesKilled.Value}";
    }

    public void SetMoneyEarned(IntVariable moneyEarned)
    {
        //Debug.Log("wat");
        MoneyEarned.text = $"{moneyEarned.Value}";
    }

}

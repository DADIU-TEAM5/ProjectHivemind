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

		var second = (int)(timeUsed.Value % 60);
		var min = (int)(timeUsed.Value / 60);
		TimeRusult.text = $"{min}" + ":" + $"{second}";
    }

    public void SetKilledEnemies(IntVariable enemiesKilled)
    {
		//Debug.Log("enemiesKilled" + enemiesKilled.Value);
		EnemiesKilled.text = $"{enemiesKilled.Value}";
    }

    public void SetMoneyEarned(IntVariable moneyEarned)
    {
        //Debug.Log("wat");
        MoneyEarned.text = $"{moneyEarned.Value}";
    }

    
}

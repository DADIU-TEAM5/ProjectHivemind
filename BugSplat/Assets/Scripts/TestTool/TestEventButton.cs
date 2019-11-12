using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEventButton : MonoBehaviour
{

    public Button buttonComponent;
    public GameEvent GE;
    public Text ButtonText;
    public int FontSize;


    public void Setup(GameEvent gameEvent)
    {
        GE = gameEvent;

        SetButtonText();
    }

    public void RaiseEventButton()
    {
        GameObject go = new GameObject();
        Debug.Log("EventRaised by button");
        GE.Raise(go);

        Destroy(go);
    }

    public void SetButtonText()
    {

        if (GE != null)
        {

            ButtonText.text = "Raise: " + GE.name;
            ButtonText.fontSize = FontSize;

        }
        else
            Debug.LogWarning("TestButton has no reference to a scriptable object");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestToggleButton : MonoBehaviour
{
    public BoolVariable Bool;
    public Text ButtonText;
    private string _toggleValue;

    public void ToggleButton()
    {


        if (Bool != null)
        {
            if (!Bool.Value)
            {
                Bool.Value = true;
                _toggleValue = "ON";
            }
            else
            {
                Bool.Value = false;
                _toggleValue = "OFF";
            }
            setButtonText();
        }
        else
        {
            Debug.LogWarning("TestButton has no reference to a scriptable object");
        }


               
    }

    public void setButtonText()
    {
        if (Bool != null)
            ButtonText.text = Bool.name + ": " + _toggleValue;
        else
            Debug.LogWarning("TestButton has no reference to a scriptable object");
    }
}

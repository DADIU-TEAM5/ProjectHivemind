using UnityEngine;
using UnityEngine.UI;

public class TestToggleButton : MonoBehaviour
{
    public Button buttonComponent;
    public BoolVariable Bool;
    public Text ButtonText;
    private string _toggleValue;
    public int FontSize;


    public void Setup(BoolVariable boolVar)
    {
        Bool = boolVar;

        SetButtonText();
    }

    public void ToggleButton()
    {

        Debug.Log("WeToggling");
       
        if (Bool != null)
        {
            Debug.Log("WeToggling And Not Null");
            if (!Bool.Value)
            {
                Bool.Value = true;
            }
            else
            {
                Bool.Value = false;
            }
            SetButtonText();
        }
        else
        {
            Debug.LogWarning("TestButton has no reference to a scriptable object");
        }



    }

    public void SetButtonText()
    {

        if (Bool != null)
        {
            if (Bool.Value)
                _toggleValue = "ON";
            else _toggleValue = "OFF";


            ButtonText.text = Bool.name + ": " + _toggleValue;
            ButtonText.fontSize = FontSize;

        }
        else
            Debug.LogWarning("TestButton has no reference to a scriptable object");
    }
}

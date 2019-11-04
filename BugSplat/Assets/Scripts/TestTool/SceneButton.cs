using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour
{
    public Button buttonComponent;
    public StringVariable SceneName;
    public Text ButtonText;
    private string _toggleValue;


    public void Setup(StringVariable sceneName)
    {
        SceneName = sceneName;

        SetButtonText();
    }

    public void ToggleButton()
    {

        Debug.Log("WeToggling");

        if (SceneName != null)
        {
            OverallSceneWorker.LoadScene(SceneName.Value);
            
        }
        else
        {
            Debug.LogWarning("TestButton has no reference to a scriptable object");
        }



    }

    public void SetButtonText()
    {

        if (SceneName != null)
        {
            


            ButtonText.text = "load "+SceneName;

        }
        else
            Debug.LogWarning("TestButton has no reference to a scriptable object");
    }
}

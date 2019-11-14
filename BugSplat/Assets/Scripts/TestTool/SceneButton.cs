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
    public EnemyObjectList EnemyList;
    public GameObjectVariable HexMapParent;
    public int FontSize;


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

            
            Destroy(HexMapParent.Value);

            EnemyList.Items = new List<Enemy>();
            

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

            ButtonText.fontSize = FontSize;

        }
        else
            Debug.LogWarning("TestButton has no reference to a scriptable object");
    }
}

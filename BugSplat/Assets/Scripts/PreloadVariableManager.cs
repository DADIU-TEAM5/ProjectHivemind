using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloadVariableManager : MonoBehaviour
{
    public BoolVariable InMainMenu;
    public BoolVariable TutorialEnabled;
    public StringVariable LastScene;
  
    // Lets Hardcode some variables... 
    void OnEnable()
    {
        //InMainMenu.Value = true;
        //TutorialEnabled.Value = true;
        //LastScene.Value = "";
    }


}

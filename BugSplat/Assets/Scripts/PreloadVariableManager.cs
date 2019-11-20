using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloadVariableManager : MonoBehaviour
{
    public BoolVariable InMainMenu;
    public BoolVariable TutorialEnabled;
  
    // Lets Hardcode some variables... 
    void OnEnable()
    {
        TutorialEnabled.Value = false;
        InMainMenu.Value = true;
    }


}

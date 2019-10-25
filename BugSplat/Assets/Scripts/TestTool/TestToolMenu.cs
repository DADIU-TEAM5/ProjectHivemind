using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestToolMenu : MonoBehaviour
{
    //List or Array?
    public List<BoolVariable> Boolvariables = new List<BoolVariable>();
    public GameObject ScrollView;
    

    public void CreateUIElements()
    {
        if (Boolvariables.Count > 0)
            for (int i = 0; i < Boolvariables.Count; i++)
            {
                
            }
    }
}

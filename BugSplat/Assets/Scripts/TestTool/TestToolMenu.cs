using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestToolMenu : MonoBehaviour
{
    //List or Array?
    public List<BoolVariable> Boolvariables = new List<BoolVariable>();
    public GameObject ScrollView;


    public GameObject Togglebutton;
    public Button TB;

    public void CreateUIElements()
    {
        if (Boolvariables.Count > 0)
            for (int i = 0; i < Boolvariables.Count; i++)
            {
                GameObject newGO = (GameObject)GameObject.Instantiate(Togglebutton);
                //Button newButton = GameObject.Instantiate(TB);

                //ScrollView.AddComponent(newButton);
                newGO.transform.SetParent(ScrollView.transform);
                newGO.SetActive(true);

                TestToggleButton toggleButton = newGO.GetComponent<TestToggleButton>();
                toggleButton.Setup(Boolvariables[i]);

                //newButton.SetParent(ScrollView);
                //newButton.SetActive(true);
            }
    }

    private void Awake()
    {
        CreateUIElements();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestToolMenu : MonoBehaviour
{
    //List or Array?
    public List<BoolVariable> Boolvariables = new List<BoolVariable>();


    public List<FloatVariable> Floatvariables = new List<FloatVariable>();

    public GameObject ScrollView;

    public GameObject Togglebutton;
    public GameObject FloatSlider;
    public Button TB;

    public void CreateUIElements()
    {
        if (Boolvariables.Count > 0)
            for (int i = 0; i < Boolvariables.Count; i++)
            {
                GameObject newGO = (GameObject)GameObject.Instantiate(Togglebutton);
                newGO.transform.SetParent(ScrollView.transform, false);
                newGO.SetActive(true);

                TestToggleButton toggleButton = newGO.GetComponent<TestToggleButton>();
                toggleButton.Setup(Boolvariables[i]);

            }

        if (Floatvariables.Count > 0)
        {
            for (int i = 0; i < Floatvariables.Count; i++)
            {
                GameObject newGO = (GameObject)GameObject.Instantiate(FloatSlider);
                newGO.transform.SetParent(ScrollView.transform, false);
                newGO.SetActive(true);

                TestFloatSlider floatSlider = newGO.GetComponent<TestFloatSlider>();
                floatSlider.Setup(Floatvariables[i]);

            }
        }
    }

    private void Awake()
    {
        CreateUIElements();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestToolMenu : MonoBehaviour
{
    //List or Array?

    public bool ResetValuesOnPlay = false;
    public List<BoolVariable> Boolvariables = new List<BoolVariable>();

    public List<StringVariable> SceneVariables = new List<StringVariable>();

    public List<FloatVariable> Floatvariables = new List<FloatVariable>();

    public List<IntVariable> IntVariables = new List<IntVariable>();

    public List<GameEvent> GameEvents = new List<GameEvent>();

    public GameObject ScrollView;
    
    public GameObject ItemScrollView;
    public ItemPool AllItems;

    public GameObject ItemButton;
    public GameObject Togglebutton;
    public GameObject EventButton;
    public GameObject ScenButton;
    public GameObject FloatSlider;
    public GameObject IntSlider;
    public Button TB;

    public void CreateUIElements()
    {
        //if(AllItems.Items.Count > 0)
        //{
        //    for (int i = 0; i < AllItems.Items.Count; i++)
        //    {
        //        GameObject newGO = (GameObject)GameObject.Instantiate(ItemButton);
        //        newGO.transform.SetParent(ItemScrollView.transform, false);
        //        newGO.SetActive(true);

        //        //TestToggleButton toggleButton = newGO.GetComponent<TestToggleButton>();
        //        //toggleButton.Setup(Boolvariables[i]);

        //    }
        //}

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
                floatSlider.Setup(Floatvariables[i], Floatvariables[i].Min, Floatvariables[i].Max);

            }
        }

        if (IntVariables.Count > 0)
        {
            for (int i = 0; i < IntVariables.Count; i++)
            {
                GameObject newGO = (GameObject)GameObject.Instantiate(IntSlider);
                newGO.transform.SetParent(ScrollView.transform, false);
                newGO.SetActive(true);

                TestSliderScript intSlider = newGO.GetComponent<TestSliderScript>();
                intSlider.Setup(IntVariables[i], IntVariables[i].Min, IntVariables[i].Max);

            }
        }

        if (GameEvents.Count > 0)
        {
            for (int i = 0; i < GameEvents.Count; i++)
            {
                GameObject newGO = (GameObject)GameObject.Instantiate(EventButton);
                newGO.transform.SetParent(ScrollView.transform, false);
                newGO.SetActive(true);

                TestEventButton eventButton = newGO.GetComponent<TestEventButton>();
                eventButton.Setup(GameEvents[i]);

            }

        }

        for (int i = 0; i < SceneVariables.Count; i++)
        {
            GameObject newGO = (GameObject)GameObject.Instantiate(ScenButton);
            newGO.transform.SetParent(ScrollView.transform, false);
            newGO.SetActive(true);

            SceneButton SceneButton = newGO.GetComponent<SceneButton>();
            SceneButton.Setup(SceneVariables[i]);

        }
    }

    private void Awake()
    {
        if (ResetValuesOnPlay)
            ResetFloatValues();

        CreateUIElements();

    }

    private void ResetFloatValues()
    {
        for (int i = 0; i < Floatvariables.Count; i++)
        {
            Floatvariables[i].ResetValue();
        }
    }
}

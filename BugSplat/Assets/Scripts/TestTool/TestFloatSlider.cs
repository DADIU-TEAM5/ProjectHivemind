using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestFloatSlider : MonoBehaviour
{

    public FloatVariable FloatVar;
    public Text TextLabel;
    private string _textLabel;
    public Slider slider;
    
    public float MaxValue;
    public float MinValue;

    public void Setup(FloatVariable floatVar, float min, float max)
    {
        FloatVar = floatVar;
        slider.value = FloatVar.Value;
        slider.maxValue = max;
        slider.minValue = min;
        SetText();
    }

    public void Setup(FloatVariable floatVar)
    {
        FloatVar = floatVar;
        slider.value = FloatVar.Value;
        SetText();
    }

    public void ChangeSlider()
    {
        FloatVar.Value = Mathf.Round(slider.value* 100f)/100f;
        SetText();
    }

    public void SetText()
    {
        _textLabel = FloatVar.name + ": " + FloatVar.Value;
        TextLabel.text = _textLabel;
    }
}

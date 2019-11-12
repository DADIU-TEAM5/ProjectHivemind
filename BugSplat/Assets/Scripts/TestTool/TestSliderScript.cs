using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSliderScript : MonoBehaviour
{
    public IntVariable IntVar;
    public Text TextLabel;
    private string _textLabel;
    public Slider slider;
    public int FontSize;
    //public FloatVariable FloatVar;

    public int MaxValue;
    public int MinValue;

    public void Setup(IntVariable intVar, int min, int max)
    {
        IntVar = intVar;
        slider.value = IntVar.Value;
        slider.maxValue = max;
        slider.minValue = min;
        slider.value = IntVar.InitialValue;
        SetText();
    }

    public void Setup(IntVariable intVar)
    {
        IntVar = intVar;
        slider.value = IntVar.Value;
        SetText();
    }

    public void ChangeSlider()
    {
        IntVar.Value = (int)slider.value;
        SetText();
    }

    public void SetText()
    {
        _textLabel = IntVar.name + ": " + IntVar.Value;
        TextLabel.text = _textLabel;
        TextLabel.fontSize = FontSize;
    }
}

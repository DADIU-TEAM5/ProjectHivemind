using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioHUD : MonoBehaviour
{
    public FloatVariable SFX_Volume;
    public GameEvent SFX_Changed;
    public FloatVariable Music_Volume;
    public GameEvent Music_Changed;

    public Slider MusicSlider;
    public Slider SFXSlider;

    public void OnEnable()
    {
        MusicSlider.value = Music_Volume.Value;
        SFXSlider.value = SFX_Volume.Value;
        UpdateMusic();
        UpdateSFX();
    }

    public void UpdateMusic()
    {
        Music_Volume.Value = MusicSlider.value;
        Music_Changed.Raise();

        Debug.Log("Music Vol: " + Music_Volume.Value.ToString());
    }

    public void UpdateSFX()
    {
        SFX_Volume.Value = SFXSlider.value;
        SFX_Changed.Raise();
        Debug.Log("SFX Vol: " + SFX_Volume.Value.ToString());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class volumeControl : MonoBehaviour
{

    [Header("Wwise events")]
    public AK.Wwise.Event select;
    public AK.Wwise.Event back;

    [Header("Wwise parameters")]
    public AK.Wwise.RTPC SFXVolume;
    public AK.Wwise.RTPC MusicVolume;

    [Header("Values")]
    public FloatVariable SFXVolumeVAR;
    public FloatVariable MusicVolumeVAR;

    public void onSFXLevelChange()
    {
        SFXVolume.SetValue(null, SFXVolumeVAR.Value);
    }

    public void onMusicLevelChange()
    {
        MusicVolume.SetValue(null, MusicVolumeVAR.Value);
    }

    public void SelectEvent()
    {
        select.Post(this.gameObject);
    }

    public void BackEvent()
    {
        back.Post(this.gameObject);
    }

}

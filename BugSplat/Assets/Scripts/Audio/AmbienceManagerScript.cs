using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManagerScript : MonoBehaviour
{

    [Header("Wwise ambience events")]
    public AK.Wwise.Event arenaAmbience;
    public AK.Wwise.Event hubAmbience;
    public AK.Wwise.Event shopAmbience;

    [Header("Wwise reverb states")]
    public AK.Wwise.State Arenaverb;
    public AK.Wwise.State Hubverb;
    public AK.Wwise.State Shopverb;


    public void ArenaSceneLoad()
    {
        arenaAmbience.Post(this.gameObject);
        Arenaverb.SetValue();
        
    }

    public void HubSceneLoad()
    {
        hubAmbience.Post(this.gameObject);
        Hubverb.SetValue();
    }

    public void ShopSceneLoad()
    {
        shopAmbience.Post(this.gameObject);
        Shopverb.SetValue();
    }

    public void SceneChange()
    {
        arenaAmbience.Stop(this.gameObject);
        hubAmbience.Stop(this.gameObject);
        hubAmbience.Stop(this.gameObject);
    }


}

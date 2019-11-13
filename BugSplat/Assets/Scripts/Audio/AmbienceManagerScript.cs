using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManagerScript : MonoBehaviour
{

    [Header("Wwise ambience events")]
    public AK.Wwise.Event arenaAmbience;
    public AK.Wwise.Event hubAmbience;
    public AK.Wwise.Event shopAmbience;


    public void ArenaSceneLoad()
    {
        arenaAmbience.Post(this.gameObject);
    }

    public void HubSceneLoad()
    {
        hubAmbience.Post(this.gameObject);
    }

    public void ShopSceneLoad()
    {
        shopAmbience.Post(this.gameObject);
    }

    public void SceneChange()
    {
        arenaAmbience.Stop(this.gameObject);
        hubAmbience.Stop(this.gameObject);
        hubAmbience.Stop(this.gameObject);
    }


}

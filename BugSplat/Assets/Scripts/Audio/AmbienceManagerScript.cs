using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManagerScript : MonoBehaviour
{

    [Header("Wwise ambience events")]
    public AK.Wwise.Event arenaAmbience;
    public AK.Wwise.Event hubAmbience;
    public AK.Wwise.Event shopAmbience;
    public AK.Wwise.Event fadeAmbience;

    public string currentScenePlaceholderSO;


    void Start()
    {
        //PH
        arenaAmbience.Post(this.gameObject);

        /*if (currentScenePlaceholderSO == "arena")
        {
            arenaAmbience.Post(this.gameObject);
        }

        if (currentScenePlaceholderSO == "hub")
        {
            hubAmbience.Post(this.gameObject);
        }

        if (currentScenePlaceholderSO == "shop")
        {
            shopAmbience.Post(this.gameObject);
        }*/

    }


    public void SceneChange()
    {

        fadeAmbience.Post(this.gameObject, (uint)AkCallbackType.AK_EndOfEvent, UnloadBankOnEventEnd);

        if (currentScenePlaceholderSO == "arena")
        {
            arenaAmbience.Post(this.gameObject);
        }

        if (currentScenePlaceholderSO == "hub")
        {
            hubAmbience.Post(this.gameObject);
        }

        if (currentScenePlaceholderSO == "shop")
        {
            shopAmbience.Post(this.gameObject);
        }

    }

    public void UnloadBankOnEventEnd(object in_cookie, AkCallbackType in_type, object in_info)
    {

    }


}

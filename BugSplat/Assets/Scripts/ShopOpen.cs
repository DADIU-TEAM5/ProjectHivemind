using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOpen : MonoBehaviour
{

    public GameEvent shopOpen;

    public StringVariable LastScene;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        if(LastScene.Value == "ArenaGeneration")
        shopOpen.Raise();

        yield return null;

    }

}

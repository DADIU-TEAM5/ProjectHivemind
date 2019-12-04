using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOpen : MonoBehaviour
{

    public GameEvent shopOpen;

    public StringVariable LastScene;

    public IntVariable CurrentLevelSO;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3);

        if (LastScene.Value == "ArenaGeneration")
        {
            if (CurrentLevelSO.Value != CurrentLevelSO.Max)
            {
                shopOpen.Raise();

            }
        }

        yield return null;

    }

}

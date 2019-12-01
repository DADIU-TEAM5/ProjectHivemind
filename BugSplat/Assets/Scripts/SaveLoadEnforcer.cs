using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadEnforcer : MonoBehaviour
{

    public ItemPool Pool;
    public FloatVariableList playerStats;
    public IntVariable bodyparts;
    public IntVariable currentlevel;
    public ShopItemSlot[] itemsslots;
    public Inventory inventory;
    public BoolVariable tutorialDone;






    /*
    private IEnumerator Start()
    {
        load();
        while (true)
        {
            yield return new WaitForSeconds(1);
            save();
        }
    }
    */
    public void load()
    {

        int hasSavedBefore =0;

        hasSavedBefore = PlayerPrefs.GetInt("hasSaved");

        if(hasSavedBefore == 64)
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("pool"), Pool);

            for (int i = 0; i < playerStats.Value.Count; i++)
            {


                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("stats" + i), playerStats.Value[i]);
            }


            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("Bodyparts"), bodyparts);
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("level"), currentlevel);
            for (int i = 0; i < itemsslots.Length; i++)
            {
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("slot" + i), itemsslots[i]);
            }
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("inventory"), inventory);

            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("tutorial"), tutorialDone);



            print("game loaded");
        }


    }

    public void save()
    {

        PlayerPrefs.SetInt("hasSaved", 64);


        PlayerPrefs.SetString("pool", JsonUtility.ToJson(Pool));

        //print(JsonUtility.ToJson(Pool.Bought));

        for (int i = 0; i < playerStats.Value.Count; i++)
        {
            PlayerPrefs.SetString("stats" + i, JsonUtility.ToJson(playerStats.Value[i]));
        }

       // PlayerPrefs.SetString("stats", JsonUtility.ToJson(playerStats));
        //print(JsonUtility.ToJson(playerStats));


        PlayerPrefs.SetString("Bodyparts", JsonUtility.ToJson(bodyparts));
        PlayerPrefs.SetString("level", JsonUtility.ToJson(currentlevel));
        for (int i = 0; i < itemsslots.Length; i++)
        {
            PlayerPrefs.SetString("slot"+i, JsonUtility.ToJson(itemsslots[i]));
        }
        PlayerPrefs.SetString("inventory", JsonUtility.ToJson(inventory));

        PlayerPrefs.SetString("tutorial", JsonUtility.ToJson(tutorialDone));

        PlayerPrefs.Save();
        print("game saved");
    }


    public void NewGame()
    {
        for (int i = 0; i < playerStats.Value.Count; i++)
        {

            playerStats.Value[i].ResetValue();

        }
        bodyparts.Value = 0;
        currentlevel.Value = 0;

        for (int i = 0; i < itemsslots.Length; i++)
        {
            itemsslots[i].Reset();
        }

        inventory.Reset();
        Pool.Reset();
        save();
    }


}

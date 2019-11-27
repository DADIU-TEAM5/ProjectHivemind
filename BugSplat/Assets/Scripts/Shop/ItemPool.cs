using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName="Shop/ItemPool")]
public class ItemPool : ScriptableObject
{
    public List<Item> Items;

    [SerializeField]
    public List<bool> Bought;


    // Takes an item out of the pool
    public Item GetItem(Tier tier) {
        if (Items.Count == 0) {
            return null;
        }

        // Get all items that fit the tier, and hasn't been bought
        var filteredItems = GetItemsFromTier(tier);

        // Pick a random
        var rngResult = Random.Range(0f, 1f);
        var rngInt = (int) (rngResult * (filteredItems.Count() - 1));
        Debug.Log(rngInt);
        Debug.Log("count: " + filteredItems.Count);
        var index = filteredItems.ElementAt(rngInt);

        Bought[index] = true;
        return Items[index];
    }
    
    // Returns all items within the specified tier, that also haven't previously been bought
    private List<int> GetItemsFromTier(Tier tier) {
        var result = new List<int>();
        for (var i = 0; i < Items.Count; i++) {
            if (!Bought[i]) {
                var item = Items[i];

                if (item == null) continue;

                if (item.Info.Tier == tier && item.Purchasable()) {
                    result.Add(i);
                }
            }
        }

        return result;
    }

    // If the item exists in the pool and has been bought. Set it to not bought.
    public void ReplenishOnce(Item itemToReplenish) {
        if (itemToReplenish == null) return;
        
        for (var i = 0; i < Items.Count; i++) {
            var item = Items[i];

            if (item == itemToReplenish) {
                if (Bought[i]) {
                    Bought[i] = false;
                    return;
                }
            }
        }
    }

    public void AddItem(Item item) {
        Items.Add(item);
        Bought.Add(false);
    }

    private void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        if (Items == null) Items = new List<Item>();
        Bought = new List<bool>(new bool[Items.Count]);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName="Shop/ItemPool")]
public class ItemPool : ScriptableObject
{
    public List<ItemObject> Items;

    private List<bool> Bought;


    // Takes an item out of the pool
    public ItemObject GetItem(Tier tier) {
        if (Items.Count == 0) {
            return null;
        }

        // Get all items that fit the tier, and hasn't been bought
        var filteredItems = GetItemsFromTier(tier);

        // Pick a random
        var rngResult = Random.Range(0f, 1f);
        var rngInt = (int) (rngResult * filteredItems.Count());
        var index = filteredItems.ElementAt(rngInt);

        Bought[index] = true;
        return Items[index];
    }
    
    // Returns all items within the specified tier, that also haven't previously been bought
    private IEnumerable<int> GetItemsFromTier(Tier tier) {
        for (var i = 0; i < Items.Count; i++) {
            if (!Bought[i]) {
                var item = Items[i];

                if (item.Tier == tier) {
                    yield return i;
                }
            }
        }
    }

    // If the item exists in the pool and has been bought. Set it to not bought.
    public void ReplenishOnce(ItemObject itemToReplenish) {
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

    public void AddItem(ItemObject item) {
        Items.Add(item);
        Bought.Add(false);
    }

    public void OnEnable() {
        if (Items == null) Items = new List<ItemObject>();
        Bought = new List<bool>(Items.Count);
    }
}
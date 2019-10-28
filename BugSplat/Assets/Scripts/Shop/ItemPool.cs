using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName="Shop/ItemPool")]
public class ItemPool : ScriptableObject
{
    public List<Item> Items;

    private Random _random;

    // Takes an item out of the pool
    public Item GetItem(Tier tier) {
        var filteredItems = Items.Where(x => x.ItemObject.Tier == tier && !x.Bought);

        var rngResult = Random.Range(0f, 1f);
        var rngInt = (int) (rngResult * filteredItems.Count());

        var item = filteredItems.ElementAt(rngInt);
        item.Bought = true;

        return item;
    }

    // Replenish an item in the pool if it had previously been bought 
    public void AddItem(Item item) {
        var poolItem = Items.Find(x => x.ItemObject == item.ItemObject && x.Bought == true);
        poolItem.Bought = false;
    }

    private void Reset() {
        _random = new Random();

        for (int i = 0; i < Items.Count; i++) {
            Items[i].Bought = true;
        }

    }

    public class Item {
        public ItemObject ItemObject;

        internal bool Bought = false;
    }
}
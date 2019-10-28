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
        var filteredItems = Items.Where(x => x.Tier == tier && x.Amount > 0);

        var rngResult = Random.Range(0f, 1f);
        var rngInt = (int) (rngResult * filteredItems.Count());

        var item = filteredItems.ElementAt(rngInt);
        item.Amount = 0;

        return item;
    }

    // Adds an item to the pool
    public void AddItem(Item item) {
        item.Amount = 1;

        Items.Add(item); 
    }

    void OnEnable() {
        Reset();
    }

    private void Reset() {
        _random = new Random();

        for (int i = 0; i < Items.Count; i++) {
            Items[i].Amount = 1;
        }

    }

    public class Item {
        public Tier Tier;

        internal int Amount = 1;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IslandBoy
{
    [Serializable]
    public class LootTable
    {
        [field:SerializeField] public List<Loot> Table { get; private set; }

        public Dictionary<ItemObject, int> ReturnLoot()
        {
            Dictionary<ItemObject, int> lootReturn = new();

            foreach (Loot loot in Table)
            {
                int randValueBtween0and99 = Random.Range(0, 100);

                if (randValueBtween0and99 < loot.Chance)
                {
                    int dropAmount = Random.Range(loot.Min, loot.Max + 1);
                    lootReturn.Add(loot.Item, dropAmount);
                }
            }

            return lootReturn;
        }
    }

    [Serializable]
    public class Loot 
    {
        public ItemObject Item;
        public int Min;
        public int Max;
        [Tooltip("Percent chance between 0% and 100%")]
        public float Chance;
    }
}

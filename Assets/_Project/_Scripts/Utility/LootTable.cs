using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [Serializable]
    public class LootTable
    {
        [field:SerializeField] public List<Loot> Table { get; private set; }

        public Dictionary<ItemObject, int> ReturnLoot()
        {
            Dictionary<ItemObject, int> loot = new();

            foreach (Loot item in Table)
            {

            }

            return returnLoot;
        }
    }

    [Serializable]
    public class Loot 
    {
        public ItemObject Item;
        public int Min;
        public int Max;
        public float Chance;
    }
}

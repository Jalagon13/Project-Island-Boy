using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "[REC] ", menuName = "Crafting/New Recipe")]
    public class CraftingRecipeObject : ScriptableObject
    {
        public ItemObject OutputItem;
        public int OutputAmount;
        public float CraftingTimer;
        public List<ItemAmount> ResourceList = new();
    }

    [Serializable]
    public struct ItemAmount
    {
        public ItemObject Item;
        public int Amount;
    }
}

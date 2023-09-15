using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "[RDB] ", menuName = "New Recipe Database")]
    public class RecipeDatabaseObject : ScriptableObject
    {
        public Recipe[] Database;
    }

    [Serializable]
    public class Recipe
    {
        public ItemObject OutputItem;
        public int OutputAmount;
        public List<ItemAmount> ResourceList = new();
    }

    [Serializable]
    public struct ItemAmount
    {
        public ItemObject Item;
        public int Amount;
    }
}

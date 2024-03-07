using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "[IDB] ", menuName = "Create Item/New Database")]
    public class ItemDatabaseObject : ScriptableObject
    {
        public ItemObject[] Database;
        public float[] Rarity; // rarity is only for fishing
        public float[] Difficulty; // difficulty is only for fishing
    }
}

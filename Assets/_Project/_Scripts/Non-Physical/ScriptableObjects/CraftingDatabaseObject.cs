using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "[CDB] ", menuName = "Crafting/New Database")]
    public class CraftingDatabaseObject : ScriptableObject
    {
        public CraftingRecipeObject[] Database;
    }
}

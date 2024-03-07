using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "[IDB] ", menuName = "Create Item/Database/New Item Database")]
    public class ItemDatabaseObject : ScriptableObject
    {
        public ItemObject[] Database;
    }
}

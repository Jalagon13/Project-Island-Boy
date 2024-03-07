using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "[FDB] ", menuName = "Create Item/Database/New Fishing Database")]
    public class FishingDatabaseObject : ScriptableObject
    {
        public FishDifficulty[] Database;
    }
}

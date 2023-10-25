using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "[NPC] ", menuName = "New Npc")]
    public class NpcObject : ScriptableObject
    {
        public string Name;
        [field: TextArea]
        public string Description;
        public Sprite Icon;
        public GameObject NPC;
        public List<DeployObject> FurnitureCheckList;
    }
}

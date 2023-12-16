using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Spell Database", menuName = "Magic/Spell Database")]
    public class CreateSpellDataBase : ScriptableObject
    {
        // int is ID and GameObject is Spell Projectile;
        public SpellEffect[] MagicSpells;
        public Dictionary<int, GameObject> SpellDatabase = new Dictionary<int, GameObject>();
    }
}
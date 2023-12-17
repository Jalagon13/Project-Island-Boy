using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "[TMR] ", menuName = "New Reference/Tilemap Reference")]
    public class TilemapObject : ScriptableObject
    {
        [SerializeField] private Tilemap _tilemap;
        public Tilemap Tilemap { get { return _tilemap; } set { _tilemap = value; } }
    }
}

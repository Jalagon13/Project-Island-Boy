using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New TM Reference", menuName = "New Reference/Tilemap References")]
    public class TilemapReferences : ScriptableObject
    {
        private Tilemap _groundTilemap;
        private Tilemap _floorTilemap;
        private Tilemap _wallTilemap;

        public Tilemap GroundTilemap { get { return _groundTilemap; } set { _groundTilemap = value; } }
        public Tilemap FloorTilemap { get { return _floorTilemap; } set { _floorTilemap = value; } }
        public Tilemap WallTilemap { get { return _wallTilemap; } set { _wallTilemap = value; } }
    }
}

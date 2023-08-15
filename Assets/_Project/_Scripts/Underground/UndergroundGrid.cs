using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class UndergroundGrid : MonoBehaviour
    {
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private Tilemap _ugGroundTm;
        [SerializeField] private Tilemap _ugFloorTm;
        [SerializeField] private Tilemap ugWallTm;

        private void OnEnable()
        {
            _tmr.GroundTilemap = _ugGroundTm;
            _tmr.FloorTilemap = _ugFloorTm;
            _tmr.WallTilemap = ugWallTm;
        }
    }
}

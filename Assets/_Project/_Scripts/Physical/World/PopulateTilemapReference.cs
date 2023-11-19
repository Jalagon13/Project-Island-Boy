using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class PopulateTilemapReference : MonoBehaviour
    {
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private Tilemap _surfaceGroundTm;
        [SerializeField] private Tilemap _surfaceFloorTm;
        [SerializeField] private Tilemap _surfaceWallTm;

        private void OnEnable()
        {
            _tmr.GroundTilemap = _surfaceGroundTm;
            _tmr.FloorTilemap = _surfaceFloorTm;
            _tmr.WallTilemap = _surfaceWallTm;
        }
    }
}

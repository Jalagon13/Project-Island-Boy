using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class SurfaceGrid : MonoBehaviour
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

            SetupPathfinding();
        }

        private void SetupPathfinding()
        {
            int width = 55;
            int height = 55;
            float nodeSize = 0.41f;
            Vector2 center = new(-2, 3);

            AstarManager.Instance.RecalculateGrid(width, height, nodeSize, center);
        }
    }
}

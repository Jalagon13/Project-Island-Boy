using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class SurfaceGrid : MonoBehaviour
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private float _nodeSize;
        [SerializeField] private Vector2 _center;
        [Header("TM stuff")]
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
            AstarManager.Instance.RecalculateGrid(_width, _height, _nodeSize, _center);
        }
    }
}

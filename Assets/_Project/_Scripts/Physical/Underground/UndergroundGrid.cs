using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class UndergroundGrid : MonoBehaviour
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private float _nodeSize;
        [SerializeField] private Vector2 _center;
        [Header("TM stuff")]
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private Tilemap _ugGroundTm;
        [SerializeField] private Tilemap _ugFloorTm;
        [SerializeField] private Tilemap ugWallTm;

        private void Awake()
        {
            _tmr.GroundTilemap = _ugGroundTm;
            _tmr.FloorTilemap = _ugFloorTm;
            _tmr.WallTilemap = ugWallTm;
        }

        private void OnEnable()
        {
            _tmr.GroundTilemap = _ugGroundTm;
            _tmr.FloorTilemap = _ugFloorTm;
            _tmr.WallTilemap = ugWallTm;

            StartCoroutine(SetupPathfinding());
        }

        private IEnumerator SetupPathfinding()
        {
            yield return new WaitForEndOfFrame();
            AstarManager.Instance.RecalculateGrid(_width, _height, _nodeSize, _center);
        }
    }
}

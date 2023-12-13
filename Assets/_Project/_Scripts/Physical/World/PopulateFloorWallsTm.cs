using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class PopulateFloorWallsTm : MonoBehaviour
    {
        [SerializeField] private TilemapReference _floorTm;
        [SerializeField] private TilemapReference _wallTm;
        [Space(10)]
        [SerializeField] private Tilemap _floor;
        [SerializeField] private Tilemap _walls;

        private void OnEnable()
        {
            _floorTm.Tilemap = _floor;
            _wallTm.Tilemap = _walls;
        }
    }
}

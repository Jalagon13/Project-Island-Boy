using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class PopulateGroundTilemap : MonoBehaviour
    {
        [SerializeField] private TilemapObject _groundTm;
        [SerializeField] private Tilemap _surfaceGroundTm;

        private void OnEnable()
        {
            _groundTm.Tilemap = _surfaceGroundTm;
        }
    }
}

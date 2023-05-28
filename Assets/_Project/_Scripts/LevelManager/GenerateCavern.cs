using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class GenerateCavern : MonoBehaviour
    {
        [SerializeField] private GameObject _coalPrefab;
        [Header("Generation Parameters")]
        [SerializeField] private float _nearEdgeDetectLength;
        [SerializeField] private float _chance;
        [SerializeField] private float _scale;

        private Tilemap _floor;
        private CaveLevelControl _control;

        private void Awake()
        {
            _floor = transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();
            _control = transform.GetComponent<CaveLevelControl>();
        }

        private void Start()
        {
            _control.SpawnPlayer();
            GenerateResources();
        }

        private void GenerateResources()
        {
            for (int x = _floor.cellBounds.xMin; x < _floor.cellBounds.xMax; x++)
            {
                for (int y = _floor.cellBounds.yMin; y < _floor.cellBounds.yMax; y++)
                {
                    for (int z = _floor.cellBounds.zMin; z < _floor.cellBounds.zMax; z++)
                    {
                        if (CalcChance(x, y) > _chance) continue;
                        
                        var pos = new Vector3Int(x, y, z);
                        TileBase tile = _floor.GetTile(pos);

                        if (tile != null)
                        {
                            if (/*IsNearEdge(pos) || */!ClearToSpawn(pos)) continue;
                            
                            Instantiate(_coalPrefab, pos, Quaternion.identity);
                        }
                    }
                }
            }
        }

        private bool ClearToSpawn(Vector3Int pos)
        {
            var colliders = Physics2D.OverlapCircleAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f), 0.45f);

            return colliders.Length <= 0;
        }

        private bool IsNearEdge(Vector3Int pos)
        {
            var colliders = Physics2D.OverlapCircleAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f), _nearEdgeDetectLength);

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("CaveBorder"))
                    return true;
            }

            return false;
        }

        private float CalcChance(int x, int y)
        {
            float xOffset = Random.Range(0f, 999999f);
            float yOffset = Random.Range(0f, 999999f);

            float xCoord = (float)x / _floor.cellBounds.xMax * _scale + xOffset;
            float yCoord = (float)y / _floor.cellBounds.yMax * _scale + yOffset;

            return Mathf.PerlinNoise(xCoord, yCoord);
        }

    }
}

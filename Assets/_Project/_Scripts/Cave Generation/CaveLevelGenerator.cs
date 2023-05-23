using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace IslandBoy
{
    public class CaveLevelGenerator : MonoBehaviour
    {
        [SerializeField] private Tilemap _caveFloor;
        [SerializeField] private GameObject _stonePrefab;
        [SerializeField] private GameObject _ascendPrefab;
        [SerializeField] private GameObject _descendPrefab;
        [SerializeField] private GameObject _playerPrefab;
        [Header("Generation Parameters")]
        [SerializeField] private float _nearEdgeDetectLength;
        [SerializeField] private float _chance;
        [SerializeField] private float _scale;

        private GameObject _assetHolder;
        private GameObject _ascendGo;

        private void Awake()
        {
            _assetHolder = transform.GetChild(1).transform.gameObject;
        }

        private void Start()
        {
            GenerateCaveLevel();
        }

        public void GenerateButton() => GenerateCaveLevel();

        public void GenerateCaveLevel()
        {
            DestroyAssets();
            InstantiateAscendStairs();
            MovePlayerToSpawnPos();
            GenerateStones();
        }

        private void MovePlayerToSpawnPos()
        {
            Instantiate(_playerPrefab, _ascendGo.transform.position + new Vector3(1.5f, 0f), Quaternion.identity);
        }

        private void GenerateStones()
        {
            for (int x = _caveFloor.cellBounds.xMin; x < _caveFloor.cellBounds.xMax; x++)
            {
                for (int y = _caveFloor.cellBounds.yMin; y < _caveFloor.cellBounds.yMax; y++)
                {
                    for (int z = _caveFloor.cellBounds.zMin; z < _caveFloor.cellBounds.zMax; z++)
                    {
                        if (CalcChance(x, y) > _chance) continue;

                        var pos = new Vector3Int(x, y, z);
                        TileBase tile = _caveFloor.GetTile(pos);

                        if (tile != null)
                        {
                            if (!IsNearEdge(pos) || !ClearToSpawn(pos)) continue;

                            GameObject stoneGo = Instantiate(_stonePrefab, pos, Quaternion.identity);
                            stoneGo.transform.SetParent(_assetHolder.transform);
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

        private void InstantiateAscendStairs()
        {
            Vector2 pos = _caveFloor.cellBounds.center * (Random.insideUnitCircle * 3f);
            Vector2 spawnPos = new(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));

            _ascendGo = Instantiate(_ascendPrefab, spawnPos, Quaternion.identity);
            _ascendGo.transform.SetParent(_assetHolder.transform);

            GameObject descendGo = Instantiate(_descendPrefab, spawnPos + new Vector2(-1f, 0f), Quaternion.identity);
            descendGo.transform.SetParent(_assetHolder.transform);
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

            float xCoord = (float)x / _caveFloor.cellBounds.xMax * _scale + xOffset;
            float yCoord = (float)y / _caveFloor.cellBounds.yMax * _scale + yOffset;

            return Mathf.PerlinNoise(xCoord, yCoord);
        }

        private void DestroyAssets()
        {
            foreach (Transform child in _assetHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}

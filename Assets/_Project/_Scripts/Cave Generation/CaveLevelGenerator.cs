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
        [Header("Generation Parameters")]
        [SerializeField] private float _nearEdgeDetectLength;
        [SerializeField] private float _chance;
        [SerializeField] private float _scale;

        private GameObject _assetHolder;

        private void Awake()
        {
            
        }

        private void Start()
        {
            

        }

        public void Generate()
        {
            _assetHolder = transform.GetChild(1).transform.gameObject;
            Debug.Log("Cave Gen");
            DestroyAssets();
            InstantiateStairs();
            GenerateStones();
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

        private void InstantiateStairs()
        {
            Vector2 pos = _caveFloor.cellBounds.center * (Random.insideUnitCircle * 2.5f);
            Vector2 spawnPos = new(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));

            GameObject ascendGo = Instantiate(_ascendPrefab, spawnPos, Quaternion.identity);
            ascendGo.transform.SetParent(_assetHolder.transform);
            ascendGo.GetComponent<CaveAscend>().Initialize();
            transform.GetComponent<CaveLevel>().EntranceSpawnPoint = ascendGo.transform.position + new Vector3(1.5f, 0f);
            Debug.Log(transform.GetComponent<CaveLevel>().EntranceSpawnPoint);

            GameObject descendGo = Instantiate(_descendPrefab, spawnPos + new Vector2(-2f, 0f), Quaternion.identity);
            descendGo.transform.SetParent(_assetHolder.transform); // temp
            descendGo.GetComponent<CaveDescend>().Initialize();
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

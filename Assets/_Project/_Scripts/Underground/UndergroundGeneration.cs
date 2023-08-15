using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using random = UnityEngine.Random;

namespace IslandBoy
{
    public class UndergroundGeneration : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _ugExitPrefab;
        [SerializeField] private GameObject _ugStaircasePrefab;
        [SerializeField] private GameObject _stonePrefab;
        [SerializeField] private GameObject _coalPrefab;
        [Header("Tiles")]
        [SerializeField] private TileBase _wallTile;
        [SerializeField] private TileBase _floorTile;
        [Header("Events")]
        [SerializeField] private UnityEvent _onGenerateLevel;
        [Header("Tilemap Groups")]
        [SerializeField] private TilemapGroup _oreVeinBp;
        [SerializeField] private TilemapGroup _stoneScatterBp;
        [SerializeField] private TilemapGroup[] _chunkGroups;

        private Tilemap _floorTm;
        private Tilemap _wallTm;
        private List<GameObject> _ugAssets = new();
        private List<Vector2> _usedPositions = new();
        private List<Vector2> _potentialOreVeinPos = new();
        private static Vector2Int[,] _chunkPositions = new Vector2Int[4,4];
        private bool _generationComplete;
        private bool _spawnExitLeftSide;
        private bool _canSpawnStaircase = true;
        private int _lastChunkElement;
        private int _currRowIndex;
        private int _currColIndex;
        private int _direction;
        private int _chunkSideLength;

        private void Awake()
        {
            _floorTm = transform.GetChild(0).GetComponent<Tilemap>();
            _wallTm = transform.GetChild(1).GetComponent<Tilemap>();
            _chunkSideLength = _chunkGroups[0].RandomTilemap.cellBounds.size.x;
        }

        private void OnEnable()
        {
            ExtensionMethods.OnSpawn += RegisterUgAsset;

            // populates the matrix with positions so the first row of the [,] is the first row of the positions etc. makes coding easier
            for (int i = 0; i < _chunkPositions.GetLength(0); i++)
            {
                for (int j = 0; j < _chunkPositions.GetLength(1); j++)
                {
                    _chunkPositions[i, j] = new Vector2Int(j * _chunkSideLength, -i * _chunkSideLength); 
                }
            }

            GenerateNewLevel();
        }

        private void OnDisable()
        {
            ExtensionMethods.OnSpawn -= RegisterUgAsset;
            _canSpawnStaircase = false;
        }

        private void RegisterUgAsset(object newObj)
        {
            GameObject newAsset = (GameObject)newObj;
            _ugAssets.Add(newAsset);
        }

        public void GenerateNewLevel()
        {
            // brand new clean slate
            _generationComplete = false;
            _canSpawnStaircase = false;
            _floorTm.ClearAllTiles();
            _wallTm.ClearAllTiles();
            _usedPositions = new();
            _potentialOreVeinPos = new();
            _spawnExitLeftSide = random.Range(0, 2) == 0;
            _currRowIndex = random.Range(0, 4);
            _currColIndex = 0;
            _onGenerateLevel?.Invoke();

            // destroy all the world structures
            foreach (GameObject asset in _ugAssets)
            {
                Destroy(asset);
            }
            _ugAssets = new();


            // spawn starting room and player
            SpawnChunk(random.Range(0, 2), _chunkPositions[_currRowIndex, _currColIndex]);

            if (_spawnExitLeftSide)
            {
                SpawnPlayer();
            }

            _direction = random.Range(1, 6);

            // generate the new level
            while (_generationComplete == false)
            {
                GenerateTilemap();
            }

            // fill in the rest of the rooms with filler rooms
            List<Vector2> spawnPosCopy = new();

            for (int i = 0; i < _chunkPositions.GetLength(0); i++)
            {
                for (int j = 0; j < _chunkPositions.GetLength(1); j++)
                {
                    spawnPosCopy.Add(_chunkPositions[i, j]);
                }
            }

            List<Vector2> unUsedPositions = new();

            for (int i = 0; i < _chunkPositions.GetLength(0); i++)
            {
                for (int j = 0; j < _chunkPositions.GetLength(1); j++)
                {
                    if(!_usedPositions.Contains(_chunkPositions[i, j]))
                    {
                        unUsedPositions.Add(_chunkPositions[i, j]);
                    }
                }
            }

            foreach (Vector2 pos in unUsedPositions)
            {
                SpawnChunk(4, pos);
            }

            // add a border around the map
            var bounds = _floorTm.cellBounds;
            var offset = 5;

            bounds.xMax += offset;
            bounds.xMin -= offset;
            bounds.yMax += offset;
            bounds.yMin -= offset;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                // separate the floor and wall tiles to their own tilemap
                if (_floorTm.GetTile(pos) == null)
                {
                    _floorTm.SetTile(pos, _floorTile);
                    _wallTm.SetTile(pos, _wallTile);
                }
                else if (_floorTm.GetTile(pos) == _wallTile) 
                {
                    _floorTm.SetTile(pos, _floorTile);
                    _wallTm.SetTile(pos, _wallTile);
                }

                // if there iss an empty space on the wall TM, add it to ore vein position list
                if (_wallTm.GetTile(pos) == null)
                {
                    _potentialOreVeinPos.Add(new Vector2(pos.x, pos.y));
                }
            }

            // generate ores I can try to pick a random position in each chunk, and try to spawn 1 to 2 ore veins in thoughs chunks

            for (int i = 0; i < _chunkPositions.GetLength(0); i++)
            {
                for (int j = 0; j < _chunkPositions.GetLength(1); j++)
                {
                    int shrinkVal = 2;
                    int randXVal = (int)random.Range(_chunkPositions[i, j].x + shrinkVal, _chunkPositions[i, j].x + _chunkSideLength - shrinkVal);
                    int randYVal = (int)random.Range(_chunkPositions[i, j].y + shrinkVal, _chunkPositions[i, j].y + _chunkSideLength - shrinkVal);
                    Vector2Int randSpawnPos = new(randXVal, randYVal);

                    // 75% chance of  spawning an ore vein in a chunk
                    if(random.Range(0, 4) < 3)
                    {
                        GenerateRscClump(_oreVeinBp.RandomTilemap, randSpawnPos, _coalPrefab);
                    }
                    // 100% chance of spawning a stone scatter in a chunk
                    GenerateRscClump(_stoneScatterBp.RandomTilemap, randSpawnPos, _stonePrefab);
                }
            }

            StartCoroutine(Delay());
        }

        // this is bc Destroy() happens near the end of the frame
        private IEnumerator Delay()
        {
            yield return new WaitForEndOfFrame();
            _canSpawnStaircase = true;
        }

        private void GenerateRscClump(Tilemap blueprint, Vector2Int originPos, GameObject objectToSpawn)
        {
            int xMultiplier = random.Range(0, 2) == 0 ? 1 : -1;
            int yMultiplier = random.Range(0, 2) == 0 ? 1 : -1;

            foreach (Vector3Int cellPos in blueprint.cellBounds.allPositionsWithin)
            {
                if (blueprint.GetTile(cellPos) != null)
                {
                    Vector2 spawnPos = new(originPos.x + (cellPos.x * xMultiplier), originPos.y + (cellPos.y * yMultiplier));
                    Vector3Int position = Vector3Int.FloorToInt(spawnPos);

                    if (_wallTm.GetTile(position) == null && IsTileClear(spawnPos))
                    {
                        SpawnResource(objectToSpawn, spawnPos);
                    }
                }
            }
        }

        private void SpawnResource(GameObject obj, Vector2 spawnPos)
        {
            GameObject rscObject = Instantiate(obj, spawnPos, Quaternion.identity);
            _ugAssets.Add(rscObject);

            rscObject.AddComponent<UndergroundBehavior>();
            rscObject.GetComponent<UndergroundBehavior>().RegisterAsset(() =>
            {
                // on resource destroy
                if (_canSpawnStaircase && random.Range(0, 100) < 50)
                {
                    GameObject staircase = Instantiate(_ugStaircasePrefab, spawnPos, Quaternion.identity);
                    _ugAssets.Add(staircase);

                    if (staircase.TryGetComponent(out UndergroundStaircase us))
                    {
                        us.GoDownAction = GenerateNewLevel;
                    }
                }
            });
        }

        private bool IsTileClear(Vector2 pos)
        {
            var colliders = Physics2D.OverlapCircleAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f), 0.25f);

            return colliders.Length == 0;
        }

        private void GenerateTilemap()
        {
            if(_lastChunkElement == 1 || _lastChunkElement == 3)
            {
                if(_currColIndex == 3)
                {
                    _generationComplete = true;
                    _direction = 0;

                    if(_spawnExitLeftSide == false)
                        SpawnPlayer();
                }
                else
                {
                    _currColIndex++;
                    SpawnChunk(random.Range(1, 4) < 3 ? 2 : 3, _chunkPositions[_currRowIndex, _currColIndex]);
                    _direction = random.Range(1, 5);

                    if (_currRowIndex == 3)
                        _direction = 1;
                    else if (_currRowIndex == 0)
                        _direction = 3;
                }
            }
            else if (_direction == 1 || _direction == 2) // 1 & 2 -> generate room up
            {
                _currRowIndex--;

                if (_currRowIndex == 0 || _currRowIndex < 0)
                {
                    _currRowIndex = 0;
                    _direction = 3;
                    SpawnChunk(1, _chunkPositions[_currRowIndex, _currColIndex]);
                }
                else
                {
                    SpawnChunk(0, _chunkPositions[_currRowIndex, _currColIndex]);
                    _direction = random.Range(1, 4) < 3 ? 1 : 5;
                }
            }
            else if (_direction == 3 || _direction == 4) // 3 & 4 -> generate room down
            {
                _currRowIndex++;

                if(_currRowIndex == 3 || _currRowIndex > 3)
                {
                    _currRowIndex = 3;
                    _direction = 1;
                    SpawnChunk(1, _chunkPositions[_currRowIndex, _currColIndex]);
                }
                else
                {
                    SpawnChunk(0, _chunkPositions[_currRowIndex, _currColIndex]);
                    _direction = random.Range(1, 4) < 3 ? 3 : 5;
                }
            }
            else if (_direction == 5) // 5 -> generate room right.
            {
                if(_lastChunkElement == 1 || _lastChunkElement == 2 || _lastChunkElement == 3)
                {
                    SpawnChunk(random.Range(1, 3) == 1 ? 3 : 2, _chunkPositions[_currRowIndex, _currColIndex]);
                }
                else
                {
                    SpawnChunk(1, _chunkPositions[_currRowIndex, _currColIndex]);
                }
            }
        }

        private void SpawnPlayer()
        {
            // spawn exit on the center of the first spawn chunk
            var spawnPos = new Vector2(_chunkPositions[_currRowIndex, _currColIndex].x + (_chunkSideLength / 2), _chunkPositions[_currRowIndex, _currColIndex].y + (_chunkSideLength / 2));
            _ugAssets.Add(Instantiate(_ugExitPrefab, spawnPos, Quaternion.identity));

            GameObject player = GameObject.Find("Player");

            if (player != null)
            {
                player.transform.SetPositionAndRotation(new Vector2(spawnPos.x + 0.5f, spawnPos.y - 1), Quaternion.identity);
            }
        }

        private void SpawnChunk(int roomType, Vector2 spawnPos)
        {
            Tilemap tilemap = _chunkGroups[roomType].RandomTilemap;
            BoundsInt area = tilemap.cellBounds;
            TileBase[] tiles = tilemap.GetTilesBlock(area);
            area = new BoundsInt(Vector3Int.FloorToInt(spawnPos), area.size);
            
            _floorTm.SetTilesBlock(area, tiles);
            _lastChunkElement = roomType;

            if(!_usedPositions.Contains(spawnPos))
                _usedPositions.Add(spawnPos);
        }
    }

    [Serializable]
    public class TilemapGroup
    {
        [SerializeField] private Tilemap[] _group;

        public Tilemap RandomTilemap { get { return _group[random.Range(0, _group.Length)]; } }
    }
}

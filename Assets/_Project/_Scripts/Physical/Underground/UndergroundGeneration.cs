using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using random = UnityEngine.Random;
using Object = UnityEngine.Object;

namespace IslandBoy
{
    public class UndergroundGeneration : MonoBehaviour
    {
        [SerializeField] private TilemapReferences _tmr;
        [Header("Prefabs")]
        [SerializeField] private GameObject _ugExitPrefab;
        [SerializeField] private GameObject _ugStaircasePrefab;
        [SerializeField] private GameObject _stonePrefab;
        [SerializeField] private GameObject _ironOrePrefab;
        [SerializeField] private GameObject _coalOrePrefab;
        [Header("Tiles")]
        [SerializeField] private TileBase _wallTile;
        [SerializeField] private TileBase _floorTile;
        [Header("Events")]
        [SerializeField] private UnityEvent _onGenerateLevel;
        [SerializeField] private UnityEvent<Vector2> _onResourceBroken;
        [Header("Tilemap Groups")]
        [SerializeField] private TilemapGroup _oreVeinBp;
        [SerializeField] private TilemapGroup _stoneScatterBp;
        [SerializeField] private TilemapGroup[] _chunkGroups;

        private List<GameObject> _ugAssets = new();
        private List<Vector2> _usedPositions = new(), _potentialOreVeinPos = new();
        private Vector2Int[,] _chunkPositions = new Vector2Int[4,4];
        private Vector2 _leftSideSpawnPos, _rightSideSpawnPos, _playerSpawnPos;
        private bool _generationComplete, _spawnExitLeftSide;
        private int _lastChunkElement, _currRowIndex, _currColIndex, _direction, _chunkSideLength;

        private void Awake()
        {
            _chunkSideLength = _chunkGroups[0].RandomTilemap.cellBounds.size.x;
        }

        private void OnEnable()
        {
            ExtensionMethods.OnSpawnObject += RegisterUgAsset;

            SpawnPlayer();
        }

        private void OnDisable()
        {
            ExtensionMethods.OnSpawnObject -= RegisterUgAsset;
        }

        private void Start()
        {
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

        private void RegisterUgAsset(object obj, Object go)
        {
            GameObject newAsset = (GameObject)go;
            _ugAssets.Add(newAsset);
        }

        public void GenerateNewLevel()
        {
            ResetVariables();
            SpawnPlayerAndStartingRoom();
            DrawCave();
            DrawFillerRooms();
            DrawBorder();

            StartCoroutine(EndOfFrame()); // this is bc Destroy() happens near the end of the frame
        }

        private void ResetVariables()
        {
            // brand new clean slate
            _generationComplete = false;
            _tmr.GroundTilemap.ClearAllTiles();
            _tmr.WallTilemap.ClearAllTiles();
            _usedPositions = new();
            _potentialOreVeinPos = new();
            _spawnExitLeftSide = random.Range(0, 2) == 0;
            _currRowIndex = random.Range(0, 4);
            _currColIndex = 0;

            // destroy all the world structures
            foreach (GameObject asset in _ugAssets)
            {
                Destroy(asset);
            }
            _ugAssets = new();
        }

        private void SpawnPlayerAndStartingRoom()
        {
            // spawn starting room and player
            SpawnChunk(random.Range(0, 2), _chunkPositions[_currRowIndex, _currColIndex]);

            _leftSideSpawnPos = new Vector2(_chunkPositions[_currRowIndex, _currColIndex].x + (_chunkSideLength / 2),
                        _chunkPositions[_currRowIndex, _currColIndex].y + (_chunkSideLength / 2));

            _direction = random.Range(1, 6);
        }

        private void DrawCave()
        {
            // generate the new level
            while (_generationComplete == false)
            {
                DrawTilemap();
            }
        }

        private void DrawFillerRooms()
        {
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
                    if (!_usedPositions.Contains(_chunkPositions[i, j]))
                    {
                        unUsedPositions.Add(_chunkPositions[i, j]);
                    }
                }
            }

            foreach (Vector2 pos in unUsedPositions)
            {
                SpawnChunk(4, pos);
            }
        }

        private void DrawBorder()
        {
            // add a border around the map
            var bounds = _tmr.GroundTilemap.cellBounds;
            var offset = 5;

            bounds.xMax += offset;
            bounds.xMin -= offset;
            bounds.yMax += offset;
            bounds.yMin -= offset;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                // separate the floor and wall tiles to their own tilemap
                if (_tmr.GroundTilemap.GetTile(pos) == null)
                {
                    _tmr.GroundTilemap.SetTile(pos, _floorTile);
                    _tmr.WallTilemap.SetTile(pos, _wallTile);
                }
                else if (_tmr.GroundTilemap.GetTile(pos) == _wallTile)
                {
                    _tmr.GroundTilemap.SetTile(pos, _floorTile);
                    _tmr.WallTilemap.SetTile(pos, _wallTile);
                }

                // if there iss an empty space on the wall TM, add it to ore vein position list
                if (_tmr.WallTilemap.GetTile(pos) == null)
                {
                    _potentialOreVeinPos.Add(new Vector2(pos.x, pos.y));
                }
            }
        }

        private void GenerateOres()
        {
            // generate ores I can try to pick a random position in each chunk, and try to spawn 1 to 2 ore veins in thoughs chunks

            for (int i = 0; i < _chunkPositions.GetLength(0); i++)
            {
                for (int j = 0; j < _chunkPositions.GetLength(1); j++)
                {
                    if (random.Range(0, 2) == 0)
                        GenerateRscClump(_oreVeinBp.RandomTilemap, RandomChunkPosition(i, j), _ironOrePrefab);

                    GenerateRscClump(_oreVeinBp.RandomTilemap, RandomChunkPosition(i, j), _coalOrePrefab, true);
                    GenerateRscClump(_stoneScatterBp.RandomTilemap, RandomChunkPosition(i, j), _stonePrefab, true);
                }
            }
        }

        private Vector2Int RandomChunkPosition(int i, int j)
        {
            int shrinkVal = 2;
            var position = _chunkPositions[i, j];
            int randXVal = (int)random.Range(position.x + shrinkVal, position.x + _chunkSideLength - shrinkVal);
            int randYVal = (int)random.Range(position.y + shrinkVal, position.y + _chunkSideLength - shrinkVal);
            Vector2Int randSpawnPos = new(randXVal, randYVal);

            return randSpawnPos;
        }

        private IEnumerator EndOfFrame() 
        {
            yield return new WaitForEndOfFrame();

            SpawnExitPrefab(_spawnExitLeftSide ? _leftSideSpawnPos : _rightSideSpawnPos);
            SpawnStaircasePrefab(_spawnExitLeftSide ? _rightSideSpawnPos : _leftSideSpawnPos);
            SpawnPlayer();
            GenerateOres();
            _onGenerateLevel?.Invoke();
        }

        private void GenerateRscClump(Tilemap blueprint, Vector2Int originPos, GameObject objectToSpawn, bool curtailSpawn = false)
        {
            int xMultiplier = random.Range(0, 1) == 0 ? 1 : -1;
            int yMultiplier = random.Range(0, 1) == 0 ? 1 : -1;

            foreach (Vector3Int cellPos in blueprint.cellBounds.allPositionsWithin)
            {
                if (blueprint.GetTile(cellPos) != null)
                {
                    if (random.Range(0, 4) == 0 && curtailSpawn)
                        continue;

                    Vector2 position = new(originPos.x + (cellPos.x * xMultiplier), originPos.y + (cellPos.y * yMultiplier));
                    Vector3Int spawnPos = Vector3Int.FloorToInt(position);

                    if (_tmr.WallTilemap.GetTile(spawnPos) == null && IsTileClear(spawnPos))
                    {
                        SpawnResource(objectToSpawn, spawnPos);
                    }
                }
            }
        }

        private void SpawnResource(GameObject obj, Vector3Int spawnPos)
        {
            GameObject rscObject = Instantiate(obj, spawnPos, Quaternion.identity);
            _ugAssets.Add(rscObject);

            rscObject.AddComponent<UndergroundAsset>();
            rscObject.GetComponent<UndergroundAsset>().RegisterAsset(() =>
            {
                // on resource destroy, update the pathfinding map
                var pos = new Vector2(spawnPos.x, spawnPos.y);
                _onResourceBroken?.Invoke(pos);
            });
        }

        private bool IsTileClear(Vector3Int pos)
        {
            var colliders = Physics2D.OverlapCircleAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f), 0.25f);

            return colliders.Length <= 0;
        }

        private void DrawTilemap()
        {
            if(_lastChunkElement == 1 || _lastChunkElement == 3)
            {
                if(_currColIndex == 3)
                {
                    _generationComplete = true;
                    _direction = 0;
                    _rightSideSpawnPos = new Vector2(_chunkPositions[_currRowIndex, _currColIndex].x + (_chunkSideLength / 2), 
                        _chunkPositions[_currRowIndex, _currColIndex].y + (_chunkSideLength / 2));
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

        private void SpawnExitPrefab(Vector2 spawnPos)
        {
            _ugAssets.Add(Instantiate(_ugExitPrefab, spawnPos, Quaternion.identity));
            _playerSpawnPos = new Vector2(spawnPos.x + 0.5f, spawnPos.y - 1);
        }

        private void SpawnStaircasePrefab(Vector2 spawnPos)
        {
            _ugAssets.Add(Instantiate(_ugStaircasePrefab, spawnPos, Quaternion.identity));
        }

        private void SpawnPlayer()
        {
            if (_playerSpawnPos != Vector2.zero)
            {
                GameObject player = GameObject.Find("Player");

                if (player != null)
                {
                    player.transform.SetPositionAndRotation(_playerSpawnPos, Quaternion.identity);
                }
            }
        }

        private void SpawnChunk(int roomType, Vector2 spawnPos)
        {
            Tilemap tilemap = _chunkGroups[roomType].RandomTilemap;
            BoundsInt area = tilemap.cellBounds;
            TileBase[] tiles = tilemap.GetTilesBlock(area);
            area = new BoundsInt(Vector3Int.FloorToInt(spawnPos), area.size);
            
            _tmr.GroundTilemap.SetTilesBlock(area, tiles);
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

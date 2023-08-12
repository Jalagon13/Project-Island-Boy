using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using random = UnityEngine.Random;

namespace IslandBoy
{
    public class UndergroundGeneration : MonoBehaviour
    {
        [SerializeField] private GameObject _ugExitPrefab;
        [SerializeField] private GameObject _stonePrefab;
        [SerializeField] private TileBase _wallTile;
        [SerializeField] private TileBase _floorTile;
        [SerializeField] private TilemapGroup _oreVeinBlueprints;
        [SerializeField] private TilemapGroup[] _chunkGroups;

        private Tilemap _floorTm;
        private Tilemap _wallTm;
        private List<GameObject> _ugAssets = new();
        private List<Vector2> _usedPositions = new();
        private List<Vector2> _potentialOreVeinPos = new();
        private static Vector2[,] _spawnPositions = new Vector2[4,4];
        private bool _generationComplete;
        private bool _spawnExitLeftSide;
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
            // populates the matrix with positions so the first row of the [,] is the first row of the positions etc. makes coding easier
            for (int i = 0; i < _spawnPositions.GetLength(0); i++)
            {
                for (int j = 0; j < _spawnPositions.GetLength(1); j++)
                {
                    _spawnPositions[i, j] = new Vector2(j * _chunkSideLength, -i * _chunkSideLength); 
                }
            }

            GenerateNewLevel();
        }

        public void GenerateNewLevel()
        {
            // brand new clean slate
            _generationComplete = false;
            _floorTm.ClearAllTiles();
            _wallTm.ClearAllTiles();
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

            SpawnChunk(random.Range(0, 2), _spawnPositions[_currRowIndex, _currColIndex]);

            if (_spawnExitLeftSide)
                SpawnPlayer();

            _direction = random.Range(1, 6);

            // generate the new level
            while (_generationComplete == false)
            {
                GenerateLevel();
            }

            // fill in the rest of the rooms with filler rooms
            List<Vector2> spawnPosCopy = new();

            for (int i = 0; i < _spawnPositions.GetLength(0); i++)
            {
                for (int j = 0; j < _spawnPositions.GetLength(1); j++)
                {
                    spawnPosCopy.Add(_spawnPositions[i, j]);
                }
            }

            List<Vector2> unUsedPositions = new();

            for (int i = 0; i < _spawnPositions.GetLength(0); i++)
            {
                for (int j = 0; j < _spawnPositions.GetLength(1); j++)
                {
                    if(!_usedPositions.Contains(_spawnPositions[i, j]))
                    {
                        unUsedPositions.Add(_spawnPositions[i, j]);
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
                    _potentialOreVeinPos.Add(new Vector2(pos.x, pos.y));

            }

            // generate ores
            Vector2 originPos = _potentialOreVeinPos[random.Range(0, _potentialOreVeinPos.Count)];
            var oreBlueprint = _oreVeinBlueprints.RandomTilemap;
            int xMultiplier = random.Range(0, 2) == 0 ? 1 : -1;
            int yMultiplier = random.Range(0, 2) == 0 ? 1 : -1;

            foreach (Vector3Int pos in oreBlueprint.cellBounds.allPositionsWithin)
            {
                if(oreBlueprint.GetTile(pos) != null)
                {
                    Vector2 spawnPos = originPos + new Vector2(pos.x * xMultiplier, pos.y * yMultiplier);

                    if (_wallTm.GetTile(Vector3Int.FloorToInt(spawnPos)) == null)
                        _ugAssets.Add(Instantiate(_stonePrefab, spawnPos, Quaternion.identity));
                }
            }
        }

        private void GenerateLevel()
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
                    SpawnChunk(random.Range(1, 4) < 3 ? 2 : 3, _spawnPositions[_currRowIndex, _currColIndex]);
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
                    SpawnChunk(1, _spawnPositions[_currRowIndex, _currColIndex]);
                }
                else
                {
                    SpawnChunk(0, _spawnPositions[_currRowIndex, _currColIndex]);
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
                    SpawnChunk(1, _spawnPositions[_currRowIndex, _currColIndex]);
                }
                else
                {
                    SpawnChunk(0, _spawnPositions[_currRowIndex, _currColIndex]);
                    _direction = random.Range(1, 4) < 3 ? 3 : 5;
                }
            }
            else if (_direction == 5) // 5 -> generate room right.
            {
                if(_lastChunkElement == 1 || _lastChunkElement == 2 || _lastChunkElement == 3)
                {
                    SpawnChunk(random.Range(1, 3) == 1 ? 3 : 2, _spawnPositions[_currRowIndex, _currColIndex]);
                }
                else
                {
                    SpawnChunk(1, _spawnPositions[_currRowIndex, _currColIndex]);
                }
            }
        }

        private void SpawnPlayer()
        {
            // spawn exit on the center of the first spawn chunk
            var spawnPos = new Vector2(_spawnPositions[_currRowIndex, _currColIndex].x + (_chunkSideLength / 2), _spawnPositions[_currRowIndex, _currColIndex].y + (_chunkSideLength / 2));
            _ugAssets.Add(Instantiate(_ugExitPrefab, spawnPos, Quaternion.identity));

            GameObject player = GameObject.Find("Player");
            if (player != null)
                player.transform.SetPositionAndRotation(new Vector2(spawnPos.x + 0.5f, spawnPos.y - 1), Quaternion.identity);
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

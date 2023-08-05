using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using random = UnityEngine.Random;

namespace IslandBoy
{
    public class UndergroundGeneration : MonoBehaviour
    {
        [SerializeField] private int _chunkSideLength = 12;
        [SerializeField] private TileBase _stoneTile;
        [SerializeField] private Tilemap[] _caveChunkPrefabs;

        private Tilemap _undergroundTm;
        private List<Vector2> _usedPositions = new();
        private static Vector2[,] _spawnPositions = new Vector2[4,4];
        private bool _generationComplete;
        private int _lastChunkElement;
        private int _currRowIndex;
        private int _currColIndex;
        private int _direction;

        private void Awake()
        {
            _undergroundTm = transform.GetChild(0).GetComponent<Tilemap>();
        }

        private void Start()
        {
            // populates the matrix with positions so the first row of the [,] is the first row of the positions etc. makes coding easier
            for (int i = 0; i < _spawnPositions.GetLength(0); i++)
            {
                for (int j = 0; j < _spawnPositions.GetLength(1); j++)
                {
                    _spawnPositions[i, j] = new Vector2(j * _chunkSideLength, -i * _chunkSideLength); 
                }
            }
        }

        public void GenerateNewLevel()
        {
            _generationComplete = false;
            _undergroundTm.ClearAllTiles();
            _currRowIndex = random.Range(0, 4);
            _currColIndex = 0;
            _usedPositions = new();

            SpawnChunk(random.Range(0, 2), _spawnPositions[_currRowIndex, _currColIndex]);

            _direction = random.Range(1, 6);

            while (_generationComplete == false)
            {
                TryGenerateNextRoom();
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
            var bounds = _undergroundTm.cellBounds;
            var offset = 5;

            bounds.xMax += offset;
            bounds.xMin -= offset;
            bounds.yMax += offset;
            bounds.yMin -= offset;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (_undergroundTm.GetTile(pos) == null)
                {
                    _undergroundTm.SetTile(pos, _stoneTile);
                }
            }
        }

        private void TryGenerateNextRoom()
        {
            if(_lastChunkElement == 1 || _lastChunkElement == 3)
            {
                if(_currColIndex == 3)
                {
                    _generationComplete = true;
                    _direction = 0;
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

                return;
            }
            else if (_direction == 1 || _direction == 2) // 1 & 2 -> generate room up
            {
                _currRowIndex--;

                if (_currRowIndex == 0 || _currRowIndex < 0)
                {
                    _currRowIndex = 0;

                    SpawnChunk(1, _spawnPositions[_currRowIndex, _currColIndex]);
                    _direction = 3;
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

                    SpawnChunk(1, _spawnPositions[_currRowIndex, _currColIndex]);
                    _direction = 1;
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

        private void SpawnChunk(int roomType, Vector2 spawnPos)
        {
            BoundsInt area = _caveChunkPrefabs[roomType].cellBounds;
            TileBase[] tiles = _caveChunkPrefabs[roomType].GetTilesBlock(area);
            area = new BoundsInt(Vector3Int.FloorToInt(spawnPos), area.size);

            _undergroundTm.SetTilesBlock(area, tiles);
            _lastChunkElement = roomType;

            if(!_usedPositions.Contains(spawnPos))
                _usedPositions.Add(spawnPos);
        }
    }
}

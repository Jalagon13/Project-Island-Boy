using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using random = UnityEngine.Random;

namespace IslandBoy
{
    public class UndergroundGeneration : MonoBehaviour
    {
        [SerializeField] private Tilemap _testTmPrefab;
        [SerializeField] private int _chunkSideLength = 12;

        private Tilemap _undergroundTm;
        private List<Vector2> _backlog = new();
        private static Vector2[,] _chunkSpawnPositions = new Vector2[4,4];
        private int _currentRowIndex;
        private int _currentColIndex;
        private int _genDirection;
        private bool _generationComplete;

        private void Awake()
        {
            _undergroundTm = transform.GetChild(0).GetComponent<Tilemap>();
        }

        private void Start()
        {
            // populates the matrix with positions so the first row of the [,] is the first row of the positions etc. makes coding easier
            for (int i = 0; i < _chunkSpawnPositions.GetLength(0); i++)
            {
                for (int j = 0; j < _chunkSpawnPositions.GetLength(1); j++)
                {
                    _chunkSpawnPositions[i, j] = new Vector2(j * _chunkSideLength, -i * _chunkSideLength); 
                }
            }
        }

        public void GenerateLevel()
        {
            _generationComplete = false;

            _undergroundTm.ClearAllTiles();

            _backlog = new();

            _currentRowIndex = random.Range(0, 4);
            _currentColIndex = 0;

            Vector2 firstRoomSpawnPos = _chunkSpawnPositions[_currentRowIndex, _currentColIndex];
            SpawnChunk(_testTmPrefab, firstRoomSpawnPos);

            _backlog.Add(firstRoomSpawnPos);

            _genDirection = random.Range(1, 6);

            while (_generationComplete == false)
            {
                TryGenerateNextRoom();
            }
        }

        private void TryGenerateNextRoom()
        {
            // 1 & 2 -> generate room up, 3 & 4 -> generate room down, 5 -> generate room right.
            if (_genDirection == 1 || _genDirection == 2)
            {
                // check if spawn pos exists
                if (_currentRowIndex == 0)
                {
                    // row does not exists, generate room either down or right
                    _genDirection = random.Range(3, 6);
                    Debug.Log($"Can't generate up room, try again. New Gen Direction: {_genDirection}");
                    return;
                }
                else
                {
                    // move row index "up"
                    _currentRowIndex--;

                    if (_backlog.Contains(_chunkSpawnPositions[_currentRowIndex, _currentColIndex]))
                    {
                        _genDirection = random.Range(3, 6);
                        Debug.Log($"Can't generate up room bc room is already taken, try again. New Gen Direction: {_genDirection}");
                        _currentRowIndex++;
                        return;
                    }
                }
            }
            else if (_genDirection == 3 || _genDirection == 4)
            {
                if(_currentRowIndex == 3)
                {
                    int temp = random.Range(1, 4);

                    if (temp == 1 || temp == 2)
                        _genDirection = 1;
                    else if(temp == 3)
                        _genDirection = 5;

                    Debug.Log($"Can't generate down room, try again. New Gen Direction: {_genDirection}");
                    return;
                }
                else
                {
                    _currentRowIndex++;

                    if (_backlog.Contains(_chunkSpawnPositions[_currentRowIndex, _currentColIndex]))
                    {
                        int temp = random.Range(1, 4);

                        if (temp == 1 || temp == 2)
                            _genDirection = 1;
                        else if (temp == 3)
                            _genDirection = 5;

                        Debug.Log($"Can't generate down room bc room is already taken, try again. New Gen Direction: {_genDirection}");
                        _currentRowIndex--;
                        return;
                    }
                }
            }
            else if (_genDirection == 5)
            {
                if(_currentColIndex == 3)
                {
                    Debug.Log("Can't genereate right room. Algorithm done");
                    _generationComplete = true;
                    return;
                }
                else
                {
                    _currentColIndex++;
                }
            }

            SpawnChunk(_testTmPrefab, _chunkSpawnPositions[_currentRowIndex, _currentColIndex]);
            _backlog.Add(_chunkSpawnPositions[_currentRowIndex, _currentColIndex]);
            _genDirection = random.Range(1, 6);
            Debug.Log($"Room Generated! Next Generation Direction: {_genDirection}");
        }

        private void SpawnChunk(Tilemap tm, Vector2 spawnPos)
        {
            BoundsInt area = tm.cellBounds;
            TileBase[] tiles = tm.GetTilesBlock(area);

            area = new BoundsInt(Vector3Int.FloorToInt(spawnPos), area.size);
            _undergroundTm.SetTilesBlock(area, tiles);
        }
    }
}

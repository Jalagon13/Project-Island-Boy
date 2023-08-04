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
        [SerializeField] private Tilemap[] _caveChunkPrefabs;

        private Tilemap _undergroundTm;
        private List<Vector2> _backlog = new();
        private static Vector2[,] _spawnPositions = new Vector2[4,4];
        private int _currRowIndex;
        private int _currColIndex;
        private int _direction;
        private int _lastChunkElement;
        private bool _generationComplete;

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

            Vector2 firstRoomSpawnPos = _spawnPositions[_currRowIndex, _currColIndex];
            SpawnChunk(random.Range(0, 2), firstRoomSpawnPos);

            _direction = random.Range(1, 6);

            StartCoroutine(Generate());

            // fill in the rest of the rooms with filler rooms
        }

        private IEnumerator Generate()
        {
            int counter = 0;

            while (_generationComplete == false)
            {
                yield return new WaitForSeconds(0.75f);
                TryGenerateNextRoom();
                counter++;
                if (counter > 100)
                {
                    Debug.Log("counter went over 100");
                    break;
                }
            }
        }

        private void TryGenerateNextRoom()
        {
            // if last room was T1, place next room either T1 or T2 then start algorithm again.
            if(_lastChunkElement == 1 || _lastChunkElement == 3)
            {
                // if on the last column, 
                if(_currColIndex == 3)
                {
                    // stop algorithm
                    Debug.Log("Done");
                    _generationComplete = true;
                }
                else
                {
                    // place either T1 or T2 then start algorithm again
                    _currColIndex++;
                    SpawnChunk(random.Range(1, 3) == 1 ? 3 : 2, _spawnPositions[_currRowIndex, _currColIndex]);
                    _direction = random.Range(1, 6);
                }

                return;
            }
            else if (_direction == 1 || _direction == 2) // 1 & 2 -> generate room up
            {
                // move row index "up"
                _currRowIndex--;

                if (_currRowIndex == 0 || _currRowIndex < 0)
                {
                    _currRowIndex = 0;

                    // spawn a T1 room
                    SpawnChunk(1, _spawnPositions[_currRowIndex, _currColIndex]);
                    return;
                }
                else
                {
                    SpawnChunk(0, _spawnPositions[_currRowIndex, _currColIndex]);
                    _direction = random.Range(1, 4) < 3 ? 1 : 5;
                    return;
                }
            }
            else if (_direction == 3 || _direction == 4) // 3 & 4 -> generate room down
            {
                _currRowIndex++;

                if(_currRowIndex == 3 || _currRowIndex > 3)
                {
                    _currRowIndex = 3;

                    // spawn a T1 room
                    SpawnChunk(1, _spawnPositions[_currRowIndex, _currColIndex]);
                    return;
                }
                else
                {
                    SpawnChunk(0, _spawnPositions[_currRowIndex, _currColIndex]);
                    _direction = random.Range(1, 4) < 3 ? 3 : 5;
                    return;
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
                
                return;

            }
        }

        private void SpawnChunk(int roomType, Vector2 spawnPos)
        {
            BoundsInt area = _caveChunkPrefabs[roomType].cellBounds;
            TileBase[] tiles = _caveChunkPrefabs[roomType].GetTilesBlock(area);

            area = new BoundsInt(Vector3Int.FloorToInt(spawnPos), area.size);

            _undergroundTm.SetTilesBlock(area, tiles);
            _lastChunkElement = roomType;
        }
    }
}

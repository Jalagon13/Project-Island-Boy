using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class IslandManager : Singleton<IslandManager>
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _minerNpcPrefab;
        [SerializeField] private GameObject _crabMobPrefab;
        [SerializeField] private GameObject _sandPilePrefab;
        [SerializeField] private GameObject _grassPilePrefab;
        [SerializeField] private TileBase _sandTile;
        [SerializeField] private TileBase _grassTile;
        [SerializeField] private Tilemap _islandTm;
        [SerializeField] private Tilemap _floorTm;
        [SerializeField] private Tilemap _wallsTm;
        [SerializeField] private LootTable _seaLoot;

        private List<Vector2> _bedPositions = new();
        private List<GameObject> _crabMobs = new();
        private bool _minerNpcSpawned;

        private IEnumerator Start()
        {
            foreach (KeyValuePair<ItemObject, int> loot in _seaLoot.Loot())
            {
                yield return new WaitForSeconds(Random.Range(1f, 5f));

                if (SceneManager.GetActiveScene().buildIndex != 0)
                    break;

                float randomX = Random.value;
                float randomY = Random.value;

                if (Random.value < 0.5f)
                    randomX = randomX < 0.5f ? -0.1f : 1.1f;
                else
                    randomY = randomY < 0.5f ? -0.1f : 1.1f;

                Vector2 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector2(randomX, randomY));

                var itemGo = WorldItemManager.Instance.SpawnItem(spawnPosition, loot.Key, loot.Value, null, false);
                itemGo.AddComponent<ItemSeaWander>().StartWander(_islandTm);
            }

            StartCoroutine(Start());
        }

        public void PushBedPosition(Vector2 pos)
        {
            if (!_bedPositions.Contains(pos))
                _bedPositions.Add(pos);
        }

        public void PopBedPosition(Vector2 pos)
        {
            if (_bedPositions.Contains(pos))
                _bedPositions.Remove(pos);
        }

        public void CheckBedsForHousing()
        {
            foreach (Vector2 pos in _bedPositions)
            {
                CheckHousing(pos);
            }
        }

        public void SpawnPiles()
        {
            SpawnPile(_sandTile, _sandPilePrefab);
            SpawnPile(_grassTile, _grassPilePrefab);
        }

        private void SpawnPile(TileBase tileToLookFor, GameObject pilePrefab)
        {
            BoundsInt bounds = _islandTm.cellBounds;

            List<Vector3Int> clearTilePos = new();

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (_islandTm.GetTile(pos) == tileToLookFor && TileAreaClear(pos))
                {
                    clearTilePos.Add(pos);
                }
            }

            for(int i = 0; i < clearTilePos.Count; i++)
            {
                var pos = clearTilePos[Random.Range(0, clearTilePos.Count)];

                if (TileAreaClear(pos) && !IsNear(pilePrefab, pos, 3f) && !_wallsTm.HasTile(pos) && !_floorTm.HasTile(pos))
                {
                    var go = Instantiate(pilePrefab, pos, Quaternion.identity);
                    go.name = pilePrefab.name;
                }
            }
            
        }

        private bool IsNear(GameObject prefab, Vector3Int pos, float radiusToCheck)
        {
            var colliders = Physics2D.OverlapCircleAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f), radiusToCheck);

            foreach (var collider in colliders)
            {
                if (collider.gameObject.name == prefab.name)
                    return true;
            }

            return false;
        }

        public void SpawnCrabs()
        {
            foreach (GameObject crab in _crabMobs)
                Destroy(crab);

            _crabMobs.Clear();

            BoundsInt bounds = _islandTm.cellBounds;

            List<Vector3Int> sandTiles = new();

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (_islandTm.HasTile(pos) && _islandTm.GetTile(pos) == _sandTile)
                {
                    sandTiles.Add(pos);
                }
            }

            if (sandTiles.Count < 40) return;

            int crabMax = sandTiles.Count / 40;
            int crabCount = 0;

            while(crabCount < crabMax)
            {
                var randPos = sandTiles[Random.Range(0, sandTiles.Count)];

                if (TileAreaClear(randPos))
                {
                    var crab = Instantiate(_crabMobPrefab, randPos, Quaternion.identity);

                    _crabMobs.Add(crab);

                    crabCount++;
                }
            }
        }

        private bool TileAreaClear(Vector3Int pos)
        {
            var colliders = Physics2D.OverlapCircleAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f), 0.25f);

            foreach (var collider in colliders)
            {
                if(collider.gameObject.layer == 3)
                    return false;
            }

            return true;
        }

        private void CheckHousing(Vector2 startPos)
        {
            Stack<Vector3Int> tiles = new();
            List<Vector3Int> floorTilePositions = new(); // list of positions of tile that have a floor and no wall or door on it.
            tiles.Push(Vector3Int.FloorToInt(startPos));

            // checks if this is an enclosed space with flooring everywhere
            while (tiles.Count > 0)
            {
                var p = Vector3Int.FloorToInt(tiles.Pop());

                if (_floorTm.HasTile(p))
                {
                    if (_wallsTm.HasTile(p) || HasDoor(p))
                    {
                        continue;
                    }
                    else if (!floorTilePositions.Contains(p))
                    {
                        floorTilePositions.Add(p);
                        tiles.Push(new Vector3Int(p.x - 1, p.y));
                        tiles.Push(new Vector3Int(p.x + 1, p.y));
                        tiles.Push(new Vector3Int(p.x, p.y - 1));
                        tiles.Push(new Vector3Int(p.x, p.y + 1));
                    }
                }
                else if (_wallsTm.HasTile(p) || HasDoor(p))
                {
                    continue;
                }
                else
                {
                    Debug.Log($"There is an empty space at {p} therefore {startPos} is not a house");
                    return;
                }
            }

            // first check how many free floor spaces are there in the floorTilePositions list
            List<Vector3Int> clearFloorTiles = new();
            foreach (var pos in floorTilePositions)
            {
                if (TileAreaClear(pos))
                {
                    clearFloorTiles.Add(pos);
                }
            }

            // if minimum number of clear floor tiles is below the set minimum, this is not a valid housing
            int minClearFloorTileAmount = 5;
            if (clearFloorTiles.Count < minClearFloorTileAmount)
            {
                Debug.Log($"Not enough free space in this closed area (clear floor tiles count: " +
                    $"{clearFloorTiles.Count} | minimum clear tiles needed: {minClearFloorTileAmount}) therefore {startPos} is not a house");
                return;
            }

            // find a random position of the clear floor tiles and spawn the NPC there
            if (_minerNpcSpawned) return;

            var randFloorTile = clearFloorTiles[Random.Range(0, clearFloorTiles.Count)];
            var spawnNpcPosition = new Vector2(randFloorTile.x + 0.5f, randFloorTile.y + 0.5f);

            Debug.Log($"{startPos} is valid housing! # of spaces: {clearFloorTiles.Count}");
            Instantiate(_minerNpcPrefab, spawnNpcPosition, Quaternion.identity);
            _minerNpcSpawned = true;
            return;
        }

        private bool HasDoor(Vector3Int pos)
        {
            var centerPos = new Vector2(pos.x + 0.5f, pos.y + 0.5f);
            var colliders = Physics2D.OverlapCircleAll(centerPos, 0.1f);

            foreach (Collider2D col in colliders)
            {
                if (col.TryGetComponent(out Door door))
                    return true;
            }

            return false;
        }
    }
}

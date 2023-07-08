using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        [SerializeField] private Tilemap _island;
        [SerializeField] private Tilemap _floor;
        [SerializeField] private Tilemap _walls;
        [SerializeField] private LootTable _seaLoot;

        private List<Vector2> _bedPositions = new();
        private List<GameObject> _crabMobs = new();
        private List<GameObject> _sandTiles = new();

        private IEnumerator Start()
        {
            foreach (KeyValuePair<ItemObject, int> loot in _seaLoot.Loot())
            {
                float randomX = Random.value;
                float randomY = Random.value;

                if (Random.value < 0.5f)
                    randomX = randomX < 0.5f ? -0.1f : 1.1f;
                else
                    randomY = randomY < 0.5f ? -0.1f : 1.1f;

                Vector2 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector2(randomX, randomY));

                var itemGo = WorldItemManager.Instance.SpawnItem(spawnPosition, loot.Key, loot.Value, null, false);
                itemGo.AddComponent<ItemSeaWander>().StartWander(_island);

                yield return new WaitForSeconds(Random.Range(1f, 5f));
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
            foreach (GameObject tile in _sandTiles)
                Destroy(tile);

            _sandTiles.Clear();

            BoundsInt bounds = _island.cellBounds;

            List<Vector3Int> sandTiles = new();

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (_island.HasTile(pos) && _island.GetTile(pos) == _sandTile)
                {
                    sandTiles.Add(pos);
                }
            }

            foreach (var item in sandTiles)
            {
                var randPos = sandTiles[Random.Range(0, sandTiles.Count)];

                if (ClearToSpawn(randPos) && !IsNearRsc(randPos))
                {
                    var tile = Instantiate(_sandPilePrefab, randPos, Quaternion.identity);

                    _sandTiles.Add(tile);
                }
            }
        }

        private bool IsNearRsc(Vector3Int pos)
        {
            var colliders = Physics2D.OverlapCircleAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f), 3.5f);

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out Resource rsc))
                    return true;
            }

            return false;
        }

        public void SpawnCrabs()
        {
            foreach (GameObject crab in _crabMobs)
                Destroy(crab);

            _crabMobs.Clear();

            BoundsInt bounds = _island.cellBounds;

            List<Vector3Int> sandTiles = new();

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (_island.HasTile(pos) && _island.GetTile(pos) == _sandTile)
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

                if (ClearToSpawn(randPos))
                {
                    var crab = Instantiate(_crabMobPrefab, randPos, Quaternion.identity);

                    _crabMobs.Add(crab);

                    crabCount++;
                }
            }
        }

        private bool ClearToSpawn(Vector3Int pos)
        {
            var colliders = Physics2D.OverlapCircleAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f), 0.45f);

            return colliders.Length <= 0;
        }

        private void CheckHousing(Vector2 startPos)
        {
            Stack<Vector3Int> tiles = new();
            List<Vector3Int> validPositions = new();
            tiles.Push(Vector3Int.FloorToInt(startPos));

            while (tiles.Count > 0)
            {
                var p = Vector3Int.FloorToInt(tiles.Pop());
                if (_floor.HasTile(p))
                {
                    if (_walls.HasTile(p) || HasDoor(p))
                    {
                        continue;
                    }
                    else if (!validPositions.Contains(p))
                    {
                        validPositions.Add(p);
                        tiles.Push(new Vector3Int(p.x - 1, p.y));
                        tiles.Push(new Vector3Int(p.x + 1, p.y));
                        tiles.Push(new Vector3Int(p.x, p.y - 1));
                        tiles.Push(new Vector3Int(p.x, p.y + 1));
                    }
                }
                else if (_walls.HasTile(p) || HasDoor(p))
                {
                    continue;
                }
                else
                {
                    Debug.Log($"There is an empty space at {p} therefore {startPos} is not a house");
                    return;
                }
            }

            Debug.Log($"{startPos} is valid housing! # of spaces: {validPositions.Count}");
            Instantiate(_minerNpcPrefab, validPositions[2], Quaternion.identity);
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

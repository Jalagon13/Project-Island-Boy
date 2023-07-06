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
        [SerializeField] private TileBase _sandTile;
        [SerializeField] private Tilemap _island;
        [SerializeField] private Tilemap _floor;
        [SerializeField] private Tilemap _walls;
        [SerializeField] private LootTable _seaLoot;

        private List<Vector2> _bedPositions = new();
        private List<GameObject> _crabMobs = new();

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
                itemGo.AddComponent<ItemSeaWander>().Wander(_pr);

                yield return new WaitForSeconds(Random.Range(3f, 7f));
            }

            StartCoroutine(Start());
        }

        private bool Is01(float a)
        {
            return a > 0 && a < 1;
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

        private void SpawnCrabs()
        {
            foreach (GameObject crab in _crabMobs)
            {
                Destroy(crab);
            }

            _crabMobs.Clear();

            int crabCounter = 0;
            int crabMax = Random.Range(1, 2);

            while (crabCounter < crabMax)
            {
                var xVal = Random.Range(_island.cellBounds.xMin, _island.cellBounds.xMax);
                var yVal = Random.Range(_island.cellBounds.yMin, _island.cellBounds.yMax);
                var pos = Vector3Int.FloorToInt(new Vector2(xVal, yVal));

                if (_island.HasTile(pos) && _island.GetTile(pos) == _sandTile)
                {
                    var crab = Instantiate(_crabMobPrefab, pos, Quaternion.identity);
                    _crabMobs.Add(crab);
                    crabCounter++;
                }
            }
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

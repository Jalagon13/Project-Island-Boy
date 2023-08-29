using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class ItemSeaWander : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Vector2 _dir;
        private float _speed;

        private void OnDisable()
        {
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if (_rb == null) return;

            _rb.MovePosition(_rb.position + _dir * _speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out IslandTag tag))
            {
                StopWander();
                Destroy(gameObject, 0.5f);
            }
        }

        public void StopWander()
        {
            _rb = null;
        }

        public void StartWander(Tilemap islandTilemap)
        {
            _rb = GetComponent<Rigidbody2D>();
            _speed = Random.Range(1f, 2f);

            BoundsInt bounds = islandTilemap.cellBounds;
            Vector3Int totalPosition = Vector3Int.zero;
            int tileCount = 0;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (islandTilemap.HasTile(pos))
                {
                    totalPosition += pos;
                    tileCount++;
                }
            }

            Vector2 averagePosition = new(totalPosition.x / (float)tileCount, totalPosition.y / (float)tileCount);

            var perpOffset = Vector2.Perpendicular((averagePosition - (Vector2)transform.position).normalized);

            if (Random.Range(0f, 1f) < 0.5f)
                perpOffset *= -1;

            var hits = Physics2D.RaycastAll(averagePosition, perpOffset);

            foreach(var hit in hits)
            {
                if(hit.transform.TryGetComponent(out IslandTag islandTag))
                {
                    _dir = ((hit.point + (perpOffset * Random.Range(6f, 8f))) - (Vector2)transform.position).normalized;
                }
            }

            Destroy(gameObject, 60f);
        }
    }
}

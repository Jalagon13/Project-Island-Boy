using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class ResourceGenerator : MonoBehaviour
    {
        [SerializeField] private TilemapReferences _tmr;

        private static List<Resource> _disabledResources = new();

        private void Awake()
        {
            GameSignals.DAY_START.AddListener(RegenerateResources);
        }

        private void OnDestroy()
        {
            _disabledResources = new();

            GameSignals.DAY_START.RemoveListener(RegenerateResources);
        }

        private void RegenerateResources(ISignalParameters parameters)
        {
            // take trees and stones and randomly generate them at the start of the day
            foreach (Resource resource in _disabledResources)
            {
                if (_tmr.FloorTilemap.HasTile(Vector3Int.FloorToInt(resource.transform.position)) ||
                    _tmr.WallTilemap.HasTile(Vector3Int.FloorToInt(resource.transform.position)))
                {
                    Destroy(resource.gameObject);
                }
                else
                {
                    resource.gameObject.SetActive(true);
                }
            }

            _disabledResources = new();
        }

        public bool IsClear(Vector2 pos)
        {
            var colliders = Physics2D.OverlapBoxAll(pos += new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), 0);

            foreach (Collider2D col in colliders)
            {
                if (col.gameObject.layer == 3)
                    return false;
            }

            return true;
        }

        public static void AddToDisabledResources(Resource rsc)
        {
            _disabledResources.Add(rsc);
        }
    }
}

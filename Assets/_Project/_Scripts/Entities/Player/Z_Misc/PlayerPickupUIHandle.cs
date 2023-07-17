using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace IslandBoy
{
    public class PlayerPickupUIHandle : MonoBehaviour
    {
        [SerializeField] private GameObject _itemPickupSignPrefab;

        private Dictionary<ItemObject, ItemPickupSign> _itemPickups = new();
        private bool _processing;

        public void PickupUIHandle(ItemObject item, int amount)
        {
            Debug.Log(item.name + " " + amount);

            GameObject itemPickupSign = Instantiate(_itemPickupSignPrefab);
            ItemPickupSign pickup = itemPickupSign.GetComponent<ItemPickupSign>();

            _itemPickups.Add(item, pickup);
            Debug.Log(_itemPickups.Count);

            if (!_processing)
                ProcessPickupUIs();
        }

        private async void ProcessPickupUIs()
        {
            _processing = true;

            while (_itemPickups.Count > 0)
            {
                await OverlapCheck();
            }

            _processing = false;
        }

        private async Task OverlapCheck()
        {
            var first = _itemPickups.First();
            var ipsGoPosition = first.Value.gameObject.transform.position;

            checkAgain:

            bool foundOverlap = false;
            var colliders = Physics2D.OverlapBoxAll(ipsGoPosition, new Vector2(2f, 0.25f), 0.2f);

            foreach (Collider2D collider in colliders)
            {
                if (!collider.TryGetComponent(out ItemPickupSign ips)) continue;
                if (ips == first.Value) continue;

                ipsGoPosition += new Vector3(0f, 0.5f, 0f);
                foundOverlap = true;
                await Task.Delay(TimeSpan.FromSeconds(0.1f));
                break;
            }

            if (foundOverlap)
                goto checkAgain;

            _itemPickups.Remove(first.Key);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace IslandBoy
{
    public class PlayerPickupUIHandle : MonoBehaviour
    {
        [SerializeField] private GameObject _itemPickupSignPrefab;
        [SerializeField] private AudioClip _popSound;
        [SerializeField] private float _disappearTimer;

        private Dictionary<ItemObject, ItemPickupSign> _itemPickups;
        private bool _processing;

        private void Awake()
        {
            _itemPickups = new();
        }

        public void PickupUIHandle(ItemObject item, int amount)
        {
            AudioManager.Instance.PlayClip(_popSound, false, true);
            GameObject itemPickupSign = Instantiate(_itemPickupSignPrefab);
            ItemPickupSign pickup = itemPickupSign.GetComponent<ItemPickupSign>();

            pickup.Initialize(amount, item.Name);

            _itemPickups.Add(item, pickup);

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
            checkAgain:

            bool foundOverlap = false;
            var colliders = Physics2D.OverlapBoxAll(_itemPickups.First().Value.gameObject.transform.position, new Vector2(2f, 0.25f), 0.2f);

            foreach (Collider2D collider in colliders)
            {
                if (!collider.TryGetComponent(out ItemPickupSign ips)) continue;
                if (ips == _itemPickups.First().Value) continue;

                _itemPickups.First().Value.gameObject.transform.position += new Vector3(0f, 0.5f, 0f);

                foundOverlap = true;

                await Task.Delay(TimeSpan.FromSeconds(0.03f));
                
                break;
            }

            if (foundOverlap)
                goto checkAgain;

            await Task.Delay(TimeSpan.FromSeconds(0.1f));

            Destroy(_itemPickups.First().Value.gameObject, _disappearTimer);

            _itemPickups.Remove(_itemPickups.First().Key);
        }
    }
}

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

namespace IslandBoy
{
    public class ItemPickupSign : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private TextMeshProUGUI _itemText;
        [SerializeField] private float _disappearTimer;

        private static float _spawnHeight = 2.5f;
        private Action _onDestroy;
        private int _currentStack;
        private string _currentItemName;
        private bool _freeze;

        private void OnDestroy()
        {
            _onDestroy?.Invoke();
        }

        public async Task Initialize(int amount, string itemName, Action OnDestry)
        {
            Debug.Log("Init");
            _currentStack = amount;
            _currentItemName = itemName;
            _itemText.text = $"+{_currentStack} {_currentItemName}";
            _onDestroy = OnDestry;

            transform.position = _pr.Position + new Vector2(0f, _spawnHeight);

            await CheckIfOverlapping();

            Destroy(gameObject, _disappearTimer);
        }

        private async Task CheckIfOverlapping()
        {
            var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(2f, 0.25f), 0.2f);

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out ItemPickupSign itemPickupSign))
                {
                    if (itemPickupSign == this) return;

                    transform.position += new Vector3(0f, 0.5f, 0f);
                    await Task.Delay(TimeSpan.FromSeconds(1f));
                    await CheckIfOverlapping();
                    break;
                }
            }
        }

        public async Task Refresh(int addAmount)
        {
            transform.position = _pr.Position + new Vector2(0f, _spawnHeight);

            _currentStack += addAmount;
            _itemText.text = $"+{_currentStack} {_currentItemName}";

            Destroy(gameObject, _disappearTimer);
            await Task.Yield();
        }
    }
}

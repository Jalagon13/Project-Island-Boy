using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Ammo : MonoBehaviour
    {
        [SerializeField] private ItemParameter _powerParameter;

        private Rigidbody2D _rb;
        private int _damage;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Vector2 direction = _rb.velocity.normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        private void Update()
        {
            if(_rb.velocity.magnitude < 4f)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 7) return;

            if (collision.gameObject.TryGetComponent(out IHealth<int> health))
            {
                health.Damage(_damage, gameObject);
                Destroy(gameObject);
            }
        }

        public void Setup(ItemObject launchObject, ItemObject ammoObject, float multiplier)
        {
            int launchPower = Mathf.RoundToInt(ExtractPower(launchObject) * multiplier);
            int ammoPower = ExtractPower(ammoObject);

            _damage = launchPower + ammoPower;


        }

        private int ExtractPower(ItemObject item)
        {
            var itemParams = item.DefaultParameterList;

            if (itemParams.Contains(_powerParameter))
            {
                int index = itemParams.IndexOf(_powerParameter);
                return (int)itemParams[index].Value;
            }
            Debug.LogError($"{item.Name} does not have power param so can not extract power int");
            return 0;
        }
    }
}

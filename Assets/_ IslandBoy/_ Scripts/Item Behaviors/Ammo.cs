using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Ammo : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _travelDistance;
        [SerializeField] private ItemParameter _damageParameter;

        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private Clickable _clickableFound = null;
        private Vector2 _targetPosition;
        private int _damage;

        private void Awake()
        {
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Vector2 direction = _rb.velocity.normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        private void FixedUpdate()
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _speed);

            if (Vector2.Distance(transform.position, _targetPosition) < 0.05f)
                Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject colliderGo = collision.gameObject;

            if (colliderGo.layer == 7) return;

            if(colliderGo.TryGetComponent(out Entity clickable))
            {
                if (_clickableFound != null) return;

                _clickableFound = clickable;
                _clickableFound.OnHit(ToolType.Sword, _damage);

                Destroy(gameObject);
            }
        }

        public void Setup(ItemObject launchObject, ItemObject ammoObject, Vector3 direction)
        {
            int launchPower = Mathf.RoundToInt(ExtractDamage(launchObject));
            int ammoPower = Mathf.RoundToInt(ExtractDamage(ammoObject));

            _damage = launchPower + ammoPower;
            _targetPosition = transform.position + (direction * _travelDistance);
            Debug.Log(_targetPosition);
            RotateSpriteTowards(_targetPosition);
        }

        private void RotateSpriteTowards(Vector2 target)
        {
            Vector2 direction = (target - (Vector2)_sr.transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var offset = -45;
            _sr.transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }

        private int ExtractDamage(ItemObject item)
        {
            var itemParams = item.DefaultParameterList;

            if (itemParams.Contains(_damageParameter))
            {
                int index = itemParams.IndexOf(_damageParameter);
                return (int)itemParams[index].Value;
            }
            Debug.LogError($"{item.Name} does not have power param so can not extract power int");
            return 0;
        }
    }
}
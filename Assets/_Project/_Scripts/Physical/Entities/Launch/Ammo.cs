using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Ammo : MonoBehaviour
    {
        //[SerializeField] SpellObject _spellObject;
        //[SerializeField] Spell _spell;
        [SerializeField] private float _speed;
        [SerializeField] private ItemParameter _powerParameter;

        private Rigidbody2D _rb;
        private Entity _targetEntity;
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
            _targetPosition = _targetEntity != null ? _targetEntity.transform.position : _targetPosition;
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _speed);

            if (_targetEntity == null)
            {
                if (Vector2.Distance(transform.position, _targetPosition) < 0.1f)
                    Destroy(gameObject);
            }
        }

        private void LateUpdate()
        {
            if(_targetEntity != null)
                RotateSpriteTowards(_targetEntity.transform.position);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject colliderGo = collision.gameObject;

            if (colliderGo.layer == 7) return;

            if(colliderGo.TryGetComponent(out Clickable clickable))
            {
                if (_clickableFound != null) return;
                _clickableFound = clickable;

                //if(_clickableFound is Entity)
                //{
                //    Spell spell = Instantiate(_spell);
                //    spell.Setup((Entity)_clickableFound, _spellObject);
                //}

                _clickableFound.OnHit(ToolType.Sword, _damage);
                Destroy(gameObject);
            }
        }

        private void RotateSpriteTowards(Vector2 target)
        {
            Vector2 direction = (target - (Vector2)_sr.transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var offset = -45;
            _sr.transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }

        public void Setup(ItemObject launchObject, ItemObject ammoObject, Entity targetEntity)
        {
            int launchPower = Mathf.RoundToInt(ExtractPower(launchObject));
            int ammoPower = Mathf.RoundToInt(ExtractPower(ammoObject));

            _damage = launchPower + ammoPower;
            _targetEntity = targetEntity;
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

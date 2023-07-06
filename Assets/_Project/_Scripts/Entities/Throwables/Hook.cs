using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Hook : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private LineRenderer _hookLr;
        [SerializeField] private float _reelSpeed;

        private LineRenderer _lr;
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private WorldItem _reelItem;
        private Vector2 _playerPos;
        private bool _foundItem;
        private bool _isReeling;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _lr = Instantiate(_hookLr);
        }

        private void LateUpdate()
        {
            _playerPos = _pr.Position + new Vector2(0, 0.5f);

            _lr.SetPosition(0, _playerPos);
            _lr.SetPosition(1, transform.position);

            if(_rb.velocity.magnitude < 0.5f)
            {
                ReelInItem();
            }

            RotateSpriteTowards(_playerPos);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(_isReeling && collision.TryGetComponent(out WorldItem item) && !_foundItem)
            {
                _foundItem = true;
                _reelItem = item;
            }
        }

        private void RotateSpriteTowards(Vector2 target)
        {
            Vector2 direction = (target - (Vector2)_sr.transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var offset = 45f;
            _sr.transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }

        private void ReelInItem()
        {
            _isReeling = true;
            transform.position = Vector2.MoveTowards(transform.position, _playerPos, _reelSpeed * Time.deltaTime);

            if(_reelItem != null)
            {
                _reelItem.transform.position = transform.position;

                if(Vector2.Distance(transform.position, _playerPos) < 2f)
                {
                    Destroy(_lr.gameObject);
                    Destroy(gameObject);
                }
            }
            else if(Vector2.Distance(transform.position, _playerPos) < 0.5f)
            {
                Destroy(_lr.gameObject);
                Destroy(gameObject);
            }
        }
    }
}

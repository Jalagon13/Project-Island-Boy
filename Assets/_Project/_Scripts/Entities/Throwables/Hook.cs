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
        private WorldItem _reelItem;
        private bool _foundItem;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _lr = Instantiate(_hookLr);
        }

        private void LateUpdate()
        {
            var plrPos = _pr.Position + new Vector2(0, 0.5f);

            _lr.SetPosition(0, plrPos);
            _lr.SetPosition(1, transform.position);

            if(_rb.velocity.magnitude < 0.5f)
            {
                if (_foundItem)
                    ReelInItem();
                else
                {
                    Destroy(_lr.gameObject);
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out WorldItem item))
            {
                _foundItem = true;
                _reelItem = item;
            }
        }

        private void ReelInItem()
        {
            if (_reelItem == null) return;

            if (Vector2.Distance(_reelItem.transform.position, _pr.Position) > 2.25f)
            {
                _reelItem.transform.position = transform.position;
                transform.position = Vector2.MoveTowards(transform.position, _pr.Position, _reelSpeed * Time.deltaTime);
            }
            else
            {
                Destroy(_lr.gameObject);
                Destroy(gameObject);
            }
        }
    }
}

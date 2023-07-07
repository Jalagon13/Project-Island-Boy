using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class WorldItem : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;

        private ItemObject _item;
        private List<ItemParameter> _currentParameters;
        private SpriteRenderer _sr;
        private int _currentStack;
        private bool _canCollect;
        private bool _collected;

        private void Awake()
        {
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f);
            _canCollect = true;
        }

        public void Initialize(ItemObject item, int currentStack, List<ItemParameter> currentParameters)
        {
            _item = item;
            _currentStack = currentStack;
            _currentParameters = currentParameters;
            _sr.sprite = item.UiDisplay;

            gameObject.name = $"[Item] {item.Name}";
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            // need to add code later on to disable attractor when invetory full and enable it when inventory has space.
            if (collision.CompareTag("Collect") && _canCollect && !_collected)
            {
                Debug.Log(gameObject.name);
                var leftover = _pr.Inventory.AddItem(_item, _currentStack, _currentParameters);
                Debug.Log(leftover);

                if (leftover == 0)
                {
                    _collected = true;
                    Destroy(gameObject);
                }
                else
                    _currentStack = leftover;
            }
        }
    }
}

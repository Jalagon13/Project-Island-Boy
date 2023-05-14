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
        private int _currentStack;
        private SpriteRenderer _sr;

        private void Awake()
        {
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        public void Initialize(ItemObject item, int currentStack, List<ItemParameter> currentParameters)
        {
            _item = item;
            _currentStack = currentStack;
            _currentParameters = currentParameters;
            _sr.sprite = item.UiDisplay;

            gameObject.name = $"[Item] {item.Name}";
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // need to add code later on to disable attractor when invetory full and enable it when inventory has space.
            if (collision.CompareTag("Collect"))
            {
                var leftover = _pr.Inventory.AddItem(_item, _currentStack, _currentParameters);

                if (leftover == 0)
                    Destroy(gameObject);
                else
                    _currentStack = leftover;
            }
        }
    }
}

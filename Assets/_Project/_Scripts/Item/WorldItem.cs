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
            if (collision.CompareTag("Collect"))
            {
                AddToInventory();
            }
        }

        public void AddToInventory()
        {
            var counter = _currentStack;

            for (int i = 0; i < counter; i++)
            {
                bool addedItem = _pr.PlayerInventory.AddItem(_item, _currentParameters);

                if (addedItem)
                {
                    _currentStack--;
                    continue;
                }

                return;
            }

            Destroy(gameObject);
        }
    }
}

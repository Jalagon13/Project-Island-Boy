using UnityEngine;

namespace IslandBoy
{
    public class WorldItemResource : MonoBehaviour, IWorldItem
    {
        [SerializeField] protected PlayerReference _pr;
        [SerializeField] private ResourceObject _resourceItem;
        [SerializeField] private int _currentStack;

        public void AddToInventory()
        {
            var counter = _currentStack;

            for (int i = 0; i < counter; i++)
            {
                bool addedItem = _pr.PlayerInventory.AddItem(_resourceItem);

                if (addedItem)
                {
                    _currentStack--;
                    continue;
                }

                return;
            }

            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Collect"))
            {
                AddToInventory();
            }
        }
    }
}

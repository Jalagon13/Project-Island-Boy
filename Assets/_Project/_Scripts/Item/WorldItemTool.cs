using UnityEngine;

namespace IslandBoy
{
    public class WorldItemTool : MonoBehaviour, IWorldItem
    {
        [SerializeField] protected PlayerReference _pr;
        [SerializeField] private ToolObject _toolItem;

        [SerializeField] private int _currentDurability;

        // For when this World Item is spawned.
        public void InitializeItemInWorld(ToolObject item, int currentDurability)
        {
            _toolItem = item;
            _currentDurability = currentDurability;
        }

        public void AddToInventory()
        {
            bool addedItem = _pr.PlayerInventory.AddTool(_toolItem, _currentDurability);

            if(addedItem)
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

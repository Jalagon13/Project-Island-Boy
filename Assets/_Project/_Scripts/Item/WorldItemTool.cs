using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class WorldItemTool : MonoBehaviour, IWorldItem
    {
        [SerializeField] protected PlayerReference _pr;
        [SerializeField] private ToolObject _toolItem;
        [SerializeField] private List<ItemParameter> _currentParameters;

        // For when this World Item is spawned.
        public void InitializeItemInWorld(ToolObject item, int currentDurability)
        {
            _toolItem = item;
        }

        public void AddToInventory()
        {
            bool addedItem = _pr.PlayerInventory.AddItem(_toolItem, _currentParameters);

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

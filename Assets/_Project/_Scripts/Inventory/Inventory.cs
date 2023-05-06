using UnityEngine;

namespace IslandBoy
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _inventoryItemPrefab;
        [SerializeField] private GameObject _inventoryItemToolPrefab;
        [SerializeField] private int _maxStack;
        [SerializeField] private InventorySlot[] _inventorySlots;

        private void Awake()
        {
            _pr.PlayerInventory = this;
        }

        public bool AddItem(ItemObject item)
        {
            // Check if any slot has the same item with count lower than max stack.
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

                if (itemInSlot != null &&
                    itemInSlot.Item == item &&
                    itemInSlot.Count < _maxStack &&
                    itemInSlot.Item.Stackable == true)
                {
                    itemInSlot.IncrementCount();
                    return true;
                }
            }

            // Find an empty slot
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];
                
                if(slot.transform.childCount == 0)
                {
                    SpawnItemInInventory(item, slot);
                    return true;
                }
            }

            return false;
        }

        private void SpawnItemInInventory(ItemObject item, InventorySlot slot)
        {
            GameObject newItemGo = Instantiate(InventoryItemPrefab(item), slot.transform);
            IInventoryItemInitializer inventoryItem = newItemGo.GetComponent<IInventoryItemInitializer>();
            inventoryItem.Initialize(item);
        }

        private GameObject InventoryItemPrefab(ItemObject item)
        {
            return item is ToolObject ? _inventoryItemToolPrefab : _inventoryItemPrefab;
        }
    }
}

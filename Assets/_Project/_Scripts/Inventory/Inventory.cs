using UnityEngine;

namespace IslandBoy
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryItemPrefab;
        [SerializeField] private GameObject _inventoryItemToolPrefab;
        [SerializeField] private InventorySlot[] _inventorySlots;

        private void OnEnable()
        {
            WorldItem.OnCollectItem += AddItem;
        }

        private void OnDisable()
        {
            WorldItem.OnCollectItem -= AddItem;
        }

        public void AddItem(ItemObject item)
        {
            for(int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                
                if(itemInSlot == null)
                {
                    SpawnItemInInventory(item, slot);
                    return;
                }
            }
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

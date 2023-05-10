using UnityEngine;

namespace IslandBoy
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _inventoryItemRscPrefab;
        [SerializeField] private GameObject _inventoryItemToolPrefab;
        [SerializeField] private int _maxStack;
        [SerializeField] private InventorySlot[] _inventorySlots;

        private MouseItemHolder _mouseItemHolder;

        public InventorySlot[] InventorySlots { get { return _inventorySlots; } }

        private void Awake()
        {
            _pr.PlayerInventory = this;
            _mouseItemHolder = transform.GetChild(2).GetComponent<MouseItemHolder>();
        }

        private void Start()
        {
            foreach (InventorySlot slot in _inventorySlots)
            {
                slot.MouseItemHolder = _mouseItemHolder;
                slot.MaxStack = _maxStack;
            }
        }

        public bool AddResource(ResourceObject resource)
        {
            // Check if any slot has the same item with count lower than max stack.
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];
                InventoryItemRsc itemInSlot = slot.GetComponentInChildren<InventoryItemRsc>();

                if (itemInSlot != null &&
                    itemInSlot.Item == resource &&
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
                    SpawnRscInInventory(resource, slot);
                    return true;
                }
            }

            return false;
        }

        private void SpawnRscInInventory(ResourceObject resource, InventorySlot slot)
        {
            GameObject newItemGo = Instantiate(_inventoryItemRscPrefab, slot.transform);
            InventoryItemRsc newItemRsc = newItemGo.GetComponent<InventoryItemRsc>();
            newItemRsc.Initialize(resource);
        }

        public bool AddTool(ToolObject tool, int durability)
        {
            // Find an empty slot
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];

                if (slot.transform.childCount == 0)
                {
                    SpawnToolInInventory(tool, slot, durability);
                    return true;
                }
            }

            return false;
        }

        private void SpawnToolInInventory(ToolObject tool, InventorySlot slot, int durability)
        {
            GameObject newItemGo = Instantiate(_inventoryItemToolPrefab, slot.transform);
            InventoryItemTool newItemTool = newItemGo.GetComponent<InventoryItemTool>();
            newItemTool.Initialize(tool, durability);
        }
    }
}

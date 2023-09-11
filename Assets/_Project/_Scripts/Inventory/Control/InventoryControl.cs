using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class InventoryControl : MonoBehaviour
    {
        public EventHandler OnInventoryClosed;

        private Inventory _inventory;
        private MouseItemHolder _mouseItemHolder;
        private PlayerInput _input;
        private RectTransform _mainInventory;
        private RectTransform _craftSlots;
        private CraftSlotsControl _craftSlotsControl;
        private Chest _currentChestOpen;
        private bool _inventoryOpen;

        private void Awake()
        {
            _input = new PlayerInput();
            _inventory = GetComponent<Inventory>();
            _craftSlotsControl = GetComponent<CraftSlotsControl>();
            _mainInventory = transform.GetChild(0).GetComponent<RectTransform>();
            _mouseItemHolder = transform.GetChild(2).GetComponent<MouseItemHolder>();
            _craftSlots = transform.GetChild(0).GetChild(2).GetComponent<RectTransform>();

            _input.Player.ToggleInventory.started += ToggleInventory;
        }
        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void Start()
        {
            CloseInventory();
        }

        public void ChestInteract(Chest chestOpened)
        {
            OpenInventory();

            _currentChestOpen = chestOpened;
            _craftSlots.gameObject.SetActive(false);
        }

        public void CraftStationInteract(RecipeDatabaseObject rdb, RuneDatabaseObject runeDb)
        {
            OpenInventory();
            CloseChest();
            
            _craftSlots.gameObject.SetActive(true);
            _craftSlotsControl.RefreshCraftingMenu(rdb, runeDb);
        }

        private void CloseChest()
        {
            if (_currentChestOpen != null)
            {
                _currentChestOpen.EnableChestSlots(false);
                _currentChestOpen = null;
            }
        }

        public void ToggleInventory(InputAction.CallbackContext context)
        {
            if (_inventoryOpen)
                CloseInventory();
            else
                OpenInventory();
        }

        private void CloseInventory()
        {
            if (_mouseItemHolder.HasItem()) return;

            _craftSlots.gameObject.SetActive(true);
            _craftSlotsControl.RefreshCraftingMenu(_craftSlotsControl.DefaultRdb, null);
            _mainInventory.gameObject.SetActive(false);
            _inventoryOpen = false;

            OnInventoryClosed?.Invoke(this, EventArgs.Empty);

            foreach (Slot slot in _inventory.InventorySlots)
            {
                slot.InventoryOpen = false;
            }
        }

        public void OpenInventory()
        {
            _mainInventory.gameObject.SetActive(true);
            _inventoryOpen = true;

            foreach (Slot slot in _inventory.InventorySlots)
            {
                slot.InventoryOpen = true;
            }
        }
    }
}

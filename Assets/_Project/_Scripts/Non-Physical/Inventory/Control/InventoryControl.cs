using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class InventoryControl : MonoBehaviour
    {
        public EventHandler OnInventoryClosed;

        [SerializeField] private RecipeDatabaseObject _defaultRdb;
        [SerializeField] private TabControl _tabControl;

        private Inventory _inventory;
        private PromptControl _promptControl;
        private MouseItemHolder _mouseItemHolder;
        private PlayerInput _input;
        private RectTransform _mainInventory;
        private CraftSlotsControl _craftSlotsControl;
        private Interactable _currentInteractableActive;
        private bool _inventoryOpen;

        private void Awake()
        {
            _input = new PlayerInput();
            _inventory = GetComponent<Inventory>();
            _promptControl = GetComponent<PromptControl>();
            _craftSlotsControl = GetComponent<CraftSlotsControl>();
            _mainInventory = transform.GetChild(0).GetComponent<RectTransform>();
            _mouseItemHolder = transform.GetChild(2).GetComponent<MouseItemHolder>();

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

        private void Update()
        {
            if (_currentInteractableActive == null) return;

            if (!_currentInteractableActive.PlayerInRange(_currentInteractableActive.gameObject.transform.position))
            {
                InteractableHandle(null);
            }
        }

        public void ChestInteract(Interactable chestOpened)
        {
            OpenInventory();
            InteractableHandle(chestOpened);

            _tabControl.DisableAllTabs();
            _promptControl.PromptHandle(null);
        }

        public void CraftStationInteract(Interactable craftStation, RecipeDatabaseObject rdb)
        {
            OpenInventory();
            _tabControl.OpenCraftTab();

            _promptControl.PromptHandle(null);

            if (craftStation == _currentInteractableActive) return;

            if(_currentInteractableActive is CraftStation)
            {
                _currentInteractableActive = craftStation;
                _craftSlotsControl.RefreshCraftingMenu(rdb);
                return;
            }

            InteractableHandle(craftStation);

            _craftSlotsControl.RefreshCraftingMenu(rdb);
        }

        private void InteractableHandle(Interactable newInteractable)
        {
            if (_currentInteractableActive != null)
                _currentInteractableActive.OnPlayerExitRange?.Invoke();

            _currentInteractableActive = newInteractable;
        }

        public void ToggleInventory(InputAction.CallbackContext context)
        {
            if (_inventoryOpen)
                CloseInventory();
            else
                OpenInventory();
        }

        public void RefreshCraftSlotsToDefault()
        {
            _craftSlotsControl.RefreshCraftingMenu(_defaultRdb);
        }

        public void CloseInventory()
        {
            if (_mouseItemHolder.HasItem()) return;

            InteractableHandle(null);
            RefreshCraftSlotsToDefault();

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
            _tabControl.OpenCraftTab();
            _mainInventory.gameObject.SetActive(true);
            _inventoryOpen = true;

            foreach (Slot slot in _inventory.InventorySlots)
            {
                slot.InventoryOpen = true;
            }
        }
    }
}

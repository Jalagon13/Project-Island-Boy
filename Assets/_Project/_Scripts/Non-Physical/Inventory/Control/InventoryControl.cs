using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class InventoryControl : MonoBehaviour
    {
        [SerializeField] private CraftingDatabaseObject _defaultCDB;
        [SerializeField] private RectTransform _craftHolder;

        private Inventory _inventory;
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
            _craftSlotsControl = GetComponent<CraftSlotsControl>();
            _mainInventory = transform.GetChild(0).GetComponent<RectTransform>();
            _mouseItemHolder = transform.GetChild(3).GetComponent<MouseItemHolder>();
            _input.Player.ToggleInventory.started += ToggleInventory;
            _input.Enable();

            GameSignals.CHEST_INTERACT.AddListener(ChestInteract);
            GameSignals.GAME_PAUSED.AddListener(PauseHandle);
            GameSignals.GAME_UNPAUSED.AddListener(UnpauseHandle);
            GameSignals.PLAYER_DIED.AddListener(PauseHandle);
            GameSignals.DAY_END.AddListener(PauseHandle);
            GameSignals.DAY_START.AddListener(UnpauseHandle);
            GameSignals.ADD_ITEM_TO_INVENTORY_FROM_CHEST.AddListener(AddItemToInventoryFromChest); // BROOKE
        }

        private void OnDestroy()
        {
            _input.Disable();

            GameSignals.CHEST_INTERACT.RemoveListener(ChestInteract);
            GameSignals.GAME_PAUSED.RemoveListener(PauseHandle);
            GameSignals.GAME_UNPAUSED.RemoveListener(UnpauseHandle);
            GameSignals.PLAYER_DIED.RemoveListener(PauseHandle);
            GameSignals.DAY_END.RemoveListener(PauseHandle);
            GameSignals.DAY_START.RemoveListener(UnpauseHandle);
            GameSignals.ADD_ITEM_TO_INVENTORY_FROM_CHEST.RemoveListener(AddItemToInventoryFromChest); // BROOKE
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

        private void PauseHandle(ISignalParameters parameter)
        {
            CloseInventory();
            _input.Disable();
        }

        private void UnpauseHandle(ISignalParameters parameters)
        {
            _input.Enable();
        }

        public void ChestInteract(ISignalParameters parameter)
        {
            Interactable chestOpened = (Interactable)parameter.GetParameter("ChestInteract");

            OpenInventory(true); // BROOKE
            _craftHolder.gameObject.SetActive(false);
            InteractableHandle(chestOpened);
        }

        public void CraftStationInteract(Interactable craftStation, CraftingDatabaseObject rdb)
        {
            OpenInventory();

            if (craftStation == _currentInteractableActive) return;

            if (_currentInteractableActive is CraftStation)
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
            _craftSlotsControl.RefreshCraftingMenu(_defaultCDB);
        }

        public void CloseInventory()
        {
            if (_mouseItemHolder.HasItem()) return;

            InteractableHandle(null);
            RefreshCraftSlotsToDefault();

            _mainInventory.gameObject.SetActive(false);
            _inventoryOpen = false;

            GameSignals.INVENTORY_CLOSE.Dispatch();

            foreach (Slot slot in _inventory.InventorySlots)
            {
                slot.InventoryOpen = false;
                slot.ChestOpen = false; // BROOKE
            }
        }

        public void OpenInventory(bool openChest = false)
        {
            _mainInventory.gameObject.SetActive(true);
            _inventoryOpen = true;
            _craftHolder.gameObject.SetActive(true);

            GameSignals.INVENTORY_OPEN.Dispatch();

            foreach (Slot slot in _inventory.InventorySlots)
            {
                slot.InventoryOpen = true;
                if (openChest) slot.ChestOpen = true; // BROOKE
            }
        }

        public void AddItemToInventoryFromChest(ISignalParameters parameters) // BROOKE --------------------------------------------------
        {
            Debug.Log("signal for additemtoinventory");
            // if item was added successfully, delete item from chest
            if (AddItemToInventoryFromChest(parameters.GetParameter("itemToAdd") as ChestInvSlot))
            {
                Destroy(parameters.GetParameter("itemObj") as GameObject);
                // TODO: creating errors?? related to destroying item when inventory full
            }
            // TODO: don't play sound or play error sound if wasn't able to add item
        }

        public bool AddItemToInventoryFromChest(ChestInvSlot itemToAdd)
        {
            int leftOver = _inventory.AddItem(itemToAdd.OutputItem, itemToAdd.OutputAmount);

            // TODO: tries to add to armor inventory when inventory is full
            if (leftOver > 0)
                return false;
            else return true;
        } // BROOKE --------------------------------------------------
    }
}

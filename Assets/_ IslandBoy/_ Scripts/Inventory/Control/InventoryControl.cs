using MoreMountains.Tools;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class InventoryControl : MonoBehaviour
    {
        [SerializeField] private CraftingDatabaseObject _defaultCDB;
        [SerializeField] private RectTransform _craftHolder;
        [SerializeField] private AudioClip _openSound;
        [SerializeField] private AudioClip _closeSound;

        private Inventory _inventory;
        private MouseSlot _mouseItemHolder;
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
            _mouseItemHolder = transform.GetChild(3).GetChild(0).GetComponent<MouseSlot>();
            _input.Player.ToggleInventory.started += ToggleInventory;
            _input.Enable();

            GameSignals.DISPLAY_INTERACTABLE.AddListener(DisplayInteractable);
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

            GameSignals.DISPLAY_INTERACTABLE.RemoveListener(DisplayInteractable);
            GameSignals.GAME_PAUSED.RemoveListener(PauseHandle);
            GameSignals.GAME_UNPAUSED.RemoveListener(UnpauseHandle);
            GameSignals.PLAYER_DIED.RemoveListener(PauseHandle);
            GameSignals.DAY_END.RemoveListener(PauseHandle);
            GameSignals.DAY_START.RemoveListener(UnpauseHandle);
            GameSignals.ADD_ITEM_TO_INVENTORY_FROM_CHEST.RemoveListener(AddItemToInventoryFromChest); // BROOKE
        }

        private void Start()
        {
            CloseInventory(playSound: false);
        }

        private void Update()
        {
            if (_currentInteractableActive == null) return;

            if (!_currentInteractableActive.PlayerInRange(_currentInteractableActive.gameObject.transform.position + new Vector3(0.5f, 0.5f)))
            {
                InteractableHandle(null);
            }
        }

        private void PauseHandle(ISignalParameters parameter)
        {
            CloseInventory(playSound:false);
            _input.Disable();
        }

        private void UnpauseHandle(ISignalParameters parameters)
        {
            _input.Enable();
        }

        public void DisplayInteractable(ISignalParameters parameters)
        {
            Interactable interactable = (Interactable)parameters.GetParameter("Interactable");

            OpenInventory(true);
            _craftHolder.gameObject.SetActive(false);
            InteractableHandle(interactable);
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

        public void CloseInventory(bool playSound = true)
        {
            if (_mouseItemHolder.HasItem()) return;

            InteractableHandle(null);
            RefreshCraftSlotsToDefault();

            _mainInventory.gameObject.SetActive(false);
            _inventoryOpen = false;

            if(playSound)
                MMSoundManagerSoundPlayEvent.Trigger(_closeSound, MMSoundManager.MMSoundManagerTracks.UI, default);

            GameSignals.INVENTORY_CLOSE.Dispatch();

            foreach (Slot slot in _inventory.InventorySlots)
            {
                slot.InventoryOpen = false;
                slot.ChestOpen = false; // BROOKE
            }
        }

        public void OpenInventory(bool openChest = false, bool playSound = true)
        {
            _mainInventory.gameObject.SetActive(true);
            _inventoryOpen = true;
            _craftHolder.gameObject.SetActive(true);

            if (playSound)
                MMSoundManagerSoundPlayEvent.Trigger(_openSound, MMSoundManager.MMSoundManagerTracks.UI, default);

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

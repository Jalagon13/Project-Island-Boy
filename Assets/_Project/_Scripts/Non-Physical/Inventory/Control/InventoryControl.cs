using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class InventoryControl : MonoBehaviour
    {
        [SerializeField] private RecipeDatabaseObject _defaultRdb;
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
            _mouseItemHolder = transform.GetChild(2).GetComponent<MouseItemHolder>();
            _input.Player.ToggleInventory.started += ToggleInventory;

            GameSignals.CHEST_INTERACT.AddListener(ChestInteract);
            GameSignals.GAME_PAUSED.AddListener(PauseHandle);
            GameSignals.GAME_UNPAUSED.AddListener(UnpauseHandle);
            GameSignals.PLAYER_DIED.AddListener(PauseHandle);
            GameSignals.DAY_END.AddListener(PauseHandle);
            GameSignals.DAY_OUT_OF_TIME.AddListener(PauseHandle);
            GameSignals.DAY_START.AddListener(UnpauseHandle);
        }

        private void OnDestroy()
        {
            GameSignals.CHEST_INTERACT.RemoveListener(ChestInteract);
            GameSignals.GAME_PAUSED.RemoveListener(PauseHandle);
            GameSignals.GAME_UNPAUSED.RemoveListener(UnpauseHandle);
            GameSignals.PLAYER_DIED.RemoveListener(PauseHandle);
            GameSignals.DAY_END.RemoveListener(PauseHandle);
            GameSignals.DAY_OUT_OF_TIME.RemoveListener(PauseHandle);
            GameSignals.DAY_START.RemoveListener(UnpauseHandle);
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

            OpenInventory();
            _craftHolder.gameObject.SetActive(false);
            InteractableHandle(chestOpened);
        }

        public void CraftStationInteract(Interactable craftStation, RecipeDatabaseObject rdb)
        {
            OpenInventory();
            
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

            GameSignals.INVENTORY_CLOSE.Dispatch();

            foreach (Slot slot in _inventory.InventorySlots)
            {
                slot.InventoryOpen = false;
            }
        }

        public void OpenInventory()
        {
            _mainInventory.gameObject.SetActive(true);
            _inventoryOpen = true;
            _craftHolder.gameObject.SetActive(true);

            GameSignals.INVENTORY_OPEN.Dispatch();

            foreach (Slot slot in _inventory.InventorySlots)
            {
                slot.InventoryOpen = true;
            }
        }
    }
}

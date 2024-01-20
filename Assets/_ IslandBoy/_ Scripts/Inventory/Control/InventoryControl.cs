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
		
		public bool IsInventoryOpen { get { return _inventoryOpen; } }

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
			GameSignals.INVENTORY_CLOSE.AddListener(CloseInventory);
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
			GameSignals.INVENTORY_CLOSE.RemoveListener(CloseInventory);
		}

		private void Start()
		{
			CloseInventory(null);
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
			CloseInventory(null);
			_input.Disable();
		}

		private void UnpauseHandle(ISignalParameters parameters)
		{
			_input.Enable();
		}

		public void DisplayInteractable(ISignalParameters parameters)
		{
			Interactable interactable = (Interactable)parameters.GetParameter("Interactable");
			MMSoundManagerSoundPlayEvent.Trigger(_openSound, MMSoundManager.MMSoundManagerTracks.UI, default, volume:2f);
			
			OpenInventory(true, false);
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
			if(_currentInteractableActive == newInteractable)
				return;
			
			if (_currentInteractableActive != null)
				_currentInteractableActive.OnPlayerExitRange?.Invoke();

			_currentInteractableActive = newInteractable;
		}

		public void ToggleInventory(InputAction.CallbackContext context)
		{
			if (_inventoryOpen)
				GameSignals.INVENTORY_CLOSE.Dispatch();
			else
				OpenInventory();
		}

		public void RefreshCraftSlotsToDefault()
		{
			_craftSlotsControl.RefreshCraftingMenu(_defaultCDB);
		}

		public void CloseInventory(ISignalParameters parameters)
		{
			if (_mouseItemHolder.HasItem()) return;

			InteractableHandle(null);
			RefreshCraftSlotsToDefault();

			_mainInventory.gameObject.SetActive(false);
			_inventoryOpen = false;

			MMSoundManagerSoundPlayEvent.Trigger(_closeSound, MMSoundManager.MMSoundManagerTracks.UI, default, volume:2f);

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
				MMSoundManagerSoundPlayEvent.Trigger(_openSound, MMSoundManager.MMSoundManagerTracks.UI, default, volume:2f);

			GameSignals.INVENTORY_OPEN.Dispatch();

			foreach (Slot slot in _inventory.InventorySlots)
			{
				slot.InventoryOpen = true;
				if (openChest) slot.ChestOpen = true; // BROOKE
			}
		}

		public void AddItemToInventoryFromChest(ISignalParameters parameters) // BROOKE --------------------------------------------------
		{
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

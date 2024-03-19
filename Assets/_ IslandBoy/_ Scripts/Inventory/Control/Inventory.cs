using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace IslandBoy
{
	public class Inventory : MonoBehaviour
	{
		[SerializeField] private PlayerObject _pr;
		[SerializeField] private int _maxStack;
		[SerializeField] private MMF_Player _tutorialFeedback;
		[SerializeField] private List<ChestInvSlot> _startingItems = new();
		[SerializeField] private List<Slot> _allSlots = new();
		[SerializeField] private List<ArmorSlot> _armorSlots = new();
		[SerializeField] private List<AccessorySlot> _accessorySlots = new();

		private MouseSlot _mouseItemHolder;
		private CraftSlotsControl _csc;

		public List<Slot> InventorySlots { get { return _allSlots; } }
		public MouseSlot MouseItemHolder { get { return _mouseItemHolder; } }
		public InventoryControl InventoryControl { get { return GetComponent<InventoryControl>(); } }
		public int MaxStack { get { return _maxStack; } }
		public CraftSlotsControl CraftSlotsControl => _csc;

		private void Awake()
		{
			_pr.Inventory = this;
			_csc = GetComponent<CraftSlotsControl>();
			_mouseItemHolder = transform.GetChild(3).GetChild(0).GetComponent<MouseSlot>();
			
			GameSignals.ENABLE_STARTING_MECHANICS.AddListener(EnableNPCView);
		}
		
		private void OnDestroy()
		{
			GameSignals.ENABLE_STARTING_MECHANICS.RemoveListener(EnableNPCView);
		}

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();

			foreach (ChestInvSlot item in _startingItems)
			{
				AddItem(item.OutputItem, item.OutputAmount, item.OutputItem.DefaultParameterList);
			}
		}
		
		private void EnableNPCView(ISignalParameters parameters)
		{
			_tutorialFeedback?.PlayFeedbacks();
		}

		public int AddItem(ItemObject item, int amount, List<ItemParameter> itemParameters = null)
		{
			// return leftover if there is.
			for (int i = 0; i < amount; i++)
			{
				if (!Add(item, amount, itemParameters))
				{
					int leftOver = amount - i;
					DispatchItemAdded();
					//_onPickupItem?.Invoke(item, amount - leftOver);
					return leftOver;
				}
			}

			DispatchItemAdded();
			//_onPickupItem?.Invoke(item, amount);
			return 0;
		}

		/// <summary>
		/// Returns true if successful, false otherwise
		/// </summary>
		public bool AddItemToSlot(ItemObject item, int amount, List<ItemParameter> itemParameters = null) // BROOKE----------------------------------------------------------------
		{
			if (item.AccessoryType != AccessoryType.None)
			{
				for (int i = 0; i < _accessorySlots.Count; i++)
				{
					AccessorySlot slot = _accessorySlots[i];

					if (slot.SpawnInventoryItem(item, itemParameters))
					{
						slot.PlaySound();
						DispatchItemAdded();
						DispatchItemToSlotAdded(item);
						return true;
					}
				}
			}
			else if (item.ArmorType != ArmorType.None)
			{
				for (int i = 0; i < _armorSlots.Count; i++)
				{
					ArmorSlot slot = _armorSlots[i];

					if (slot.ArmorType == item.ArmorType && slot.SpawnInventoryItem(item, itemParameters))
					{
						slot.PlaySound();
						DispatchItemAdded();
						DispatchItemToSlotAdded(item);
						return true;
					}
				}
			}

			DispatchItemAdded();
			return false;
		}

		private void DispatchItemToSlotAdded(ItemObject item)
		{
			Signal signal = GameSignals.EQUIP_ITEM;
			signal.ClearParameters();
			signal.AddParameter("item", item);
			signal.Dispatch();
		}

		/// <summary>
		/// returns true if there is no empty slots, false otherwise
		/// </summary>
		public bool IsFull()
		{
			for (int i = 0; i < _allSlots.Count; i++)
			{
				Slot slot = _allSlots[i];

				if (slot.ArmorType == ArmorType.None && !slot.IsAccessorySlot)
				{
					if (!slot.HasItem())
					{
						return false;
					}
				}
				else return true;
			}
			return true;
		}
		// BROOKE----------------------------------------------------------------

		private void DispatchItemAdded()
		{
			Signal signal = GameSignals.ITEM_ADDED;
			signal.ClearParameters();
			signal.AddParameter("Inventory", this);
			signal.Dispatch();
		}

		private bool Add(ItemObject item, int amount, List<ItemParameter> itemParameters = null)
		{
			// Check if any slot has the same item with count lower than max stack.
			for (int i = 0; i < _allSlots.Count; i++)
			{
				Slot slot = _allSlots[i];

				if (slot is not InventorySlot)
					continue;

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
			for (int i = 0; i < _allSlots.Count; i++)
			{
				Slot slot = _allSlots[i];

				if (slot is not InventorySlot)
					continue;

				if (slot.SpawnInventoryItem(item, itemParameters))
				{
					return true;
				}
			}

			return false;
		}

		public void RemoveItem(ItemObject item, int amount)
		{
			var counter = 0;

			for (int j = 0; j < _allSlots.Count; j++)
			{
				if (counter >= amount) return;

				Slot slot = _allSlots[j];
				InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

				repeat:
				if (itemInSlot == null || itemInSlot.Item != item)
					continue;

				itemInSlot.Count--;
				counter++;

				if (itemInSlot.Count == 0)
					continue;
				else if (counter < amount)
					goto repeat;
			}
		}

		public int GetItemAmount(ItemObject item)
		{
			int amount = 0;

			for (int i = 0; i < _allSlots.Count; i++)
			{
				Slot slot = _allSlots[i];

				if (slot.ItemObject == item)
				{
					//Debug.Log(slot == null);
					amount += slot.InventoryItem.Count;
				}
			}

			return amount;
		}

		/// <summary>
		/// returns true if can add item to inventory
		/// </summary>
		public bool CanAddItem(ItemObject item)
		{
			if (!IsFull()) return true; // if empty slot in inv exists, always true
			else if (!item.Stackable) return false; // if inv full and item isn't stackable, always false

			for (int i = 0; i < _allSlots.Count; i++)
			{
				Slot slot = _allSlots[i];

				if (slot.ItemObject == item && slot.InventoryItem.Count < _maxStack)
				{
					return true;
				}
			}

			return false;
		}

		public bool Contains(ItemObject item, int amount)
		{
			var counter = amount;

			for (int i = 0; i < _allSlots.Count; i++)
			{
				Slot slot = _allSlots[i];
				//Debug.Log($"All slots Null?: {_allSlots == null}");
				InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

				if (itemInSlot == null || itemInSlot.Item != item)
					continue;

				counter -= itemInSlot.Count;

				if (counter > 0)
					continue;
				else
					return true;
			}

			return false;
		}
	}
}

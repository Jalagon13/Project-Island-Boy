using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
	public class QuestSlot : Slot
	{
		[Header("Quest Slot Parameters")]
		[Required]
		[SerializeField] private Quest _quest;
		
		private bool _slotComplete;
		
		public override void OnPointerClick(PointerEventData eventData)
		{
			if(_slotComplete) return;
			
			if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
			{
				if (MouseHasQuestItem())
				{
					if(_quest.QuestItems.TryGetValue(_mouseItemHolder.ItemObject, out int value))
					{
						int mouseAmount = _mouseItemHolder.InventoryItem.Count;
						int amountReq = value;
						
						if( mouseAmount >= amountReq)
						{
							_mouseItemHolder.InventoryItem.Count -= amountReq;
							_slotComplete = true;
							_quest.RemoveReq(_mouseItemHolder.ItemObject);
							SpawnInventoryItem(_mouseItemHolder.ItemObject, null, amountReq);
						}
					}
				}
			}
		}
		
		private bool MouseHasQuestItem()
		{
			if(!_mouseItemHolder.HasItem()) return false;
			
			return _quest.QuestItems.ContainsKey(_mouseItemHolder.ItemObject);
		}
	}
}

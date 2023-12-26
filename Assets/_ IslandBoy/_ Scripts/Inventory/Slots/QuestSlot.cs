using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
	public class QuestSlot : Slot
	{
		[Header("Quest Slot Parameters")]
		[SerializeField] private Quest _quest;
		
		public override void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
			{
				if (MouseHasQuestItem())
				{
					_mouseItemHolder.GiveItemToSlot(transform);
					
					// Play game feel here
					PlaySound();
				}
			}
		}
		
		public bool MouseHasQuestItem()
		{
			if(!_mouseItemHolder.HasItem()) return false;
			
			return true;
		}
	}
}

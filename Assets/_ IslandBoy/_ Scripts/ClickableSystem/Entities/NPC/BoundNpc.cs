using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace IslandBoy
{
	public class BoundNpc : Prompt
	{
		[Header("Bound NPC Parameters")]
		[SerializeField] private string _npcToUnlock;
		[SerializeField] private Sprite _unboundSprite;
		[SerializeField] private MMF_Player _recruitFeedback;
		[SerializeField] private MMF_Player _freeFeedback;
		[SerializeField] private List<ChestInvSlot> _unboundRewards = new();
		
		private bool _isFree;

		public void CloseUIBtn() => CloseUI(null); // attached to close button
		
		public override void Interact()
		{
			if (Pointer.IsOverUI()) return;
			if(!_isFree)
			{
				FreeNpc();
				// GiveReward();
			}

			base.Interact();
		}
		
		public void RecruitButton()
		{
			PlayRecruitFeedback();
			PopupMessage.Create(transform.position, $"The {_npcToUnlock} has been recruited and will move in the next day!", Color.white, Vector2.up, 3f);
			HousingController.Instance.UnlockNpc(_npcToUnlock);
			
			switch(_npcToUnlock)
			{
				case "Blacksmith":
					NpcSlots.Instance.UpdateBlacksmithSlot();
					break;
				case "Wizard":
					NpcSlots.Instance.UpdateWizardSlot();
					break;
				case "Knight":
					NpcSlots.Instance.UpdateKnightSlot();
					break;
			}
			
			Destroy(gameObject);
		}
		
		private void PlayRecruitFeedback()
		{
			if (_recruitFeedback != null)
			{
				_recruitFeedback.transform.SetParent(null);
				_recruitFeedback?.PlayFeedbacks();
			}
		}
		
		public void GiveReward()
		{
			foreach (ChestInvSlot item in _unboundRewards)
			{
				_po.Inventory.AddItem(item.OutputItem, item.OutputAmount, item.OutputItem.DefaultParameterList);
			}
			
			GameSignals.SLOT_CLICKED.Dispatch();
		}
		
		private void FreeNpc()
		{
			_isFree = true;
			_sr.sprite = _unboundSprite;
			_freeFeedback?.PlayFeedbacks();
		}
	}
}

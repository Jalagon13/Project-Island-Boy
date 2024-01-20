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
		[SerializeField] private MMF_Player _freeFeedback;
		[SerializeField] private List<ChestInvSlot> _unboundRewards = new();
		
		private bool _isFree;

		protected override void Awake()
		{
			base.Awake();
			
			GameSignals.DAY_END.AddListener(DestroyEntity);
		}
		
		private void OnDestroy()
		{
			GameSignals.DAY_END.RemoveListener(DestroyEntity);
		}

		public void CloseUIBtn() => CloseUI(null); // attached to close button
		
		public void DestroyEntity(ISignalParameters parameters)
		{
			if(_isFree)
			{
				Destroy(gameObject);
			}
		}
		
		public override void Interact()
		{
			if (Pointer.IsOverUI()) return;
			if(!_isFree)
			{
				FreeNpc();
				GiveReward();
			}

			base.Interact();
		}
		
		private void GiveReward()
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
			PopupMessage.Create(transform.position, $"The {_npcToUnlock} has been untied!", Color.white, Vector2.up, 2.5f);
			HousingController.Instance.UnlockNpc(_npcToUnlock);
		}
	}
}

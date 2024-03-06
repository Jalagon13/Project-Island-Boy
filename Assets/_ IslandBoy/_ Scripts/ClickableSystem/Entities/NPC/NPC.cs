using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
	public class NPC : Prompt
	{
		[Header("NPC Parameters")]
		// [SerializeField] private Canvas _questCanvas;
		[SerializeField] private Canvas _shopCanvas;
		[SerializeField] private Canvas _upgradeCanvas;
		[SerializeField] private string _npcToUnlock;
		[SerializeField] private Sprite _unboundSprite;
		[SerializeField] private TextMeshProUGUI _hoverText;
		[SerializeField] private MMF_Player _recruitFeedback;
		[SerializeField] private MMF_Player _freeFeedback;

		[SerializeField] private GameObject _shopButton;
		private Sign _sign;

		protected KnockbackFeedback _knockback;
		private bool _isFree;

		public bool IsFree => _isFree;

		protected override void Awake()
		{
			base.Awake();
			if(_hoverText != null)
				_hoverText.text = "Free";
			_knockback = GetComponent<KnockbackFeedback>();
			_sign = GetComponent<Sign>();
		}
		
		public void OpenQuestUI() // connected to quest button
		{
			DisplayInteractable();

			_shopCanvas.gameObject.SetActive(false);
			// _questCanvas.gameObject.SetActive(true);
			_upgradeCanvas.gameObject.SetActive(false);
		}
		
		public void OpenShopUI() // connected to shop button
		{
			DisplayInteractable();

			_shopCanvas.gameObject.SetActive(true);
			// _questCanvas.gameObject.SetActive(false);
			_upgradeCanvas.gameObject.SetActive(false);
		}
		
		public void OpenUpgradeUI() // connected to upgrade button
		{
			DisplayInteractable();

			_shopCanvas.gameObject.SetActive(false);
			// _questCanvas.gameObject.SetActive(false);
			_upgradeCanvas.gameObject.SetActive(true);
		}
		
		public void Close() => CloseUI(null);
		
		public override void CloseUI(ISignalParameters parameters)
		{
			base.CloseUI(parameters);
			
			// _questCanvas.gameObject.SetActive(false);
			_shopCanvas.gameObject.SetActive(false);
			_upgradeCanvas.gameObject.SetActive(false);
		}
		
		public override void Interact()
		{
			if (Pointer.IsOverUI()) return;
			if(!_isFree)
			{
				FreeNpc();
				_sign.NoBedText();
				_shopButton.SetActive(true);
				_shopButton.gameObject.SetActive(true);
				// GiveReward();
			}
				
			base.Interact();
		}
		
		private void FreeNpc()
		{
			_isFree = true;
			_sr.sprite = _unboundSprite;
			_hoverText.text = "Talk";
			_freeFeedback?.PlayFeedbacks();
			NpcSlots.Instance.FreeNPC(_npcToUnlock);
		}

		private void DisplayInteractable()
		{
			Signal signal = GameSignals.DISPLAY_INTERACTABLE;
			signal.ClearParameters();
			signal.AddParameter("Interactable", this);
			signal.Dispatch();
		}
		
		public override void ShowDisplay()
		{
			EnableInstructions(true);
			EnableAmountDisplay(false);
			EnableProgressBar(false);
		}

		public void UnlockShop()
		{
			switch (_npcToUnlock)
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
			_shopButton.gameObject.SetActive(true);
			_sign.HasBedText();
		}

		public void CloseShop()
		{
			NpcSlots.Instance.RemoveSlot(_npcToUnlock);
			_shopButton.gameObject.SetActive(false);
			_sign.NoBedText();
		}
	}
}

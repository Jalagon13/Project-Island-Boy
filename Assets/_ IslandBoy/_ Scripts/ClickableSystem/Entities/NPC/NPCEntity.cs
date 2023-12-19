using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
	public class NPCEntity : Prompt
	{
		[SerializeField] private ItemObject _questItem;
		[SerializeField] private UnityEvent _onBarkDetected;

		private bool _questComplete;
		protected KnockbackFeedback _knockback;

		protected override void Awake()
		{
			base.Awake();

			_knockback = GetComponent<KnockbackFeedback>();
		}

		private void OnDestroy()
		{
			
		}
		
		public override void Interact()
		{
			if (_questComplete)
			{
				return;
			}

			base.Interact();
		}

		public override bool OnHit(ToolType incomingToolType, int amount, bool displayHit = true)
		{
			_knockback.PlayFeedback(_pr.Position);
			_currentHitPoints = _maxHitPoints;

			if (base.OnHit(incomingToolType, amount, displayHit))
			{
				if (displayHit)
				{
					PopupMessage.Create(transform.position, amount.ToString(), Color.yellow, Vector2.up * 0.5f, 0.4f);
					UpdateAmountDisplay();
					UpdateFillImage();
					EnableAmountDisplay(false);
					EnableInstructions(false);
					EnableProgressBar(false);
				}
				return true;
			}

			return false;
		}

		public void TryToCompleteQuest()
		{
			if (_questComplete)
			{
				PopupMessage.Create(transform.position, $"You can now visit my hideout south of this island!", Color.yellow, Vector2.up, 2f);
				return;
			}

			if (_pr.Inventory.Contains(_questItem, 1))
			{
				PopupMessage.Create(transform.position, $"Just what I was looking for! Thank you! Goodluck out there!", Color.yellow, Vector2.up, 5f);
				_questComplete = true;
				_onBarkDetected?.Invoke();
				return;
			}
			else
			{
				PopupMessage.Create(transform.position, $"Silly goose! You don't have my quest item yet!", Color.red, Vector2.up, 3f);
			}
		}

		public override void ShowDisplay()
		{
			EnableInstructions(true);
			EnableAmountDisplay(false);
			EnableProgressBar(false);
		}
	}
}

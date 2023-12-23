using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
	public class NPCEntity : Prompt
	{
		[Header("NPC Parameters")]
		[SerializeField] private string _name;
		
		public string Name { get { return _name; } }

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

		public override void ShowDisplay()
		{
			EnableInstructions(true);
			EnableAmountDisplay(false);
			EnableProgressBar(false);
		}
	}
}

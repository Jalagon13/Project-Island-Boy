using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Furniture : Clickable
    {
		private Timer _restoreHpTimer;

		protected override void Awake()
		{
			base.Awake();

			_restoreHpTimer = new(5);
			_restoreHpTimer.OnTimerEnd += RestoreHitPoints;
		}

		private void OnDestroy()
		{
			_restoreHpTimer.OnTimerEnd -= RestoreHitPoints;
		}

		protected virtual void Update()
		{
			_restoreHpTimer.Tick(Time.deltaTime);
		}

		public override bool OnHit(ToolType incomingToolType, int amount, bool displayHit = true, ToolTier incomingToolTier = ToolTier.None)
		{
			if (base.OnHit(incomingToolType, amount, displayHit, incomingToolTier))
			{
				if (displayHit)
				{
					UpdateAmountDisplay();
					UpdateFillImage();
					EnableProgressBar(true);
					EnableAmountDisplay(false);
				}
				_restoreHpTimer.RemainingSeconds = 5;
				return true;
			}
			return false;
		}

		private void RestoreHitPoints()
		{
			_currentHitPoints = _maxHitPoints;
		}

		public override void ShowDisplay()
		{
			_instructions.gameObject.SetActive(true);
		}
	}
}

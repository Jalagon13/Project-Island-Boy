using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public abstract class Interactable : Clickable
	{
		public Action OnPlayerExitRange;
		
		private float _interactRange = 2;
		protected bool _canInteract;
		private Vector3 _origin;
		private SpriteRenderer _rightClickSr;
		private Timer _restoreHpTimer;

		protected override void Awake()
		{
			base.Awake();

			_restoreHpTimer = new(5);
			_restoreHpTimer.OnTimerEnd += RestoreHitPoints;
			_origin = transform.position + new Vector3(0.5f, 0.5f);
			_rightClickSr = transform.GetChild(1).GetComponent<SpriteRenderer>();
		}

		private void OnDestroy()
		{
			_restoreHpTimer.OnTimerEnd -= RestoreHitPoints;
		}

		public virtual IEnumerator Start()
		{

			yield return new WaitForSeconds(0.15f);
			_canInteract = true;
		}

		protected virtual void Update()
		{
			_restoreHpTimer.Tick(Time.deltaTime);
		}

		protected override void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.TryGetComponent<CursorControl>(out var cc))
			{
				HideDisplay();
			}
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
					EnableInstructions(false);
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

		public abstract void Interact();

		public bool PlayerInRange(Vector2 customPos)
		{
			return Vector2.Distance(customPos, _po.Position) < _interactRange;
		}

		public override void ShowDisplay()
		{
			throw new NotImplementedException();
		}
	}
}

using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
	public abstract class Clickable : MonoBehaviour
	{
		[Header("Base Clickable Parameters")]
		[SerializeField] protected PlayerObject _po;
		[SerializeField] protected int _maxHitPoints;
		[SerializeField] protected int _expAmount;
		[SerializeField] protected ToolType _breakType;
		[SerializeField] protected ToolTier _breakTier;
		[SerializeField] protected SkillCategory _skillCategory;
		[SerializeField] protected MMF_Player _clickFeedback;
		[SerializeField] protected MMF_Player _destroyFeedback;
		[SerializeField] protected LootTable _lootTable;

		protected int _currentHitPoints;
		protected GameObject _progressBar;
		protected GameObject _amountDisplay;
		protected GameObject _instructions;
		[SerializeField] protected GameObject _cantBreakInstructions;
		protected SpriteRenderer _sr;
		protected Vector2 _dropPosition;

		public ToolType BreakType { get { return _breakType; } }
		public ToolTier BreakTier { get { return _breakTier; } }
		public MMF_Player ClickFeedback { get { return _clickFeedback; } }

		protected virtual void Awake()
		{
			_currentHitPoints = _maxHitPoints;
			_progressBar = transform.GetChild(2).GetChild(0).gameObject;
			_amountDisplay = transform.GetChild(2).GetChild(1).gameObject;
			_instructions = transform.GetChild(2).GetChild(2).gameObject;
			_sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
			_dropPosition = transform.position + (Vector3.one * 0.5f);
		}

		protected virtual void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.TryGetComponent<CursorControl>(out var cc))
			{
				HideDisplay();
			}
		}

		/// <summary>
		/// Checks if given tool's tier can break the given object's tier.
		/// </summary>
		/// <returns>true if can break it, false otherwise</returns>
		public bool IsTierCompatibleWith(ToolTier toolTier, ToolTier objTier)
		{
			switch (toolTier)
			{
				case ToolTier.Iron:
					if (objTier == ToolTier.Iron) return true;
					goto case ToolTier.Stone;
				case ToolTier.Stone:
					if (objTier == ToolTier.Stone) return true;
					goto case ToolTier.Wood;
				case ToolTier.Wood:
					if (objTier == ToolTier.Wood) return true;
					goto case ToolTier.None;
				case ToolTier.None:
					if (objTier == ToolTier.None) return true;
					return false;
			}
			return false;
		}

		public virtual bool OnHit(ToolType incomingToolType, int amount, bool displayHit = true, ToolTier incomingToolTier = ToolTier.None)
		{
			if (incomingToolType != _breakType || !IsTierCompatibleWith(incomingToolTier, _breakTier) || incomingToolType == ToolType.None)
				return false;

			_clickFeedback?.PlayFeedbacks();
			_currentHitPoints -= amount;

			if (_currentHitPoints <= 0)
				OnBreak();

			return true;
		}

		public virtual void OnBreak()
		{
			_lootTable.SpawnLoot(_dropPosition);
			AStarExtensions.Instance.UpdatePathfinding(transform.position, new(2,2,2));
			PlayDestroyFeedbacks();
			StopAllCoroutines();
			
			Signal signal = GameSignals.CLICKABLE_DESTROYED;
			signal.ClearParameters();
			signal.AddParameter("TimeAmount", _maxHitPoints);
			signal.AddParameter("SkillCategory", _skillCategory);
			signal.AddParameter("ExpAmount", _expAmount);
			signal.Dispatch();
			
			Destroy(gameObject);
		}

		public void ResetHealth()
		{
			_currentHitPoints = _maxHitPoints;
			EnableProgressBar(false);
		}

		private void PlayDestroyFeedbacks()
		{
			if (_destroyFeedback != null)
			{
				_destroyFeedback.transform.SetParent(null);
				_destroyFeedback?.PlayFeedbacks();
			}
		}

		public abstract void ShowDisplay();
		
		public virtual void ShowCantBreakDisplay()
		{
			EnableInstructions(false);
			EnableCantBreakInstructions(true);
		}

		public virtual void HideDisplay()
		{
			EnableProgressBar(false);
			EnableAmountDisplay(false);
			EnableInstructions(false);
			EnableCantBreakInstructions(false);
		}

		public virtual void EnableCantBreakInstructions(bool _)
		{
			if (_cantBreakInstructions != null)
				_cantBreakInstructions.SetActive(_);
		}		

		protected virtual void EnableProgressBar(bool _)
		{
			_progressBar.SetActive(_);
		}

		protected virtual void EnableAmountDisplay(bool _)
		{
			_amountDisplay.SetActive(_);
		}

		protected virtual void EnableInstructions(bool _)
		{
			_instructions.SetActive(_);
		}

		protected virtual void UpdateFillImage()
		{
			var fillImage = _progressBar.transform.GetChild(1).GetComponent<Image>();
			fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _maxHitPoints, _currentHitPoints));
		}

		protected virtual void UpdateAmountDisplay()
		{
			var amountText = _amountDisplay.GetComponent<TextMeshProUGUI>();
			amountText.text = _currentHitPoints.ToString();
		}
	}
}

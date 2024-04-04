using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;

namespace IslandBoy
{
	[SelectionBase]
	public class Resource : Clickable
	{
		[Header("Base Resource Parameters")]
		[SerializeField] private string _rscName;
		[SerializeField] private bool _giveXp = true;
		[SerializeField] protected bool _destructable = true;
		// [Tooltip("Think of Grass from Terraria")]
		// [SerializeField] protected bool _swingDestructOnly = false;
		// [SerializeField] protected int _spawnRate;

		// public bool SwingDestructOnly { get { return _swingDestructOnly; } }
		// public int SpawnRate { get { return _spawnRate; } }
		public int MaxHitPoints => _maxHitPoints;
		public string RscName => _rscName;

		// private Timer _restoreHpTimer;

		protected override void Awake()
		{
			base.Awake();

			// _restoreHpTimer = new(5);
			// _restoreHpTimer.OnTimerEnd += RestoreHitPoints;
			_dropPosition = transform.position + new Vector3(0.5f, 0.5f, 0);
		}

		private void OnDestroy()
		{
			// _restoreHpTimer.OnTimerEnd -= RestoreHitPoints;
		}

		private void OnEnable()
		{
			_currentHitPoints = _maxHitPoints;

			EnableProgressBar(false);
			EnableAmountDisplay(false);
			EnableInstructions(false);
		}

		private void Start()
		{
			EnableProgressBar(false);
			EnableAmountDisplay(false);
			EnableInstructions(false);
		}

		// protected virtual void Update()
		// {
		// 	_restoreHpTimer.Tick(Time.deltaTime);
		// }

		protected override void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.TryGetComponent<CursorControl>(out var cc))
			{
				HideDisplay();
			}
		}

		public override bool OnHit(ToolType incomingToolType, int amount, bool displayHit = true, ToolTier incomingToolTier = ToolTier.None)
		{
			if (!_destructable || !base.OnHit(incomingToolType, amount, displayHit, incomingToolTier)) 
				return false;

			if (displayHit)
			{
				UpdateAmountDisplay();
				UpdateFillImage();
				EnableProgressBar(true);
				EnableAmountDisplay(false);
			}
			
			Signal signal = GameSignals.CLICKABLE_CLICKED;
			signal.ClearParameters();
			signal.AddParameter("EnergyLost", 1);
			signal.Dispatch();
			
			// _restoreHpTimer.RemainingSeconds = 5;

			return true;
		}
		
		public override void OnBreak()
		{
			if(_giveXp)
			{
				Signal signal = GameSignals.RESOURCE_CLEARED;
				signal.ClearParameters();
				signal.AddParameter("Resource", this);
				signal.Dispatch();
			}

			base.OnBreak();
		}

		private void RestoreHitPoints()
		{
			_currentHitPoints = _maxHitPoints;
		}

		public override void ShowDisplay()
		{
			EnableCantBreakInstructions(false);
			EnableInstructions(true);
		}

		protected override void EnableProgressBar(bool _)
		{
			_progressBar.SetActive(_destructable ? _ : false);
		}

		protected override void EnableAmountDisplay(bool _)
		{
			_amountDisplay.SetActive(_destructable ? _ : false);
		}

		protected override void EnableInstructions(bool _)
		{
			_instructions.SetActive(_destructable ? _ : false);
		}
	}
}

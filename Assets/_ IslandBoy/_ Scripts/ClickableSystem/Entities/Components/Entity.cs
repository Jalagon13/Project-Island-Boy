using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IslandBoy
{
	public class Entity : Clickable
	{
		[Header("Base Entity Parameters")]
		[SerializeField] protected PlayerObject _pr;
		[SerializeField] private float _durationUntilDespawn;
		[MinMaxSlider(0, 99, true)]
		[SerializeField] private Vector2 _amount;

		protected KnockbackFeedback _knockback;
		private Timer _despawnTimer;

		protected override void Awake()
		{
			base.Awake();

			_knockback = GetComponent<KnockbackFeedback>();
			_despawnTimer = new(_durationUntilDespawn);
			_despawnTimer.OnTimerEnd += Despawn;

			GameSignals.DAY_END.AddListener(DestroyThisEntity);
			GameSignals.PLAYER_DIED.AddListener(DestroyThisEntity);
		}

		private void OnDisable()
		{
			Despawn();
		}
		
		private void OnDestroy()
		{
			GameSignals.DAY_END.RemoveListener(DestroyThisEntity);
			GameSignals.PLAYER_DIED.RemoveListener(DestroyThisEntity);
			_despawnTimer.OnTimerEnd -= Despawn;
		}

		protected virtual void Update()
		{
			_despawnTimer.Tick(Time.deltaTime);
		}

		private void Despawn()
		{
			Destroy(gameObject);
		}

		private void DestroyThisEntity(ISignalParameters parameters)
		{
			Destroy(gameObject);
		}

		public override bool OnHit(ToolType incomingToolType, int amount, bool displayHit = true)
		{
			_knockback.PlayFeedback(_pr.Position);
			_despawnTimer.RemainingSeconds = _durationUntilDespawn;

			if (base.OnHit(incomingToolType, amount, displayHit))
			{
				if (displayHit)
				{
					PopupMessage.Create(transform.position, amount.ToString(), Color.yellow, Vector2.up * 0.5f, 0.4f);
					UpdateAmountDisplay();
					UpdateFillImage();
					EnableAmountDisplay(false);
					EnableInstructions(false);
					EnableProgressBar(true);
				}
				return true;
			}

			return false;
		}

		public override void ShowDisplay()
		{
			UpdateAmountDisplay();
			UpdateFillImage();
			EnableInstructions(true);
			EnableAmountDisplay(true);
			EnableProgressBar(true);
		}

		public override void HideDisplay()
		{
			EnableProgressBar(true);
			EnableAmountDisplay(false);
			EnableInstructions(false);
		}

		public override void OnBreak()
		{
			_dropPosition = transform.position;
			
			GiveMoney();

			base.OnBreak();
		}
		
		public void GiveMoney()
		{
			int amount = Random.Range((int)_amount.x, (int)_amount.y);
			PlayerGoldController.Instance.AddCurrency(amount, transform.position);
		}
	}
}

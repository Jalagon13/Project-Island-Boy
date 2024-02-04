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
		[MinMaxSlider(0, 99, true)]
		[SerializeField] private Vector2 _amount;

		protected KnockbackFeedback _knockback;

		protected override void Awake()
		{
			base.Awake();

			_knockback = GetComponent<KnockbackFeedback>();

			GameSignals.DAY_END.AddListener(DestroyThisEntity);
			GameSignals.PLAYER_DIED.AddListener(DestroyThisEntity);
			GameSignals.MONSTER_HEART_CLEARED.AddListener(Despawn);
		}
		
		private void OnDestroy()
		{
			GameSignals.DAY_END.RemoveListener(DestroyThisEntity);
			GameSignals.PLAYER_DIED.RemoveListener(DestroyThisEntity);
			GameSignals.MONSTER_HEART_CLEARED.RemoveListener(Despawn);
		}

		private void Despawn(ISignalParameters parameters)
		{
			PlayDestroyFeedbacks();
			Destroy(gameObject);
		}
		
		private void PlayDestroyFeedbacks()
		{
			if (_destroyFeedback != null)
			{
				_destroyFeedback.transform.SetParent(null);
				_destroyFeedback?.PlayFeedbacks();
			}
		}

		private void DestroyThisEntity(ISignalParameters parameters)
		{
			Destroy(gameObject);
		}

		public bool Damage(ToolType incomingToolType, int amount, bool displayHit = true, bool kbEnabled = true)
		{
			if(kbEnabled)
				_knockback.PlayFeedback(_pr.Position);
			// _despawnTimer.RemainingSeconds = _durationUntilDespawn;

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
			GameSignals.MONSTER_KILLED.Dispatch();
			GiveMoney();

			base.OnBreak();
		}
		
		public void GiveMoney()
		{
			int amount = Random.Range((int)_amount.x, (int)_amount.y);
			PlayerGoldController.Instance.AddCurrency(amount, transform.position);
		}
		
		public void StartBurn()
		{
			StartCoroutine(Burn());
		}
		
		private IEnumerator Burn()
		{
			Damage(ToolType.Sword, 1);
			
			yield return new WaitForSeconds(0.5f);
			
			StartCoroutine(Burn());
		}
		
		public void StopBurn()
		{
			StopAllCoroutines();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
	public class Entity : Clickable
	{
		[Header("Base Entity Parameters")]
		[SerializeField] private string _entityName;
		[SerializeField] private int _xpAmount;
		[SerializeField] protected PlayerObject _pr;
		[SerializeField] private EntityRuntimeSet _entityRts;
		[SerializeField] private UnityEvent _onDamage;

		protected KnockbackFeedback _knockback;
		
		public int XpAmount => _xpAmount;
		public string EntityName => _entityName;

		protected override void Awake()
		{
			base.Awake();

			if(_entityRts != null)
				_entityRts.AddToList(this);

			_knockback = GetComponent<KnockbackFeedback>();
			
			GameSignals.DAY_END.AddListener(DestroyThisEntity);
			GameSignals.MONSTER_HEART_CLEARED.AddListener(Despawn);
		}
		
		private void OnDestroy()
		{
			if(_entityRts != null)
				_entityRts.RemoveFromList(this);
				
			GameSignals.DAY_END.RemoveListener(DestroyThisEntity);
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
			if (_entityName != "Monster Heart")
				Destroy(gameObject);
			else ResetHealth();
		}

		public override bool OnHit(ToolType incomingToolType, int amount, bool displayHit = true, ToolTier incomingToolTier = ToolTier.None)
		{
			if (!base.OnHit(incomingToolType, amount, displayHit, incomingToolTier)) 
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
		
		public bool Damage(ToolType incomingToolType, int amount, bool displayHit = true, bool kbEnabled = true, float strength = 5)
		{
			_onDamage?.Invoke();
			
			// if(kbEnabled)
			// 	_knockback.PlayFeedback(_pr.Position, strength);
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
			// EnableInstructions(true);
			// EnableAmountDisplay(true);
			// EnableProgressBar(true);
		}

		public override void HideDisplay()
		{
			EnableProgressBar(true);
			EnableAmountDisplay(false);
			EnableInstructions(false);
		}

		public override void OnBreak()
		{
			if (_entityName == "Treevil") // will only be set off if the entity is a Treevil
				GameSignals.TREEVIL_VANQUISHED.Dispatch();

			_dropPosition = transform.position;
			GameSignals.MONSTER_KILLED.Dispatch();
			GiveMoney();
			
			PlayerGoldController.Instance.AddCurrency(_maxHitPoints);

			base.OnBreak();
		}
		
		public void GiveMoney()
		{
			// int amount = Random.Range((int)_amount.x, (int)_amount.y);
			// PlayerGoldController.Instance.AddCurrency(amount, transform.position);
			
			Signal signal = GameSignals.ENTITY_DIED;
			signal.ClearParameters();
			signal.AddParameter("Entity", this);
			signal.Dispatch();
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

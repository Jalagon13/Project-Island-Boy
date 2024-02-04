using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class PlayerAttackCollider : MonoBehaviour
	{
		[SerializeField] private float _detectionBetweenHits;
		[SerializeField] private ItemParameter _damageParameter;

		private Slot _focusSlotRef;
		private List<Entity> _entitiesFoundThisSwing;
		private List<Entity> _entitiesHitThisSwing;
		private List<Resource> _rscFoundThisSwing;
		private List<Resource> _rscHitThisSwing;
		private int _damage;

		private void Awake()
		{
			GameSignals.FOCUS_SLOT_UPDATED.AddListener(UpdateDamage);
		}

		private void OnDestroy()
		{
			GameSignals.FOCUS_SLOT_UPDATED.RemoveListener(UpdateDamage);
		}

		private void OnEnable()
		{
			_entitiesFoundThisSwing = new();
			_entitiesHitThisSwing = new();
			_rscFoundThisSwing = new();
			_rscHitThisSwing = new();
			StartCoroutine(HitEnemies());
		}

		private void OnDisable()
		{
			_entitiesFoundThisSwing = new();
			_entitiesHitThisSwing = new();
			_rscFoundThisSwing = new();
			_rscHitThisSwing = new();
			StopAllCoroutines();
		}

		private IEnumerator HitEnemies()
		{
			if(_entitiesFoundThisSwing.Count > 0)
			{
				foreach (Entity entity in _entitiesFoundThisSwing.ToArray())
				{
					if (_entitiesHitThisSwing.Contains(entity)) continue;

					entity.Damage(ToolType.Sword, _damage);
					yield return new WaitForSeconds(_detectionBetweenHits);
					_entitiesFoundThisSwing.Remove(entity);
					_entitiesHitThisSwing.Add(entity);
				}
			}

			if(_rscFoundThisSwing.Count > 0)
			{
				foreach (Resource rsc in _rscFoundThisSwing.ToArray())
				{
					if (_rscHitThisSwing.Contains(rsc)) continue;

					_rscFoundThisSwing.Remove(rsc);
					_rscHitThisSwing.Add(rsc);
					
					if(rsc.SwingDestructOnly)
					{
						var gmod = rsc.GetComponent<GiveMoneyOnDestroy>();
						
						if(gmod != null)
						{
							gmod.GiveMoney();
						}
					}
					
					rsc.OnBreak();
					
					yield return new WaitForSeconds(_detectionBetweenHits);
				}
			}
			
			yield return null;

			StartCoroutine(HitEnemies());
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if(collision.TryGetComponent(out Entity entity))
			{
				_entitiesFoundThisSwing.Add(entity);
			}
			
			if(collision.TryGetComponent(out Resource rsc))
			{
				if(rsc.SwingDestructOnly)
				{
					_rscFoundThisSwing.Add(rsc);
				}
			}
		}

		private void UpdateDamage(ISignalParameters parameters)
		{
			if (parameters.HasParameter("FocusSlot"))
			{
				_focusSlotRef = (Slot)parameters.GetParameter("FocusSlot");
				if (_focusSlotRef == null) return;

				if (!_focusSlotRef.HasItem()) return;

				if(_focusSlotRef.ItemObject is ToolObject)
					_damage = ExtractDamage(_focusSlotRef.ItemObject);
			}
		}

		private int ExtractDamage(ItemObject item)
		{
			var itemParams = item.DefaultParameterList;

			if (itemParams.Contains(_damageParameter))
			{
				int index = itemParams.IndexOf(_damageParameter);
				return (int)itemParams[index].Value;
			}
			
			return 0;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class PlayerAttackCollider : MonoBehaviour
	{
		[SerializeField] private float _detectionBetweenHits;
		[SerializeField] private ItemParameter _damageMinParameter;
		[SerializeField] private ItemParameter _damageMaxParameter;
		[SerializeField] private ItemParameter _strengthParameter;

		private Slot _focusSlotRef;
		private List<Entity> _entitiesFoundThisSwing;
		private List<Entity> _entitiesHitThisSwing;
		private List<Resource> _rscFoundThisSwing;
		private List<Resource> _rscHitThisSwing;
		private int _damageMin;
		private int _damageMax;
		private float _strength;

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

					System.Random rnd = new System.Random();
					var damage = rnd.Next(_damageMin, _damageMax); 
					entity.Damage(ToolType.Sword, damage, strength:_strength);
					
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
				{
					_damageMin = ExtractMinDamage(_focusSlotRef.ItemObject);
					_damageMax = ExtractMaxDamage(_focusSlotRef.ItemObject);
					_strength = ExtractStrength(_focusSlotRef.ItemObject);
				}
			}
		}

		private int ExtractMinDamage(ItemObject item)
		{
			var itemParams = item.DefaultParameterList;

			if (itemParams.Contains(_damageMinParameter))
			{
				int index = itemParams.IndexOf(_damageMinParameter);
				return (int)itemParams[index].Value;
			}
			
			return 0;
		}
		
		private int ExtractMaxDamage(ItemObject item)
		{
			var itemParams = item.DefaultParameterList;

			if (itemParams.Contains(_damageMaxParameter))
			{
				int index = itemParams.IndexOf(_damageMaxParameter);
				return (int)itemParams[index].Value;
			}
			
			return 0;
		}
		
		private float ExtractStrength(ItemObject item)
		{
			var itemParams = item.DefaultParameterList;

			if (itemParams.Contains(_strengthParameter))
			{
				int index = itemParams.IndexOf(_strengthParameter);
				return itemParams[index].Value;
			}
			
			return 5;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class Surface : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private EntityRuntimeSet _entityRTS;
		[Header("Crab Spawn")]
		[SerializeField] private Entity _crabEntity;
		[SerializeField] private List<Transform> _crabSpawnPositions;
		[Header("Ghost Spawn")]
		[SerializeField] private Entity _ghostEntity;
		[SerializeField] private List<Transform> _ghostSpawnPositions;
		
		private bool _enabledStartingUI;
		
		private void Awake() 
		{
			GameSignals.DAY_START.AddListener(DayMonsterHandle);
			GameSignals.NIGHT_START.AddListener(NightMonsterHandle);
		}
		
		private void OnDestroy() 
		{
			GameSignals.DAY_START.RemoveListener(DayMonsterHandle);
			GameSignals.NIGHT_START.RemoveListener(NightMonsterHandle);
		}
		
		private void OnEnable()
		{
			if(!_enabledStartingUI)
			{
				StartCoroutine(Delay());
			}
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForSeconds(3f);
			_enabledStartingUI = true;
			GameSignals.ENABLE_STARTING_MECHANICS.Dispatch();
		}
		
		private IEnumerator Start() 
		{
			yield return new WaitForSeconds(.5f);
			DayMonsterHandle(null);
		}
		
		private void DayMonsterHandle(ISignalParameters parameters)
		{
			ClearMonsters();
			SpawnDayMonsters();
		}
		
		private void NightMonsterHandle(ISignalParameters parameters)
		{
			SpawnNightMonsters();
		}
		
		private void SpawnNightMonsters()
		{
			foreach (Transform tf in _ghostSpawnPositions)
			{
				Vector3 randomSpawnPoint = tf.position + (Vector3)Random.insideUnitCircle * 5;
				
				if(Vector2.Distance(randomSpawnPoint, _po.Position) < 10)
					continue;
				
				Instantiate(_ghostEntity, randomSpawnPoint, Quaternion.identity);
			}
		}
		
		private void SpawnDayMonsters()
		{
			foreach (Transform tf in _crabSpawnPositions)
			{
				Vector3 randomSpawnPoint = tf.position + (Vector3)Random.insideUnitCircle * 3;
				Instantiate(_crabEntity, randomSpawnPoint, Quaternion.identity);
			}
		}
		
		private void ClearMonsters()
		{
			if(_entityRTS.ListSize > 0)
			{
				foreach (Entity entity in _entityRTS.Items)
				{
					Destroy(entity.gameObject);
				}
				
				_entityRTS.Initialize();
			}
		}
	}
}

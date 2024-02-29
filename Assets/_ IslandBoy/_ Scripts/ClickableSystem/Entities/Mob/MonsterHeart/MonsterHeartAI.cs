using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
	public class MonsterHeartAI : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private Transform _forceFieldTf;
		[SerializeField] private MonsterHeartView _mhView;
		[SerializeField] private MonsterSpawner _monsterSpawner;
		[SerializeField] private Clickable _levelEntranceLower;
		[SerializeField] private float _heartBeatDelay = 2.5f;
		[SerializeField] private int _killQuota;
		[SerializeField] private int _heartBeatQuota;
		[Header("Monster Spawn")]
		[SerializeField] private int _maxMonsterAmount;
		[SerializeField] private EntityRuntimeSet _monsterRTS;
		[SerializeField] private List<SpawnSetting> _spawnSettings;
		[Header("Laser stuff")]
		[SerializeField] private Laser _laser;
		[SerializeField] private float _delayFire;
		[Header("Feel")]
		[SerializeField] private MMF_Player _heartBeatFeedbacks;
		[SerializeField] private MMF_Player _monsterSpawnFeedbacks;
		[SerializeField] private MMF_Player _forceFieldDestroyedFeedbacks;
		[SerializeField] private MMF_Player _monsterKilledFeedbacks;
		[SerializeField] private AudioClip _agroSound;
		
		private int _killCounter;
		private int _heartBeatCounter;
		private bool _forceFieldDown;
		private BoxCollider2D _rscCollider;
		
		[Serializable]
		public class SpawnSetting
		{
			[MinMaxSlider(0, 99, true)]
			public Vector2 SpawnAmount;
			public float BetweenSpawnTimer;
			public int MaxSpawnRange;
			public Entity MonsterToSpawn;
		}
		
		private void Awake() 
		{
			_rscCollider = transform.parent.GetComponent<BoxCollider2D>();
			_rscCollider.enabled = false;
			_mhView.UpdateText(_killCounter, _killQuota);
		}
		
		private IEnumerator HeartBeatRoutine()
		{
			yield return new WaitForSeconds(_heartBeatDelay);
			
			OnHeartBeat();
			
			StartCoroutine(HeartBeatRoutine());
		}
		
		private IEnumerator LaserSequence() 
		{
			yield return new WaitForSeconds(_delayFire);
			
			FireLaser();
			
			while(_laser.gameObject.activeInHierarchy)
			{
				yield return new WaitForEndOfFrame();
			}
			
			StartCoroutine(LaserSequence());
		}
		
		private void FireLaser()
		{
			_laser.gameObject.SetActive(true);
		}
		
		private void OnEnable() 
		{
			_mhView.UpdateText(_killCounter, _killQuota);
			StartCoroutine(HeartBeatRoutine());
			
			GameSignals.MONSTER_KILLED.AddListener(IncrementMonsterMeter);
		}
		
		private void OnDisable()
		{
			StopAllCoroutines();
			GameSignals.MONSTER_KILLED.RemoveListener(IncrementMonsterMeter);
		}
		
		private void OnDestroy() 
		{
			StopAllCoroutines();
			var go = Instantiate(_levelEntranceLower, transform.parent.position, Quaternion.identity);
			go.transform.SetParent(transform.parent.parent.transform);
			GameSignals.MONSTER_HEART_CLEARED.Dispatch();
		}
		
		private void OnHeartBeat()
		{
			_heartBeatFeedbacks?.PlayFeedbacks();
			_heartBeatCounter++;
			// _mhView.UpdateHeartBeatText(_heartBeatCounter, _heartBeatQuota);
			
			if(_heartBeatCounter >= _heartBeatQuota)
			{
				_mhView.ShowSpawningText();
				StartCoroutine(SpawnMonsters());
				_heartBeatCounter = 0;
			}
		}
		
		private IEnumerator SpawnMonsters()
		{
			_monsterSpawnFeedbacks?.PlayFeedbacks();
			
			foreach(SpawnSetting setting in _spawnSettings)
			{
				int amount = UnityEngine.Random.Range((int)setting.SpawnAmount.x, (int)setting.SpawnAmount.y + 1);
				
				for (int i = 0; i < amount; i++)
				{
					yield return new WaitForSeconds(setting.BetweenSpawnTimer);
					
					if(_monsterRTS.ListSize < _maxMonsterAmount)
					{
						_monsterSpawner.SpawnMonster(setting.MonsterToSpawn, setting.MaxSpawnRange);
					}
				}
			}
		}
		
		public void DropForceField()
		{
			_forceFieldTf.gameObject.SetActive(false);
			transform.GetChild(0).gameObject.SetActive(false);
			_rscCollider.enabled = true;
			_forceFieldDown = true;
			_mhView.DisableForceFieldUI();
			// _heartBeatDelay = 1.5f;
			StartCoroutine(LaserSequence());
			
			_forceFieldDestroyedFeedbacks?.PlayFeedbacks();
		}
		
		private void IncrementMonsterMeter(ISignalParameters parameters)
		{
			if(_forceFieldDown) return;
			
			_killCounter++;
			_monsterKilledFeedbacks?.StopFeedbacks();
			_monsterKilledFeedbacks?.PlayFeedbacks();
			// _mhView.UpdateText(_killCounter, _killQuota);
			
			if(_killCounter >= _killQuota)
			{
				DropForceField();
			}
		}
	}
}

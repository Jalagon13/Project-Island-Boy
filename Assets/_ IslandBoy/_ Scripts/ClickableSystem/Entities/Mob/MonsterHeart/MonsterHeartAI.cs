using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
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
		[SerializeField] private bool _spawnLowerLevelEntrance = true;
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
		[SerializeField] private Image _forceFieldProgress;	
		
		private int _killCounter;
		private int _heartBeatCounter;
		private bool _forceFieldDown;
		private BoxCollider2D _rscCollider;
		private SpriteRenderer _srCracks;
		
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
			GameSignals.DAY_END.AddListener(ResetMonsterHeart);
			_srCracks = transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
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
			GameSignals.MONSTER_HEART_CLEARED.Dispatch();

			if (_spawnLowerLevelEntrance)
			{
				var go = Instantiate(_levelEntranceLower, transform.parent.position, Quaternion.identity);
				go.transform.SetParent(transform.parent.parent.transform);
			}
			GameSignals.DAY_END.RemoveListener(ResetMonsterHeart);
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

		public void ResetForceField()
		{
			_forceFieldTf.gameObject.SetActive(true);
			transform.GetChild(0).gameObject.SetActive(true);
			_rscCollider.enabled = false;
			_forceFieldDown = false;
			
			_mhView.EnableForceFieldUI();
			StopAllCoroutines();
			_laser.gameObject.SetActive(false);
		}

		private void IncrementMonsterMeter(ISignalParameters parameters)
		{
			if(_forceFieldDown) return;
			
			_killCounter++;
			_monsterKilledFeedbacks?.StopFeedbacks();
			_monsterKilledFeedbacks?.PlayFeedbacks();
			// _mhView.UpdateText(_killCounter, _killQuota);
			_forceFieldProgress.fillAmount = _killCounter / (float)_killQuota;
			
			if(_killCounter >= _killQuota)
			{
				DropForceField();
			}
		}

		public void ResetMonsterHeart(ISignalParameters parameters)
		{
			_killCounter = 0;
			if (_forceFieldDown)
				ResetForceField();
			_monsterKilledFeedbacks?.StopFeedbacks();
			_srCracks.color = new Color(_srCracks.color.r, _srCracks.color.g, _srCracks.color.b, 0f);
			_forceFieldProgress.fillAmount = 0;
		}
	}
}

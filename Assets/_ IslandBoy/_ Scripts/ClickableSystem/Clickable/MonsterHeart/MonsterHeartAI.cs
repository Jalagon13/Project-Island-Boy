using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
	public class MonsterHeartAI : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private SpriteRenderer _indicatorSr;
		[SerializeField] private Transform _forceFieldTf;
		[SerializeField] private MonsterHeartView _mhView;
		[SerializeField] private MonsterSpawner _monsterSpawner;
		[SerializeField] private int _killQuota;
		[SerializeField] private int _heartBeatQuota;
		[Header("Laser stuff")]
		[SerializeField] private Laser _laser;
		[SerializeField] private float _delayFire;
		[Header("Monsters")]
		[SerializeField] private Entity _meleeMonster;
		[SerializeField] private Entity _laserMonster;
		[Header("Feel")]
		[SerializeField] private MMF_Player _heartBeatFeedbacks;
		[SerializeField] private MMF_Player _monsterSpawnFeedbacks;
		[SerializeField] private AudioClip _agroSound;
		
		private int _killCounter;
		private int _heartBeatCounter;
		private float _heartBeatDelay = 2.5f;
		private bool _forceFieldDown;
		private bool _threatened; // if threatened, spawn monsters
		private BoxCollider2D _rscCollider;
		private SpriteRenderer _sprite;
		
		private void Awake() 
		{
			UpdateAgroIndicator(new Color(0.5f, 0.1f, 0.65f, 0.25f));
			
			_rscCollider = transform.parent.GetComponent<BoxCollider2D>();
			_sprite = transform.parent.GetChild(0).GetComponent<SpriteRenderer>();
			_rscCollider.enabled = false;
			_mhView.UpdateText(_killCounter, _killQuota);
			_mhView.UpdateHeartBeatText(_heartBeatCounter, _heartBeatQuota);
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
			
			if(_threatened)
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
			StartCoroutine(HeartBeatRoutine());
			StartCoroutine(LaserSequence());
			GameSignals.MONSTER_KILLED.AddListener(IncrementMonsterMeter);
		}
		
		private void OnDisable()
		{
			StopAllCoroutines();
			GameSignals.MONSTER_KILLED.RemoveListener(IncrementMonsterMeter);
		}
		
		private void OnDestroy() 
		{
			GameSignals.MONSTER_HEART_CLEARED.Dispatch();
		}
		
		private void OnTriggerEnter2D(Collider2D other) 
		{
			if(other.TryGetComponent<CollectTag>(out var ct))
			{
				ActivateAgroPhase();
			}
		}
		
		private void ActivateAgroPhase()
		{
				UpdateAgroIndicator(new Color(1, 0, 0, 0.25f));
				_sprite.color = Color.red;
				_sprite.transform.localScale = new(1.2f, 1.2f, 1.2f);
				_threatened = true;
				_heartBeatDelay = 1f;
				
				MMSoundManagerSoundPlayEvent.Trigger(_agroSound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
				// need visual indicator it's agroed on player
				// line going to player maybe
		}
		
		private void OnTriggerExit2D(Collider2D other) 
		{
			if(other.TryGetComponent<CollectTag>(out var ct))
			{
				UpdateAgroIndicator(new Color(0.5f, 0.1f, 0.65f, 0.25f));
				_sprite.color = Color.white;
				_sprite.transform.localScale = new(1f, 1f, 1f);
				_threatened = false;
				_heartBeatDelay = 2.5f;
				
				// need visual indicator that its NOT agrod anymore
			}
		}
		
		private void OnHeartBeat()
		{
			_heartBeatFeedbacks?.PlayFeedbacks();
			_heartBeatCounter++;
			_mhView.UpdateHeartBeatText(_heartBeatCounter, _heartBeatQuota);
			
			if(_heartBeatCounter >= _heartBeatQuota)
			{
				StartCoroutine(SpawnMonsters());
				_heartBeatCounter = 0;
			}
		}
		
		private IEnumerator SpawnMonsters()
		{
			_monsterSpawnFeedbacks?.PlayFeedbacks();
			
			for (int i = 0; i < 8; i++)
			{
				yield return new WaitForSeconds(0.2f);
				_monsterSpawner.SpawnMonster(_meleeMonster);
			}
			
			yield return new WaitForSeconds(1f);
			
			for (int i = 0; i < 2; i++)
			{
				yield return new WaitForSeconds(1.25f);
				_monsterSpawner.SpawnMonster(_laserMonster);
			}
		}
		
		public void DropForceField()
		{
			_forceFieldTf.gameObject.SetActive(false);
			GetComponent<CircleCollider2D>().enabled = false;
			transform.GetChild(0).gameObject.SetActive(false);
			_rscCollider.enabled = true;
			_forceFieldDown = true;
			ActivateAgroPhase();
		}
		
		private void IncrementMonsterMeter(ISignalParameters parameters)
		{
			if(_forceFieldDown) return;
			
			_killCounter++;
			_mhView.UpdateText(_killCounter, _killQuota);
			
			if(_killCounter >= _killQuota)
			{
				DropForceField();
			}
		}
		
		private void UpdateAgroIndicator(Color color)
		{
			_indicatorSr.color = color;
		}
		
		
	}
}

using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace IslandBoy
{
	public class EvilHeart : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private float _threatRange;
		[SerializeField] private float _spawnTimer;
		// should have it's own way of implementing specific mosnters to spawn around the player set by the desigenr
		
		[Header("Audio")]
		[SerializeField] private AudioClip _agroSound;
		
		private bool _threatened; // if threatened, spawn monsters
		private Timer _monsterTimer;
		
		private void Awake() 
		{
			_monsterTimer = new Timer(_spawnTimer);
			_monsterTimer.OnTimerEnd += SpawnMonsters;
		}
		
		private void Update() 
		{
			if(_threatened)
			{
				_monsterTimer.Tick(Time.deltaTime);
			}
		}
		
		private void OnTriggerEnter2D(Collider2D other) 
		{
			if(other.TryGetComponent<CollectTag>(out var ct))
			{
				Debug.Log("Player In Range");
				_threatened = true;
				MMSoundManagerSoundPlayEvent.Trigger(_agroSound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
				// need visual indicator it's agroed on player
				// line going to player maybe
			}
		}
		
		private void OnTriggerExit2D(Collider2D other) 
		{
			if(other.TryGetComponent<CollectTag>(out var ct))
			{
				Debug.Log("Player Out of Range");
				_threatened = false;
				// need visual indicator that its NOT agrod anymore
			}
		}
		
		private void SpawnMonsters()
		{
			Debug.Log("Spawning Monsters");
			_monsterTimer.RemainingSeconds = _spawnTimer;
			// need visual indicator for mosnter spawning
		}
	}
}

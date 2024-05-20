using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class InitialSpawnPosition : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		
		private Vector2 _spawnPos;
		
		private void Awake()
		{
			_spawnPos = transform.position;
		}
		
		private void OnEnable()
		{
			StartCoroutine(Delay());
			GameSignals.PLAYER_RESPAWN_IN_CAVE.AddListener(OnPlayerRespawnInCave);
		}
		
		private void OnDisable() 
		{
			SetSpawnPos(_po.Position);
			GameSignals.PLAYER_RESPAWN_IN_CAVE.RemoveListener(OnPlayerRespawnInCave);
		}
		
		private void OnPlayerRespawnInCave(ISignalParameters parameters)
		{
			StartCoroutine(Delay());
		}
		
		public void SetSpawnPos(Vector2 pos)
		{
			_spawnPos = pos;
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForEndOfFrame();
			_po.GameObject.transform.position = _spawnPos;
		}
	}
}

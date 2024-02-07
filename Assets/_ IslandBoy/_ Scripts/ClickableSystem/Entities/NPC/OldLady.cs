using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace IslandBoy
{
	public class OldLady : MonoBehaviour
	{
		[SerializeField] private GameObject _spawnPrefab;
		[SerializeField] private MMF_Player _summonFeedbacks;
		
		public void SummonButton()
		{
			_summonFeedbacks?.PlayFeedbacks();
		}
		
		public void Summon()
		{
			Instantiate(_spawnPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}

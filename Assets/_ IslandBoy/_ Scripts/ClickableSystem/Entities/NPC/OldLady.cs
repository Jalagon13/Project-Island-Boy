using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
	public class OldLady : MonoBehaviour
	{
		[SerializeField] private GameObject _spawnPrefab;
		[SerializeField] private MMF_Player _summonFeedbacks;
		[SerializeField] private TextMeshProUGUI _promptText;
		[SerializeField] private UnityEvent _onReqsMet;
		[SerializeField] private UnityEvent _onSummon;
		
		private void OnEnable() 
		{
			_promptText.text = $"Mysterious Old Lady: I can't let you leave until you find all my children...<br><br>Settlers Found: {NpcSlots.SettlerCount}/3";
			
			if(NpcSlots.SettlerCount >= 3)
			{
				_onReqsMet?.Invoke();
			}
		}
		
		public void SummonButton()
		{
			_onSummon?.Invoke();
			_summonFeedbacks?.PlayFeedbacks();
		}
		
		public void Summon()
		{
			Instantiate(_spawnPrefab, transform.root.position, Quaternion.identity);
			Destroy(transform.root.gameObject);
		}
	}
}

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


        private void Awake()
        {
            GameSignals.DAY_START.AddListener(Respawn);
        }

        private void OnDestroy()
        {
            GameSignals.DAY_START.RemoveListener(Respawn);
        }

        private void OnEnable() 
		{
			_promptText.text = $"Mysterious Old Lady: Hello there youngling...Free the others and I'll give you a special treet?<br><br>Settlers Found: {NpcSlots.SettlerCount}/2";
			
			if(NpcSlots.SettlerCount >= 2)
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
			transform.root.gameObject.SetActive(false);
		}

		private void Respawn(ISignalParameters parameters)
		{
            transform.root.gameObject.SetActive(true);
        }
	}
}

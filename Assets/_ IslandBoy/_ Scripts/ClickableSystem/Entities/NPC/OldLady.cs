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
		[SerializeField] private string _startText;
        [SerializeField] private string _rematchText;
        [SerializeField] private string _vanquishedText;
        [SerializeField] private UnityEvent _onReqsMet;
		[SerializeField] private UnityEvent _onSummon;
		[SerializeField] private bool _bypassUnlockPrereq;

		private bool _beenSummoned, _beenVanquished;


        private void Awake()
        {
            GameSignals.DAY_START.AddListener(Respawn);
			GameSignals.TREEVIL_VANQUISHED.AddListener(VanquishListener);
        }

        private void OnDestroy()
        {
            GameSignals.DAY_START.RemoveListener(Respawn);
            GameSignals.TREEVIL_VANQUISHED.RemoveListener(VanquishListener);
        }

        private void OnEnable() 
		{
			if(!_beenSummoned)
				_promptText.text = _startText + $"<br><br>Settlers Housed: {NpcSlots.SettlerCount}/2";
			else if(!_beenVanquished)
				_promptText.text = _rematchText + $"<br><br>Settlers Housed: {NpcSlots.SettlerCount}/2";
			else
                _promptText.text = _vanquishedText + $"<br><br>Settlers Housed: {NpcSlots.SettlerCount}/2";

            if (NpcSlots.SettlerCount >= 2 || _bypassUnlockPrereq)
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
			_beenSummoned = true;
			_beenVanquished = false;
			transform.root.gameObject.SetActive(false);
		}

		private void Respawn(ISignalParameters parameters)
		{
            transform.root.gameObject.SetActive(true);
        }

		private void VanquishListener(ISignalParameters parameters)
		{
			_beenVanquished = true;
		}

    }
}

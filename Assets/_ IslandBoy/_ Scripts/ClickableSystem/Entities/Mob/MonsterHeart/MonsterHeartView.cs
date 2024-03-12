using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
	public class MonsterHeartView : MonoBehaviour
	{
		[SerializeField] private Image _fillImage;
		private TextMeshProUGUI _viewText;
		private TextMeshProUGUI _heartBeatText;
		
		private int _killCounter;
		private int _killQuota;
		private bool _showingSpawnText;
		
		private void Awake() 
		{
			_viewText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
			_heartBeatText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		}
		
		public void UpdateText(int killCounter, int killQuota)
		{
			if(_viewText == null || _showingSpawnText) return;
			
			// _killCounter = killCounter;
			// _killQuota = killQuota;
			// _viewText.enabled = false;
			// _fillImage.fillAmount = ((float)killQuota - (float)killCounter) / (float)killQuota;
			// _viewText.text = killCounter >= killQuota ? $"Force Field Down!" : $"Monster Quota: {killCounter} / {killQuota}";
			// _fillImage.fillAmount = (float)killCounter / (float)killQuota;
		}
		
		public void ShowSpawningText()
		{
			// StartCoroutine(SpawningText());
		}
		
		private IEnumerator SpawningText()
		{
			_viewText.text = "Spawning Monsters!";
			_showingSpawnText = true;
			yield return new WaitForSeconds(2);
			_showingSpawnText = false;
			_viewText.text = _killCounter >= _killQuota ? $"Force Field Down!" : $"Monster Quota: {_killCounter} / {_killQuota}";
		}
		
		public void DisableForceFieldUI()
		{
			_fillImage.transform.parent.gameObject.SetActive(false);
		}

		public void EnableForceFieldUI()
		{
			_fillImage.transform.parent.gameObject.SetActive(true);
		}

		// public void UpdateHeartBeatText(int beatCounter, int beatQuota)
		// {
		// 	_fillImage.fillAmount = (float)beatCounter / (float)beatQuota;
		// 	// _heartBeatText.text = $"Heart Beats: {beatCounter} / {beatQuota}";
		// }
	}
}

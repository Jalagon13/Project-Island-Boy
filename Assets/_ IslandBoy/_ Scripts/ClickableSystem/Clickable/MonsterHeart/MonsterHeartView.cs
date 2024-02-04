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
		
		private void Awake() 
		{
			_viewText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
			_heartBeatText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		}
		
		public void UpdateText(int killCounter, int killQuota)
		{
			_viewText.text = killCounter >= killQuota ? $"Force Field Down!" : $"Monster Quota: {killCounter} / {killQuota}";
		}
		
		public void UpdateHeartBeatText(int beatCounter, int beatQuota)
		{
			_fillImage.fillAmount = (float)beatCounter / (float)beatQuota;
			// _heartBeatText.text = $"Heart Beats: {beatCounter} / {beatQuota}";
		}
	}
}

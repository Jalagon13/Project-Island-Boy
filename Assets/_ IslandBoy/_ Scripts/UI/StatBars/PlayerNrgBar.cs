using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
	public class PlayerNrgBar : MonoBehaviour
	{
		[SerializeField] private MMF_Player _hungryFeedback;
		
		private int _currentNrg;
		private int _maxNrg;

		private Image _fillImage;
		private TextMeshProUGUI _counter;

		private void Awake()
		{
			_fillImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
			_counter = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

			GameSignals.PLAYER_NRG_CHANGED.AddListener(UpdateEnergyUI);
			GameSignals.PLAYER_HUNGRY_WARNING.AddListener(HungerGameFeel);
		}

		private void OnDestroy()
		{
			GameSignals.PLAYER_NRG_CHANGED.RemoveListener(UpdateEnergyUI);
			GameSignals.PLAYER_HUNGRY_WARNING.RemoveListener(HungerGameFeel);
		}
		
		private void HungerGameFeel(ISignalParameters parameters)
		{
			_hungryFeedback?.PlayFeedbacks();
		}

		private void UpdateEnergyUI(ISignalParameters parameters)
		{
			_currentNrg = (int)parameters.GetParameter("CurrentNrg");
			_maxNrg = (int)parameters.GetParameter("MaxNrg");

			_fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _maxNrg, _currentNrg));
			_counter.text = _currentNrg.ToString();
		}
	}
}

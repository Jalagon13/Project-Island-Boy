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
		[SerializeField] private PlayerObject _pr;
		[SerializeField] private MMF_Player _hungryFeedback;
		
		private int _currentNrg;
		private int _maxNrg;
		private bool _hungerQuotaFilled;

		private Image _fillImage;

		private void Awake()
		{
			_fillImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();

			GameSignals.PLAYER_NRG_CHANGED.AddListener(UpdateEnergyUI);
			GameSignals.PLAYER_HUNGRY_WARNING.AddListener(HungerGameFeel);
			GameSignals.DAY_START.AddListener(ResetHunger);
			_pr.GameObject.GetComponent<Player>().DispatchNrgChange();
		}

		private void OnDestroy()
		{
			GameSignals.PLAYER_NRG_CHANGED.RemoveListener(UpdateEnergyUI);
			GameSignals.PLAYER_HUNGRY_WARNING.RemoveListener(HungerGameFeel);
			GameSignals.DAY_START.RemoveListener(ResetHunger);
		}
		
		private void HungerGameFeel(ISignalParameters parameters)
		{
			// _hungryFeedback?.PlayFeedbacks();
		}
		
		private void ResetHunger(ISignalParameters parameters)
		{
			_hungerQuotaFilled = false;
		}

		private void UpdateEnergyUI(ISignalParameters parameters)
		{
			_currentNrg = (int)parameters.GetParameter("CurrentNrg");
			_maxNrg = (int)parameters.GetParameter("MaxNrg");

			_fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _maxNrg, _currentNrg));
			
			if(_currentNrg >= _maxNrg && !_hungerQuotaFilled)
			{
				GameSignals.HUNGER_RESTORED.Dispatch();
				Debug.Log("Hunger Restored");
				_hungerQuotaFilled = true;
			}
		}
	}
}

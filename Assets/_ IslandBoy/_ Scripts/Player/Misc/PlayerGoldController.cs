using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
	public class PlayerGoldController : MonoBehaviour
	{
		[SerializeField] private AudioClip _goldSound;
		[SerializeField] private TextMeshProUGUI _goldViewText;
		[SerializeField] private MMF_Player _tutorialFeedback;

		private static PlayerGoldController _instance;
		
		[InfoBox("Debug Money button")]
		[Button]
		private void PlusHundred()
		{
			AddCurrency(100);
		}
		
		private int _changeAmount;
		
		private GoldCurrency _currency = new();
		
		public static PlayerGoldController Instance => _instance;
		public int CurrencyValue { get { return _currency.CurrentValue; } }
		
		private void Awake()
		{
			_currency = new();
			_instance = this;

			GameSignals.ENABLE_STARTING_MECHANICS.AddListener(EnableGoldDisplay);
			_goldViewText.transform.parent.gameObject.SetActive(false);
		}
		
		private void OnDestroy()
		{
			if(_currency != null)
			{
				_currency.ValueIncreased += UpdateView;
				_currency.ValueDecreased += UpdateView;
			}

			GameSignals.ENABLE_STARTING_MECHANICS.RemoveListener(EnableGoldDisplay);
		}
		
		private void Start()
		{
			if(_currency != null)
			{
				_currency.ValueIncreased += UpdateView;
				_currency.ValueDecreased += UpdateView;
			}
		}
		
		private void EnableGoldDisplay(ISignalParameters parameters)
		{
			_tutorialFeedback?.PlayFeedbacks();
		}
		
		public void AddCurrency(int amount, Vector2 popupPosition = default)
		{
			if(popupPosition == default)
				popupPosition = transform.position;
				
			PopupMessage.Create(popupPosition, $"+${amount}", Color.green, Vector2.up, 1f);
			MMSoundManagerSoundPlayEvent.Trigger(_goldSound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
			_changeAmount = amount;
			_currency?.Increment(amount);
		}
		
		public void SubtractCurrency(int amount)
		{
			_changeAmount = amount;
			_currency?.Decrement(amount);
		}
		
		private void UpdateView()
		{
			if(_currency == null)
				return;
			
			if(_goldViewText != null)
			{
				_goldViewText.text = _currency.CurrentValue.ToString();
			}
		}
	}
}

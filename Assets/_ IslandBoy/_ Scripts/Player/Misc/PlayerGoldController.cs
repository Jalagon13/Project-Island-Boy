using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
	public class PlayerGoldController : MonoBehaviour
	{
		[SerializeField] private AudioClip _goldSound;
		[SerializeField] private TextMeshProUGUI _goldViewText;
		
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
			
			GameSignals.INVENTORY_OPEN.AddListener(EnableGoldDisplay);
			GameSignals.INVENTORY_CLOSE.AddListener(DisableGoldDisplay);
		}
		
		private void OnDestroy()
		{
			if(_currency != null)
			{
				_currency.ValueIncreased += UpdateView;
				_currency.ValueDecreased += UpdateView;
			}
			
			GameSignals.INVENTORY_OPEN.RemoveListener(EnableGoldDisplay);
			GameSignals.INVENTORY_CLOSE.RemoveListener(DisableGoldDisplay);
		}
		
		private void Start()
		{
			if(_currency != null)
			{
				_currency.ValueIncreased += UpdateView;
				_currency.ValueDecreased += UpdateView;
			}
			
			DisableGoldDisplay(null);
		}
		
		private void EnableGoldDisplay(ISignalParameters parameters)
		{
			_goldViewText.transform.parent.gameObject.SetActive(true);
		}
		
		private void DisableGoldDisplay(ISignalParameters parameters)
		{
			_goldViewText.transform.parent.gameObject.SetActive(false);
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

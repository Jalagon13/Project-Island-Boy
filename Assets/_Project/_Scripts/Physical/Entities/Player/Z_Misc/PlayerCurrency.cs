using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class PlayerCurrency : MonoBehaviour
    {
        [SerializeField] private int _startingCurrency;
        [Range(0,1)]
        [SerializeField] private float _percentKeptAfterDeath;

        private static Currency _currency;
        private TextMeshProUGUI _coinText;

        private void Awake()
        {
            GameSignals.PLAYER_DIED.AddListener(OnDeath);

            _coinText = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
            _currency = new();
            _currency.OnCurrencyChanged += UpdateGoldDisplay;
        }

        private void OnDestroy()
        {
            GameSignals.PLAYER_DIED.RemoveListener(OnDeath);

            _currency.OnCurrencyChanged -= UpdateGoldDisplay;
        }

        private void Start()
        {
            _currency.Add(_startingCurrency);
        }

        public static void AddCurrency(int amount)
        {
            _currency.Add(amount);
        }

        private void UpdateGoldDisplay(object sender, EventArgs e)
        {
            _coinText.text = _currency.Count.ToString();
        }

        public void OnDeath(ISignalParameters parameters)
        {
            int newCurrency = Mathf.RoundToInt(_currency.Count * _percentKeptAfterDeath);

            _currency.Set(newCurrency);
        }
    }
}

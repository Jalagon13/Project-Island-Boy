using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class PlayerCurrency : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private int _startingCurrency;
        [Range(0,1)]
        [SerializeField] private float _percentKeptAfterDeath;
        [SerializeField] private TextMeshProUGUI _coinText;
        private Currency _currency;

        private void Awake()
        {
            _currency = new();
            _pr.Currency = _currency;
        }

        private void OnEnable()
        {
            _pr.Currency.OnCurrencyChanged += UpdateGoldDisplay;
        }

        private void OnDisable()
        {
            _pr.Currency.OnCurrencyChanged -= UpdateGoldDisplay;
        }

        private void Start()
        {
            _pr.Currency.Add(_startingCurrency);
        }

        private void UpdateGoldDisplay(object sender, EventArgs e)
        {
            Currency currency = (Currency)sender;

            //_coinText.text = currency.Count.ToString();
        }

        public void OnDeath()
        {
            float subtractAmt = _currency.Count * (1 - _percentKeptAfterDeath);

            _pr.Currency.Subtract(Mathf.RoundToInt(subtractAmt));
        }
    }
}

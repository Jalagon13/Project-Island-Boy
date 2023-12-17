using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class PlayerExperience : MonoBehaviour
    {
        [SerializeField] private int _startingXp;
        [Range(0,1)]
        [SerializeField] private float _percentKeptAfterDeath;

        private static Experience _experience;
        private TextMeshProUGUI _experienceText;

        public static Experience Experience { get { return _experience; } }

        private void Awake()
        {
            GameSignals.PLAYER_DIED.AddListener(OnDeath);

            _experienceText = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
            _experience = new();
            _experience.OnCurrencyChanged += UpdateExperienceDisplay;
        }

        private void OnDestroy()
        {
            GameSignals.PLAYER_DIED.RemoveListener(OnDeath);

            _experience.OnCurrencyChanged -= UpdateExperienceDisplay;
        }

        private void Start()
        {
            _experience.Add(_startingXp);
        }

        public static void AddExerpience(int amount)
        {
            _experience.Add(amount);
        }

        private void UpdateExperienceDisplay(object sender, EventArgs e)
        {
            _experienceText.text = _experience.Count.ToString();
        }

        public void OnDeath(ISignalParameters parameters)
        {
            int newCurrency = Mathf.RoundToInt(_experience.Count * _percentKeptAfterDeath);

            _experience.Set(newCurrency);
        }
    }
}

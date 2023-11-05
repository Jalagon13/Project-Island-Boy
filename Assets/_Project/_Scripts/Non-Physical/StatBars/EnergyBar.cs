using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class EnergyBar : Bar
    {
        protected override void Awake()
        {
            base.Awake();
            GameSignals.SWING_PERFORMED.AddListener(DrainEnergy);
            GameSignals.OBJECT_LAUNCHED.AddListener(DrainEnergy);
            GameSignals.DAY_START.AddListener(RestoreStat);
        }

        private void OnDestroy()
        {
            GameSignals.SWING_PERFORMED.RemoveListener(DrainEnergy);
            GameSignals.OBJECT_LAUNCHED.RemoveListener(DrainEnergy);
            GameSignals.DAY_START.RemoveListener(RestoreStat);
        }

        private void RestoreStat(ISignalParameters parameters)
        {
            AddTo(999);
        }

        private void DrainEnergy(ISignalParameters parameters)
        {
            _currentValue--;

            if (_currentValue <= 0)
            {
                _currentValue = 0;
            }

            UpdateUI();
        }
    }
}

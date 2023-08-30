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


        private void OnEnable()
        {
            ActionControl.SwingPerformEvent += DrainEnergy;
            LaunchObject.LaunchEvent += DrainEnergy;
        }

        private void OnDisable()
        {
            ActionControl.SwingPerformEvent -= DrainEnergy;
            LaunchObject.LaunchEvent -= DrainEnergy;
        }

        private void DrainEnergy()
        {
            _currentValue--;

            if (_currentValue <= 0)
            {
                _currentValue = 0;
                _onValueDepleted?.Invoke();
            }

            UpdateUI();
        }
    }
}

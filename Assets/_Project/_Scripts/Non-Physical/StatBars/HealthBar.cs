using System.Collections;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class HealthBar : Bar
    {
        protected override void Awake()
        {
            base.Awake();

            GameSignals.DAY_START.AddListener(RestoreStat);
        }

        private void OnDestroy()
        {
            GameSignals.DAY_START.RemoveListener(RestoreStat);
        }

        private void RestoreStat(ISignalParameters parameters)
        {
            AddTo(999);
        }


    }
}

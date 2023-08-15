using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class UndergroundCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counterText;

        private int _levelCounter = 0;

        private void Start()
        {
            _counterText.text = $"Level {_levelCounter}";
        }

        public void IncrementLevelCounter()
        {
            _levelCounter++;

            _counterText.text = $"Level {_levelCounter}";
        }
    }
}

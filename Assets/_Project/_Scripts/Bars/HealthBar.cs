using System.Collections;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class HealthBar : Bar
    {
        public IEnumerator DrainHp()
        {
            yield return new WaitForSeconds(0.5f);
            _currentValue--;
            UpdateUI();
            StartCoroutine(DrainHp());
        }
    }
}

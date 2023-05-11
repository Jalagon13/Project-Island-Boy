using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class DurabilityCounter : MonoBehaviour
    {
        private Image _fillImage;

        private void Awake()
        {
            _fillImage = transform.GetChild(1).GetComponent<Image>();
        }

        public void UpdateDurabilityCounter(int maxDurability, int currentDurability)
        {
            _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, maxDurability, currentDurability));
        }
    }
}

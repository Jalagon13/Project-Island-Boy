using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class FillControl : MonoBehaviour
    {
        private Image _fillImage;

        private void Awake()
        {
            _fillImage = transform.GetChild(1).GetComponent<Image>();
        }

        public void ShowFill()
        {
            _fillImage.enabled = true;
        }

        public void HideFill()
        {
            _fillImage.enabled = false;
        }

        public void UpdateFill(float maxFill, float currentFill)
        {
            _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, maxFill, currentFill));
        }
    }
}

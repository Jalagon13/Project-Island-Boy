using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class PlayerHpBar : MonoBehaviour
    {
        private int _currentHp;
        private int _maxHp;

        private Image _fillImage;
        private TextMeshProUGUI _counter;

        private void Awake()
        {
            _fillImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            _counter = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            GameSignals.PLAYER_HP_CHANGED.AddListener(UpdateHealthUI);
        }

        private void OnDestroy()
        {
            GameSignals.PLAYER_HP_CHANGED.RemoveListener(UpdateHealthUI);
        }

        private void UpdateHealthUI(ISignalParameters parameters)
        {
            _currentHp = (int)parameters.GetParameter("CurrentHp");
            _maxHp = (int)parameters.GetParameter("MaxHp");

            _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _maxHp, _currentHp));
            _counter.text = _currentHp.ToString();
        }
    }
}

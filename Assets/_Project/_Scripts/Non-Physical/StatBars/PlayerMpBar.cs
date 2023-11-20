using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class PlayerMpBar : MonoBehaviour
    {
        private int _currentMp;
        private int _maxMp;

        private Image _fillImage;
        private TextMeshProUGUI _counter;

        private void Awake()
        {
            _fillImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            _counter = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            GameSignals.PLAYER_MP_CHANGED.AddListener(UpdateMpUI);
        }

        private void OnDestroy()
        {
            GameSignals.PLAYER_MP_CHANGED.RemoveListener(UpdateMpUI);
        }

        private void UpdateMpUI(ISignalParameters parameters)
        {
            _currentMp = (int)parameters.GetParameter("CurrentMp");
            _maxMp = (int)parameters.GetParameter("MaxMp");

            _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _maxMp, _currentMp));
            _counter.text = _currentMp.ToString();
        }
    }
}

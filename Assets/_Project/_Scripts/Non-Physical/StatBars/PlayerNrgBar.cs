using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class PlayerNrgBar : MonoBehaviour
    {
        private int _currentNrg;
        private int _maxNrg;
        private float _cooldownDuration;

        private Timer _cooldownTimer;
        private Image _fillImage;
        private Image _cdFillImage;
        private TextMeshProUGUI _counter;

        private void Awake()
        {
            _fillImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            _counter = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            _cdFillImage = transform.GetChild(3).GetComponent<Image>();
            _cdFillImage.enabled = false;
            _cooldownTimer = new(0);
            _cooldownDuration = 0;

            GameSignals.PLAYER_NRG_CHANGED.AddListener(UpdateEnergyUI);
        }

        private void OnDestroy()
        {
            GameSignals.PLAYER_NRG_CHANGED.RemoveListener(UpdateEnergyUI);
        }

        private void UpdateEnergyUI(ISignalParameters parameters)
        {
            _currentNrg = (int)parameters.GetParameter("CurrentNrg");
            _maxNrg = (int)parameters.GetParameter("MaxNrg");

            if (parameters.HasParameter("NrgTimer"))
            {
                _cooldownTimer = (Timer)parameters.GetParameter("NrgTimer");
                _cooldownDuration = _cooldownTimer.RemainingSeconds;

                if (_cooldownTimer.RemainingSeconds > 0)
                {
                    StartCoroutine(Cooldown());
                }
            }

            _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _maxNrg, _currentNrg));
            _counter.text = _currentNrg.ToString();
        }

        private IEnumerator Cooldown()
        {
            _cdFillImage.enabled = true;
            while (_cooldownTimer.RemainingSeconds > 0)
            {
                yield return new WaitForSeconds(0.01f);
                UpdateCoolDownUI();
            }
            _cdFillImage.enabled = false;
        }

        private void UpdateCoolDownUI()
        {
            _cdFillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _cooldownDuration, _cooldownTimer.RemainingSeconds));
        }
    }
}

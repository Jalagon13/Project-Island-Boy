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

            if (parameters.HasParameter("MpTimer"))
            {
                _cooldownTimer = (Timer)parameters.GetParameter("MpTimer");
                _cooldownDuration = _cooldownTimer.RemainingSeconds;

                if (_cooldownTimer.RemainingSeconds > 0)
                {
                    StartCoroutine(Cooldown());
                }
            }

            _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _maxMp, _currentMp));
            _counter.text = _currentMp.ToString();
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

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

            if(parameters.HasParameter("HpTimer"))
            {
                _cooldownTimer = (Timer)parameters.GetParameter("HpTimer");
                _cooldownDuration = _cooldownTimer.RemainingSeconds;

                if (_cooldownTimer.RemainingSeconds > 0)
                {
                    StartCoroutine(Cooldown());
                }
            }

            _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _maxHp, _currentHp));
            _counter.text = _currentHp.ToString();
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

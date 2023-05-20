using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class Bar : MonoBehaviour
    {
        [SerializeField] private int _maxValue;
        [SerializeField] private float _coolDown;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _cdFillImage;
        [SerializeField] private TextMeshProUGUI _counter;

        protected int _currentValue;
        private float _cdCounter;

        public int CurrentValue { get { return _currentValue; } }
        public int MaxValue { get { return _maxValue; } }
        public bool InCoolDown { get { return _cdCounter < _coolDown; } }

        private void Awake()
        {
            _currentValue = _maxValue;
            _cdFillImage.enabled = false;
        }

        private void Start()
        {
            UpdateUI();
        }

        private void FixedUpdate()
        {
            _cdCounter += Time.deltaTime;

            if (_cdCounter > _coolDown)
                _cdCounter = _coolDown;
        }

        public void AddTo(int value)
        {
            _currentValue += value;

            if (_currentValue > _maxValue)
                _currentValue = _maxValue;

            StartCoroutine(Cooldown());
            UpdateUI();
        }

        private IEnumerator Cooldown()
        {
            _cdCounter = 0f;
            _cdFillImage.enabled = true;
            while (_cdCounter < _coolDown)
            {
                yield return new WaitForSeconds(0.01f);
                UpdateCoolDownUI();
            }
            _cdFillImage.enabled = false;
        }

        private void UpdateCoolDownUI()
        {
            _cdFillImage.fillAmount = Mathf.Clamp01(1 - Mathf.InverseLerp(0, _coolDown, _cdCounter));
        }
        protected void UpdateUI()
        {
            _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _maxValue, _currentValue));
            _counter.text = _currentValue.ToString();
        }
    }
}

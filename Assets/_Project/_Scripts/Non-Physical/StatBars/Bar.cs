using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IslandBoy
{
    public class Bar : MonoBehaviour
    {
        [SerializeField] protected ConsumeType _consumeType;
        [SerializeField] protected int _maxValue;
        [SerializeField] protected float _coolDown;

        private Image _fillImage;
        private Image _cdFillImage;
        private TextMeshProUGUI _counter;
        protected int _currentValue;
        protected float _cdCounter;

        protected virtual void Awake()
        {
            GameSignals.CONSUME_ITEM_TRY.AddListener(ProcessItemConsumed);

            _fillImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            _cdFillImage = transform.GetChild(3).GetComponent<Image>();
            _counter = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            _currentValue = _maxValue;
            _cdFillImage.enabled = false;
        }

        private void OnDestroy()
        {
            GameSignals.CONSUME_ITEM_TRY.RemoveListener(ProcessItemConsumed);
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

        private void ProcessItemConsumed(ISignalParameters parameters)
        {
            ConsumableObject consumedItem = (ConsumableObject)parameters.GetParameter("ConsumeItem");

            if (consumedItem.ConsumeType != _consumeType || _currentValue >= _maxValue || InCoolDown())
                return;

            GameSignals.CONSUME_ITEM_SUCCESS.Dispatch();
            AudioManager.Instance.PlayClip(consumedItem.ConsumeSound, false, false);
            AddTo(consumedItem.ConsumeValue);
        }

        protected void ResetValue()
        {
            _currentValue = _maxValue;
            UpdateUI();
        }

        protected bool InCoolDown()
        {
            return _cdCounter < _coolDown;
        }

        public virtual void AddTo(int value)
        {
            _currentValue += value;

            // need to create screen popup for this
            PopupMessage.Create(transform.root.position, $"+{value} {_consumeType}", Color.green, new(0, 0.5f), 1f);

            if (_currentValue > _maxValue)
                _currentValue = _maxValue;

            if(value > 0)
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

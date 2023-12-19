using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public abstract class Clickable : MonoBehaviour
    {
        [Header("Base Clickable Parameters")]
        [SerializeField] protected int _maxHitPoints;
        [SerializeField] protected ToolType _breakType;
        [SerializeField] protected MMF_Player _clickFeedback;
        [SerializeField] protected MMF_Player _destroyFeedback;
        [SerializeField] protected LootTable _lootTable;

        protected int _currentHitPoints;
        protected GameObject _progressBar;
        protected GameObject _amountDisplay;
        protected GameObject _instructions;
        protected SpriteRenderer _sr;
        protected Vector2 _dropPosition;

        public ToolType BreakType { get { return _breakType; } }
        public MMF_Player ClickFeedback { get { return _clickFeedback; } }

        protected virtual void Awake()
        {
            _currentHitPoints = _maxHitPoints;
            _progressBar = transform.GetChild(2).GetChild(0).gameObject;
            _amountDisplay = transform.GetChild(2).GetChild(1).gameObject;
            _instructions = transform.GetChild(2).GetChild(2).gameObject;
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _dropPosition = transform.position + (Vector3.one * 0.5f);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            //if (collision.TryGetComponent<CursorControl>(out var cc))
            //{
            //    ShowDisplay();
            //}
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<CursorControl>(out var cc))
            {
                HideDisplay();
            }
        }

        public virtual bool OnHit(ToolType incomingToolType, int amount, bool displayHit = true)
        {

            if (incomingToolType != _breakType || incomingToolType == ToolType.None)
            {
                return false;
            }

            _clickFeedback?.PlayFeedbacks();
            _currentHitPoints -= amount;

            GameSignals.CLICKABLE_CLICKED.Dispatch();

            if (_currentHitPoints <= 0)
                OnBreak();

            return true;
        }

        protected virtual void OnBreak()
        {
            if (_destroyFeedback != null)
            {
                _destroyFeedback.transform.SetParent(null);
                _destroyFeedback?.PlayFeedbacks();
            }

            _lootTable.SpawnLoot(_dropPosition);
            StopAllCoroutines();
            Destroy(gameObject);
        }

        public abstract void ShowDisplay();
        public virtual void HideDisplay()
        {
            EnableProgressBar(false);
            EnableAmountDisplay(false);
            EnableInstructions(false);
        }

        protected virtual void EnableProgressBar(bool _)
        {
            _progressBar.SetActive(_);
        }

        protected virtual void EnableAmountDisplay(bool _)
        {
            _amountDisplay.SetActive(_);
        }

        protected virtual void EnableInstructions(bool _)
        {
            _instructions.SetActive(_);
        }

        protected virtual void UpdateFillImage()
        {
            var fillImage = _progressBar.transform.GetChild(1).GetComponent<Image>();
            fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _maxHitPoints, _currentHitPoints));
        }

        protected virtual void UpdateAmountDisplay()
        {
            var amountText = _amountDisplay.GetComponent<TextMeshProUGUI>();
            amountText.text = _currentHitPoints.ToString();
        }
    }
}

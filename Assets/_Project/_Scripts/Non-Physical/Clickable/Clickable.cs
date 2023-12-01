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
        [SerializeField] private bool _dontGiveXp = false;
        [SerializeField] protected int _maxHitPoints;
        [SerializeField] protected ToolType _breakType;
        [SerializeField] protected MMF_Player _clickFeedback;
        [SerializeField] protected MMF_Player _destroyFeedback;
        [SerializeField] protected LootTable _lootTable;

        protected int _currentHitPoints;
        private Timer _timer;
        protected GameObject _progressBar;
        protected GameObject _amountDisplay;
        protected GameObject _instructions;
        protected GameObject _yellowCorners;
        protected Vector2 _dropPosition;

        public int MaxHitPoints { get { return _maxHitPoints; } set { _maxHitPoints = value; } }
        public int CurrentHitPoints { get { return _currentHitPoints; } set { _currentHitPoints = value; } }
        public ToolType BreakType { get { return _breakType; } set { _breakType = value; } }

        protected virtual void Awake()
        {
            _timer = new(3f);
            _timer.RemainingSeconds = 0;
            _currentHitPoints = _maxHitPoints;
            _progressBar = transform.GetChild(2).GetChild(0).gameObject;
            _amountDisplay = transform.GetChild(2).GetChild(1).gameObject;
            _instructions = transform.GetChild(2).GetChild(2).gameObject;
            _yellowCorners = transform.GetChild(2).GetChild(3).gameObject;
        }

        private void Update()
        {
            _timer.Tick(Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("CursorControl")) return;
            ShowDisplay();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("CursorControl")) return;
            HideDisplay();
        }

        public virtual bool OnHit(ToolType incomingToolType, int amount, bool displayHit = true)
        {
            _clickFeedback?.PlayFeedbacks();

            if (incomingToolType != _breakType || incomingToolType == ToolType.None)
            {
                if(displayHit && _timer.RemainingSeconds == 0)
                {
                    PopupMessage.Create(transform.position, $"I need a {_breakType} to hit this", Color.red, Vector2.one, 1f);
                    _timer.RemainingSeconds = 3;
                }

                return false;
            }

            _currentHitPoints -= amount;

            GameSignals.CLICKABLE_CLICKED.Dispatch();

            if (_currentHitPoints <= 0)
                OnBreak();

            return true;
        }

        protected virtual void OnBreak()
        {
            if (!_dontGiveXp)
            {
                PopupMessage.Create(transform.position, $"+ {_maxHitPoints} XP", Color.white, Vector2.up, 1f);
                PlayerExperience.AddExerpience(_maxHitPoints);
            }

            if (_destroyFeedback != null)
            {
                _destroyFeedback.transform.SetParent(null);
                _destroyFeedback?.PlayFeedbacks();
            }

            _lootTable.SpawnLoot(_dropPosition);
        }

        public abstract void ShowDisplay();
        public abstract void HideDisplay();

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

        protected virtual void EnableYellowCorners(bool _)
        {
            _yellowCorners.SetActive(_);
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

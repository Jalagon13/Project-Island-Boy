using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

namespace IslandBoy
{
    [SelectionBase]
    public class Resource : Clickable
    {
        [Header("Base Resource Parameters")]
        [SerializeField] protected bool _destructable = true;

        public string ResourceName { get { return null; } }

        private Timer _restoreHpTimer;

        protected override void Awake()
        {
            base.Awake();

            _restoreHpTimer = new(5);
            _restoreHpTimer.OnTimerEnd += RestoreHitPoints;
            _dropPosition = transform.position + new Vector3(0.5f, 0.5f, 0);
        }

        private void OnDestroy()
        {
            _restoreHpTimer.OnTimerEnd -= RestoreHitPoints;
        }

        private void OnEnable()
        {
            _currentHitPoints = _maxHitPoints;

            EnableProgressBar(false);
            EnableAmountDisplay(false);
            EnableYellowCorners(false);
            EnableInstructions(false);
        }

        private void Start()
        {
            EnableProgressBar(false);
            EnableAmountDisplay(false);
            EnableYellowCorners(false);
            EnableInstructions(false);
        }

        protected virtual void Update()
        {
            _restoreHpTimer.Tick(Time.deltaTime);
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<CursorControl>(out var cc))
            {
                HideDisplay();
            }
        }

        public override bool OnHit(ToolType incomingToolType, int amount, bool displayHit = true)
        {
            if (!_destructable) return false;

            if (base.OnHit(incomingToolType, amount, displayHit))
            {
                if (displayHit)
                {
                    UpdateAmountDisplay();
                    UpdateFillImage();
                    EnableProgressBar(true);
                    EnableAmountDisplay(false);
                    EnableYellowCorners(false);
                    EnableInstructions(false);
                }

                _restoreHpTimer.RemainingSeconds = 5;

                return true;
            }

            return false;
        }

        private void RestoreHitPoints()
        {
            _currentHitPoints = _maxHitPoints;
        }

        public override void ShowDisplay()
        {
            EnableInstructions(true);
            EnableYellowCorners(true);
        }

        public override void HideDisplay()
        {
            EnableProgressBar(false);
            EnableAmountDisplay(false);
            EnableInstructions(false);
            EnableYellowCorners(false);
        }

        protected override void EnableProgressBar(bool _)
        {
            _progressBar.SetActive(_destructable ? _ : false);
        }

        protected override void EnableAmountDisplay(bool _)
        {
            _amountDisplay.SetActive(_destructable ? _ : false);
        }

        protected override void EnableInstructions(bool _)
        {
            _instructions.SetActive(_destructable ? _ : false);
        }

        protected override void EnableYellowCorners(bool _)
        {
            _yellowCorners.SetActive(_);
        }
    }
}

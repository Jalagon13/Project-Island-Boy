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

        
        protected SpriteRenderer _sr;


        public string ResourceName { get { return null; } }

        protected override void Awake()
        {
            base.Awake();

            _dropPosition = transform.position + new Vector3(0.5f, 0.5f, 0);
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();

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

        public override bool OnHit(ToolType incomingToolType, int amount, bool displayHit = true)
        {
            if (!_destructable) return false;

            _clickFeedback?.PlayFeedbacks();

            if (base.OnHit(incomingToolType, amount, displayHit))
            {
                if (displayHit)
                {
                    UpdateAmountDisplay();
                    UpdateFillImage();
                    EnableProgressBar(true);
                    EnableAmountDisplay(true);
                    EnableYellowCorners(false);
                    EnableInstructions(false);
                }                
                return true;
            }

            return false;
        }

        protected override void OnBreak()
        {
            base.OnBreak();

            StopAllCoroutines();

            Destroy(gameObject);
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

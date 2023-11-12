using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IslandBoy
{
    [SelectionBase]
    public class Resource : Clickable
    {
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private AudioClip _breakSound;
        [SerializeField] private bool _regenerateOnDayStart;
        [SerializeField] private LootTable _lootTable;

        protected int _idleHash = Animator.StringToHash("[Anm] RscIdle");
        protected int _onClickHash = Animator.StringToHash("[Anm] RscHit");
        protected SpriteRenderer _sr;
        protected Vector2 _dropPosition;
        protected Animator _animator;
        protected GameObject _progressBar;
        protected GameObject _amountDisplay;
        protected GameObject _instructions;
        protected GameObject _yellowCorners;

        public string ResourceName { get { return null; } }

        protected override void Awake()
        {
            base.Awake();

            _animator = GetComponent<Animator>();
            _dropPosition = transform.position + new Vector3(0.5f, 0.5f, 0);
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _progressBar = transform.GetChild(2).GetChild(0).gameObject;
            _amountDisplay = transform.GetChild(2).GetChild(1).gameObject;
            _instructions = transform.GetChild(2).GetChild(2).gameObject;
            _yellowCorners = transform.GetChild(2).GetChild(3).gameObject;
        }

        private void Start()
        {
            EnableProgressBar(false);
            EnableAmountDisplay(false);
            EnableYellowCorners(false);
            EnableInstructions(false);
        }

        public void RscIdle()
        {
            if (_animator == null) return;

            AnimStateManager.ChangeAnimationState(_animator, _idleHash);
        }

        public void RscHit()
        {
            if (_animator == null) return;

            _animator.enabled = false;
            _animator.enabled = true;
            AnimStateManager.ChangeAnimationState(_animator, _onClickHash);
        }

        public override void OnClick(ToolType incomingToolType)
        {
            base.OnClick(incomingToolType);

            AudioManager.Instance.PlayClip(_hitSound, false, true, 0.7f);

            UpdateAmountDisplay();
            UpdateFillImage();
            EnableProgressBar(true);
            EnableAmountDisplay(true);
            EnableYellowCorners(false);
            EnableInstructions(true);
            RscHit();
        }

        protected override void OnBreak()
        {
            base.OnBreak();

            AudioManager.Instance.PlayClip(_breakSound, false, true, 0.75f);
            _lootTable.SpawnLoot(_dropPosition);

            StopAllCoroutines();

            // if regenerate resource = true, don't destroy, disable this game object, and enable it with full hp on start of day.
            // if there is a tile in the day do not regenerate it, break it instead, do not drop loot if so.
            Destroy(gameObject);
        }

        public override void ShowDisplay()
        {
            EnableInstructions(true);
            EnableYellowCorners(true);
        }

        protected void UpdateFillImage()
        {
            var fillImage = _progressBar.transform.GetChild(1).GetComponent<Image>();
            fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _maxHitPoints, _currentHitPoints));
        }

        protected void UpdateAmountDisplay()
        {
            var amountText = _amountDisplay.GetComponent<TextMeshProUGUI>();
            amountText.text = _currentHitPoints.ToString();
        }

        public override void HideDisplay()
        {
            EnableProgressBar(false);
            EnableAmountDisplay(false);
            EnableInstructions(false);
            EnableYellowCorners(false);
        }

        protected void EnableProgressBar(bool _)
        {
            _progressBar.SetActive(_);
        }

        protected void EnableAmountDisplay(bool _)
        {
            _amountDisplay.SetActive(_);
        }

        protected void EnableInstructions(bool _)
        {
            _instructions.SetActive(_);
        }

        protected void EnableYellowCorners(bool _)
        {
            _yellowCorners.SetActive(_);
        }
    }
}

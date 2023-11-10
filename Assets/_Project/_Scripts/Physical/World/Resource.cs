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
            EnableDisplay(false);
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

            EnableDisplay(true);
            UpdateAmountDisplay();
            UpdateFillImage();
            DisableInstructions();
            RscHit();
        }

        protected override void OnBreak()
        {
            AudioManager.Instance.PlayClip(_breakSound, false, true, 0.75f);
            _lootTable.SpawnLoot(_dropPosition);

            StopAllCoroutines();
            Destroy(gameObject);
        }

        public override void ShowDisplay()
        {
            EnableDisplay(true);
            UpdateFillImage();
            UpdateAmountDisplay();
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

        protected void DisableInstructions()
        {
            _instructions.SetActive(false);
        }

        public override void HideDisplay()
        {
            EnableDisplay(false);
        }

        protected void EnableDisplay(bool _)
        {
            _progressBar.SetActive(_);
            _amountDisplay.SetActive(_);
            _instructions.SetActive(_);
            _yellowCorners.SetActive(_);
        }
    }
}

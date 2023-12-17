using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Entity : Clickable
    {
        [Header("Base Entity Parameters")]
        [SerializeField] protected PlayerObject _pr;
        [SerializeField] private float _durationUntilDespawn;
        [SerializeField] private bool _dontGiveXp = false;

        protected KnockbackFeedback _knockback;
        private Timer _despawnTimer;

        protected override void Awake()
        {
            base.Awake();

            _knockback = GetComponent<KnockbackFeedback>();
            _despawnTimer = new(_durationUntilDespawn);
            _despawnTimer.OnTimerEnd += Despawn;

            GameSignals.DAY_END.AddListener(DestroyThisEntity);
            GameSignals.PLAYER_DIED.AddListener(DestroyThisEntity);
        }

        private void OnDestroy()
        {
            GameSignals.DAY_END.RemoveListener(DestroyThisEntity);
            GameSignals.PLAYER_DIED.RemoveListener(DestroyThisEntity);
            _despawnTimer.OnTimerEnd -= Despawn;
        }

        protected virtual void Update()
        {
            _despawnTimer.Tick(Time.deltaTime);
        }

        private void Despawn()
        {
            Destroy(gameObject);
        }

        private void DestroyThisEntity(ISignalParameters parameters)
        {
            Destroy(gameObject);
        }

        public override bool OnHit(ToolType incomingToolType, int amount, bool displayHit = true)
        {
            _knockback.PlayFeedback(_pr.Position);
            _despawnTimer.RemainingSeconds = _durationUntilDespawn;

            if (base.OnHit(incomingToolType, amount, displayHit))
            {
                if (displayHit)
                {
                    PopupMessage.Create(transform.position, amount.ToString(), Color.yellow, Vector2.up * 0.5f, 0.4f);
                    UpdateAmountDisplay();
                    UpdateFillImage();
                    EnableYellowCorners(false);
                    EnableAmountDisplay(false);
                    EnableInstructions(false);
                    EnableProgressBar(true);
                }
                return true;
            }

            return false;
        }

        public override void ShowDisplay()
        {
            UpdateAmountDisplay();
            UpdateFillImage();
            EnableInstructions(true);
            EnableYellowCorners(true);
            EnableAmountDisplay(true);
            EnableProgressBar(true);
        }

        public override void HideDisplay()
        {
            EnableProgressBar(true);
            EnableAmountDisplay(false);
            EnableInstructions(false);
            EnableYellowCorners(false);
        }

        protected override void OnBreak()
        {
            _dropPosition = transform.position;

            if (!_dontGiveXp)
            {
                PopupMessage.Create(transform.position, $"+ {_maxHitPoints} XP", Color.white, Vector2.up, 1f);
                PlayerExperience.AddExerpience(_maxHitPoints);
            }

            base.OnBreak();
        }
    }
}

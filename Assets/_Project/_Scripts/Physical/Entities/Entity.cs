using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Entity : Clickable
    {
        [Header("Base Entity Parameters")]
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private bool _dontGiveXp = false;

        private KnockbackFeedback _knockback;

        protected override void Awake()
        {
            base.Awake();

            _knockback = GetComponent<KnockbackFeedback>();

            GameSignals.DAY_END.AddListener(DestroyThisEntity);
        }

        private void OnDestroy()
        {
            GameSignals.DAY_END.RemoveListener(DestroyThisEntity);
        }

        //protected override void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if(collision.TryGetComponent<Clickable>(out var click))
        //    {
        //        if(Random.Range(0, 2) == 0)
        //            click.ClickFeedback?.PlayFeedbacks();
        //    }
        //}

        private void DestroyThisEntity(ISignalParameters parameters)
        {
            Destroy(gameObject);
        }

        public override bool OnHit(ToolType incomingToolType, int amount, bool displayHit = true)
        {
            _knockback.PlayFeedback(_pr.Position);

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

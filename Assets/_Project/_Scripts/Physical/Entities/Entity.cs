using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Entity : Resource
    {
        [SerializeField] private PlayerReference _pr;

        private KnockbackFeedback _knockback;

        protected override void Awake()
        {
            base.Awake();

            _knockback = GetComponent<KnockbackFeedback>();
            _idleHash = Animator.StringToHash("[ANIM] Idle");
            _onClickHash = Animator.StringToHash("[ANIM] Hit");

            GameSignals.DAY_END.AddListener(DestroyThisEntity);
        }

        private void OnDestroy()
        {
            GameSignals.DAY_END.RemoveListener(DestroyThisEntity);
        }

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
            GameSignals.ENTITY_SLAIN.Dispatch();
            base.OnBreak();
        }
    }
}

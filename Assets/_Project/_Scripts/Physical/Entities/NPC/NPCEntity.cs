using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
    public class NPCEntity : Prompt
    {
        [SerializeField] private ItemObject _treevilBark;
        [SerializeField] private UnityEvent _onBarkDetected;

        private bool _questComplete;
        protected KnockbackFeedback _knockback;

        protected override void Awake()
        {
            base.Awake();

            _knockback = GetComponent<KnockbackFeedback>();
        }

        public override void Interact()
        {
            if (QuestComplete())
            {
                return;
            }

            base.Interact();
        }

        public override bool OnHit(ToolType incomingToolType, int amount, bool displayHit = true)
        {
            _knockback.PlayFeedback(_pr.Position);
            _currentHitPoints = _maxHitPoints;

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
                    EnableProgressBar(false);
                }
                return true;
            }

            return false;
        }

        public bool QuestComplete()
        {
            if (_questComplete)
            {
                PopupMessage.Create(transform.position, $"You can now visit my hideout south of this island!", Color.yellow, Vector2.up, 2f);
                return true;
            }

            if(_pr.Inventory.Contains(_treevilBark, 1))
            {
                PopupMessage.Create(transform.position, $"Thank you for the Treevil bark! I have unlocked my hideout south of this island!", Color.yellow, Vector2.up, 5f);
                _questComplete = true;
                _onBarkDetected?.Invoke();
                return true;
            }

            return false;
        }

        public override void ShowDisplay()
        {
            EnableInstructions(true);
            EnableYellowCorners(true);
            EnableAmountDisplay(false);
            EnableProgressBar(false);
        }
    }
}

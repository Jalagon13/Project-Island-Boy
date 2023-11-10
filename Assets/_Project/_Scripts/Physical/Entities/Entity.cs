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
        }

        public override void OnClick(ToolType incomingToolType)
        {
            base.OnClick(incomingToolType);
            
            _knockback.PlayFeedback(_pr.Position);
        }

        protected override void OnBreak()
        {
            _dropPosition = transform.position;
            base.OnBreak();
        }
    }
}

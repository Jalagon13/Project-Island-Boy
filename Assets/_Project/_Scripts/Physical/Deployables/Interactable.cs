using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public abstract class Interactable : Resource
    {
        public Action OnPlayerExitRange;
        [Header("Base Interactable Parameters")]
        [SerializeField] protected PlayerReference _pr;

        private float _interactRange = 3f;
        protected bool _canInteract;
        private Vector3 _origin;
        private SpriteRenderer _rightClickSr;

        protected override void Awake()
        {
            base.Awake();

            _origin = transform.position + new Vector3(0.5f, 0.5f);
            _rightClickSr = transform.GetChild(1).GetComponent<SpriteRenderer>();
        }

        public virtual IEnumerator Start()
        {

            yield return new WaitForSeconds(0.15f);
            _canInteract = true;
        }

        public override bool OnHit(ToolType incomingToolType, int amount)
        {
            if (base.OnHit(incomingToolType, amount))
                return true;

            return false;
        }

        public abstract void Interact();

        public bool PlayerInRange()
        {
            return Vector2.Distance(_origin, _pr.Position) < _interactRange;
        }

        public bool PlayerInRange(Vector2 customPos)
        {
            return Vector2.Distance(customPos, _pr.Position) < _interactRange;
        }
    }
}

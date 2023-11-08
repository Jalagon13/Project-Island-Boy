using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public abstract class Interactable : MonoBehaviour
    {
        public Action OnPlayerExitRange;

        [SerializeField] protected PlayerReference _pr;

        private float _interactRange = 3f;
        protected bool _canInteract;
        private Vector3 _origin;
        private SpriteRenderer _rightClickSr;

        public virtual void Awake()
        {
            _origin = transform.position + new Vector3(0.5f, 0.5f);
            _rightClickSr = transform.GetChild(1).GetComponent<SpriteRenderer>();
        }

        public virtual IEnumerator Start()
        {

            yield return new WaitForSeconds(0.15f);
            _canInteract = true;
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

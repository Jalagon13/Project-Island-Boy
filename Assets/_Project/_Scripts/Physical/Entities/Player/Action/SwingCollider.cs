using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class SwingCollider : MonoBehaviour
    {
        [SerializeField] private ItemParameter _damageParameter;
        [SerializeField] private ItemParameter _durabilityParameter;

        private float _baseDamage;
        private Slot _focusSlot;

        public float BaseDamage { set { _baseDamage = value; } }
        public Slot FocusSlot { set { _focusSlot = value; } }

        private void Awake()
        {
            GameSignals.FOCUS_SLOT_UPDATED.AddListener(InjectFocusSlot);
        }

        private void OnDestroy()
        {
            GameSignals.FOCUS_SLOT_UPDATED.RemoveListener(InjectFocusSlot);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IHealth<int> health))
            {
                health.Damage(Mathf.RoundToInt(CalcDamage()), transform.root.gameObject);
                ModifyDurability();
            }
        }

        private void InjectFocusSlot(ISignalParameters parameters)
        {
            if (parameters.HasParameter("FocusSlot"))
            {
                _focusSlot = (Slot)parameters.GetParameter("FocusSlot");
            }
        }

        private void ModifyDurability()
        {
            if (_focusSlot.CurrentParameters.Count <= 0) return;

            var itemParams = _focusSlot.CurrentParameters;

            if (itemParams.Contains(_durabilityParameter))
            {
                int index = itemParams.IndexOf(_durabilityParameter);
                float newValue = itemParams[index].Value - 1;
                itemParams[index] = new ItemParameter
                {
                    Parameter = _durabilityParameter.Parameter,
                    Value = newValue
                };

                _focusSlot.InventoryItem.UpdateDurabilityCounter();
            }
        }

        private float CalcDamage()
        {
            if (_focusSlot.CurrentParameters.Count <= 0) return _baseDamage;

            var itemParams = _focusSlot.CurrentParameters;

            if (itemParams.Contains(_damageParameter))
            {
                int index = itemParams.IndexOf(_damageParameter);
                return itemParams[index].Value;
            }

            return _baseDamage;
        }
    }
}

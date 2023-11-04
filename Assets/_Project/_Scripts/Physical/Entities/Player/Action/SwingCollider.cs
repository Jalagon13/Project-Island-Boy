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
        private InventorySlot _selectedSlot;

        public float BaseDamage { set { _baseDamage = value; } }
        public InventorySlot SelectedSlot { set { _selectedSlot = value; } }

        private void Awake()
        {
            GameSignals.SELECTED_SLOT_UPDATED.AddListener(InjectSelectedSlot);
        }

        private void OnDisable()
        {
            GameSignals.SELECTED_SLOT_UPDATED.RemoveListener(InjectSelectedSlot);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IHealth<int> health))
            {
                health.Damage(Mathf.RoundToInt(CalcDamage()), transform.root.gameObject);
                ModifyDurability();
            }
        }

        private void InjectSelectedSlot(ISignalParameters parameters)
        {
            _selectedSlot = (InventorySlot)parameters.GetParameter("SelectedSlot");
        }

        private void ModifyDurability()
        {
            

            if (_selectedSlot.CurrentParameters.Count <= 0) return;

            var itemParams = _selectedSlot.CurrentParameters;

            if (itemParams.Contains(_durabilityParameter))
            {
                int index = itemParams.IndexOf(_durabilityParameter);
                float newValue = itemParams[index].Value - 1;
                itemParams[index] = new ItemParameter
                {
                    Parameter = _durabilityParameter.Parameter,
                    Value = newValue
                };

                _selectedSlot.InventoryItem.UpdateDurabilityCounter();
            }
        }

        private float CalcDamage()
        {
            if (_selectedSlot.CurrentParameters.Count <= 0) return _baseDamage;

            var itemParams = _selectedSlot.CurrentParameters;

            if (itemParams.Contains(_damageParameter))
            {
                int index = itemParams.IndexOf(_damageParameter);
                return itemParams[index].Value;
            }

            return _baseDamage;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class SwingCollider : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private ItemParameter _damageParameter;
        [SerializeField] private ItemParameter _durabilityParameter;

        private float _baseDamage;

        public float BaseDamage { set { _baseDamage = value; } }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IHealth<int> health))
            {
                health.Damage(Mathf.RoundToInt(CalcDamage()), transform.root.gameObject);
                ModifyDurability();
            }
        }

        private void ModifyDurability()
        {
            if (_pr.SelectedSlot.CurrentParameters.Count <= 0) return;

            var itemParams = _pr.SelectedSlot.CurrentParameters;

            if (itemParams.Contains(_durabilityParameter))
            {
                int index = itemParams.IndexOf(_durabilityParameter);
                float newValue = itemParams[index].Value - 1;
                itemParams[index] = new ItemParameter
                {
                    Parameter = _durabilityParameter.Parameter,
                    Value = newValue
                };

                _pr.SelectedSlot.InventoryItem.UpdateDurabilityCounter();
            }
        }

        private float CalcDamage()
        {
            if (_pr.SelectedSlot.CurrentParameters.Count <= 0) return _baseDamage;

            var itemParams = _pr.SelectedSlot.CurrentParameters;

            if (itemParams.Contains(_damageParameter))
            {
                int index = itemParams.IndexOf(_damageParameter);
                return itemParams[index].Value;
            }

            return _baseDamage;
        }
    }
}

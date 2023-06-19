using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class SwingCollider : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private ItemParameter _damageParameter;

        private float _baseDamage;

        public float BaseDamage { set { _baseDamage = value; } }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IHealth<int> health))
            {
                health.Damage(Mathf.RoundToInt(CalcDamage()));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] private float _damageAmount;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerEntity playerEntity))
            {
                playerEntity.Damage(Mathf.RoundToInt(_damageAmount), gameObject);
            }
        }
    }
}

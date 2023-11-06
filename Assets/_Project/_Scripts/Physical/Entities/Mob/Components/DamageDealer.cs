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
            if (collision.TryGetComponent(out Player player))
            {
                int damageAmount = Mathf.RoundToInt(_damageAmount);
                Vector2 damagerPosition = transform.root.gameObject.transform.position;

                player.Damage(damageAmount, damagerPosition);
            }
        }
    }
}

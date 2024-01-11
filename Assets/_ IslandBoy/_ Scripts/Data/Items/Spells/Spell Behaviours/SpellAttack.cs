using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class SpellAttack : SpellBehavior
    {
        // This is a basic attack spell
        public override void ApplySpell(Vector2 targetPosition, ConsumableObject playerHealth)
        {
            float damage = CalculateDamage(targetPosition);
            //playerHealth.TakeDamage(damage);
            Debug.Log(damage);
        }

        private float CalculateDamage(Vector2 targetPosition)
        {
            // Calculate the amount of damage a target should take based on it's position.
            Vector2 explosionTarget = targetPosition - (Vector2)transform.position;

            float explosionDistance = explosionTarget.magnitude;

            float relativeDistance = (m_SplashRadius - explosionDistance) / m_SplashRadius;

            float damage = relativeDistance * m_MaxDamage;

            damage = Mathf.Max(0f, damage);

            return damage;
        }
    }
}
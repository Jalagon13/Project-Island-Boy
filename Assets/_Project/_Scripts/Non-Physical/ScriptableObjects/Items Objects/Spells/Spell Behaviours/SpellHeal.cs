using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class SpellHeal : SpellBehavior
    {
        // This is a basic healing spell
        public override void ApplySpell(Vector2 targetPosition, ConsumableObject playerHealth)
        {
            float heal = CalculateHealing(targetPosition);
            //playerHealth.Heal(heal);
            Debug.Log("Apply Heal");
        }

        private float CalculateHealing(Vector2 targetPosition)
        {
            // Calculate the amount of damage a target should take based on it's position.
            Vector2 explosionTarget = targetPosition - (Vector2)transform.position;

            float explosionDistance = explosionTarget.magnitude;

            float relativeDistance = (m_SplashRadius - explosionDistance) / m_SplashRadius;

            float heal = relativeDistance * m_MaxDamage;

            heal = Mathf.Max(0f, heal);

            return heal;
        }
    }
}

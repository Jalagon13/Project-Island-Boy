using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class SpellDeath : SpellBehavior
    {
        public float m_DeathDamage = 999f;
        // This is a spell that will instantly kill anyone within its radius, very potent...
        public override void ApplySpell(Vector2 targetPosition, ConsumableObject playerHealth)
        {
            //playerHealth.TakeDamage(m_DeathDamage);
        }
    }
}
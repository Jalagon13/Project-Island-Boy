using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Magic Spell", menuName = "Magic/Spell")]
    public class CreateSpell : SpellEffect
    {
        private const float _defaultCastForce = 100f;
        private const float _defaultChargeDuration = 0.5f;
        private const float _defaultProjectileDuration = 3f;
        private const int _defaultNumOfProjCasts = 1;

        // Binds parameters on create
        private void Awake()
        {
            if (SpellCastForce == 0)
                SpellCastForce = _defaultCastForce;
            if (ChargeDuration == 0)
                ChargeDuration = _defaultChargeDuration;
            if (NumOfProjCasts == 0)
                NumOfProjCasts = _defaultNumOfProjCasts;
            if (ProjectileDuration == 0)
                ProjectileDuration = _defaultProjectileDuration;
        }
    }
}
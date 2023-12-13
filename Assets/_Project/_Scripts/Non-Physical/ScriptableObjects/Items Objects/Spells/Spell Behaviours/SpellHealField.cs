using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class SpellHealField : SpellProjectile
    {
        private protected override void Awake()
        {
            base.Awake();
        }

        private protected override void Start()
        {
            StartCoroutine(CastMousePosSpell());
            Debug.Log("HEAL FIELD!");
        }
    }
}
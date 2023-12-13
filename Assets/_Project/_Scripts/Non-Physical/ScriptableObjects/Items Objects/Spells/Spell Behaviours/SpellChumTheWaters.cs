using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class SpellChumTheWaters : SpellProjectile
    {

        private protected override void Awake()
        {
            base.Awake();
        }

        private protected override void Start()
        {
            StartCoroutine(CastMousePosSpell());
        }
    }
}
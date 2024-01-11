using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public abstract class SpellEffect : ScriptableObject
    {
        public Sprite SpellDisplaySprite;
        public float SpellCastForce;
        public float ChargeDuration;
        public float ProjectileDuration;
        public int NumOfProjCasts;
        public GameObject SpellProjectile;
        public string SpellName;
        public string SpellDescription;
    }
}
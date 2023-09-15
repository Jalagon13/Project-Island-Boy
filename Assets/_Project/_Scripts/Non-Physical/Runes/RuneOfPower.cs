using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class RuneOfPower : MonoBehaviour, IRune
    {
        [SerializeField] private AudioClip _abilitySound;

        private TileAction _ta;

        public Action<Vector2> Ability { get { return PowerAbility; } }

        public void Initialize(TileAction ta, List<IRune> runeList)
        {
            _ta = ta;
        }

        public void Execute()
        {
            PowerAbility(_ta.transform.position);
        }

        private void PowerAbility(Vector2 pos)
        {
            StartCoroutine(Delay(pos));
        }

        private IEnumerator Delay(Vector2 pos)
        {
            yield return new WaitForSeconds(0.1f);
            _ta.ApplyDamageToBreakable(pos);
            yield return new WaitForSeconds(0.1f);
            _ta.ApplyDamageToBreakable(pos);
            // additional game feel here.
            //AudioManager.Instance.PlayClip(_abilitySound, false, true);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IslandBoy
{
    public class RuneOfDestruction : MonoBehaviour, IRune
    {
        [SerializeField] private AudioClip _abilitySound;

        private TileAction _ta;
        private List<Action<Vector2>> _abilityList = new();

        public void Initialize(TileAction ta, List<IRune> runeList)
        {
            _ta = ta;

            foreach(IRune rune in runeList)
            {
                if (rune is RuneOfPower)
                {
                    RuneOfPower ability = (RuneOfPower)rune;
                    _abilityList.Add(ability.Ability);
                }
            }
        }

        public void Execute()
        {
            StartCoroutine(Delay());

            //AudioManager.Instance.PlayClip(_abilitySound, false, true);

            
        }

        private IEnumerator Delay()
        {
            Vector3 origin = _ta.gameObject.transform.position;

            for (int nx = -1; nx < 2; nx++)
            {
                for (int ny = -1; ny < 2; ny++)
                {
                    if (nx == 0 && ny == 0 ||
                        nx == -1 && ny == 1 ||
                        nx == 1 && ny == 1 ||
                        nx == -1 && ny == -1 ||
                        nx == 1 && ny == -1) continue;

                    yield return new WaitForSeconds(0.1f);

                    var pos = new Vector3(origin.x + nx, origin.y + ny, 0);
                    foreach (Action<Vector2> ability in _abilityList)
                    {
                        ability(pos);
                    }

                    if (_ta.ApplyDamageToBreakable(pos))
                    {
                        _ta.ModifyDurability();
                    }
                }
            }

            _abilityList.Clear();
        }
    }
}

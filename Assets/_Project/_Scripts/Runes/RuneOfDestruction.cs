using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class RuneOfDestruction : MonoBehaviour, IRune
    {
        private TileAction _ta;
        private List<Action<Vector2>> _modifierList = new();

        public void Initialize(TileAction ta, List<IRune> runeList)
        {
            _ta = ta;

            foreach(IRune rune in runeList)
            {
                if (rune is RuneOfGreed)
                {
                    RuneOfGreed modifier = (RuneOfGreed)rune;
                    _modifierList.Add(modifier.Modifier);
                }
            }
        }

        public void Execute()
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

                    var pos = new Vector3(origin.x + nx, origin.y + ny, 0);

                    foreach (Action<Vector2> modifier in _modifierList)
                    {
                        modifier(pos);
                    }

                    if (_ta.ApplyDamageToBreakable(pos))
                        _ta.ModifyDurability();
                }
            }

            _modifierList.Clear();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class RuneOfGreed : MonoBehaviour, IRune
    {
        TileAction _ta;

        public Action<Vector2> Modifier { get { return MarkWithFortune; } }

        public void Initialize(TileAction ta, List<IRune> runeList)
        {
            _ta = ta;
        }

        public void Execute()
        {
            MarkWithFortune(_ta.transform.position);
        }

        private void MarkWithFortune(Vector2 pos)
        {
            var colliders = Physics2D.OverlapCircleAll(pos, 0.2f);

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IBreakable breakable))
                {
                    collider.gameObject.AddComponent<FortuneEffect>();
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class RuneOfDestruction : MonoBehaviour, IRune
    {
        [SerializeField] private TilemapReferences _tmr;

        public void Execute(TileAction ta)
        {
            Vector3 origin = ta.gameObject.transform.position;

            for (int nx = -1; nx < 2; nx++)
            {
                for (int ny = -1; ny < 2; ny++)
                {
                    if (nx == 0 && ny == 0) continue;

                    var pos = new Vector3(origin.x + nx, origin.y + ny, 0);
                    if (ta.ApplyDamageToBreakable(pos))
                        ta.ModifyDurability();
                }
            }
        }
    }
}

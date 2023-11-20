using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(menuName = "New Extended Rule Tile")]
    public class RuleTileExtended : RuleTile
    {
        public ItemObject Item;
        public AudioClip BreakSound;
        public AudioClip PlaceSound;

        public void UpdatePathfinding(Vector2 pos)
        {
            Bounds updateBounds = new(pos, new(2, 2, 1));
            AstarPath.active.UpdateGraphs(updateBounds, 0.1f);
        }
    }
}

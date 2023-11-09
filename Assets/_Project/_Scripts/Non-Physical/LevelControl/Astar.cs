using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Astar : MonoBehaviour
    {
        private static AstarPath _activeAp;

        private void Awake()
        {
            _activeAp = GetComponent<AstarPath>();
        }

        public static void SetUpActiveAstarPath(AstarPath ap)
        {
            _activeAp = ap;
            
            _activeAp.data.gridGraph.Scan();
        }

        public static void UpdateGridGraph(Vector2 position)
        {
            Bounds updateBounds = new(position, new(2, 2, 1));
            //AstarPath.active.UpdateGraphs(updateBounds, 0.1f);
            //Debug.Log($"Active Ap Null? {_activeAp == null}");
            AstarPath.active.UpdateGraphs(updateBounds, 0.1f);
        }

    }
}

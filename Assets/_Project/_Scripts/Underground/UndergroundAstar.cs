using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class UndergroundAstar : MonoBehaviour
    {
        private AstarPath _ap;

        private void Awake()
        {
            _ap = GetComponent<AstarPath>();
            Debug.Log(_ap == null);
        }

        public void RecalculateGrid() // calleed from UG Grid generation event
        {
            Debug.Log("Recalculating Grid callback");

            GridGraph gg = _ap.data.gridGraph;
            gg.SetDimensions(50, 50, 1);
            gg.center = new(20, -10);

            gg.Scan();
        }

        public void UpdateGrid(Vector2 position)
        {
            Bounds updateBounds = new(position, new(2, 2, 1));

            _ap.UpdateGraphs(updateBounds);
        }
    }
}

using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class AstarManager : Singleton<AstarManager>
    {
        private AstarPath _ap;

        protected override void Awake()
        {
            base.Awake();
            _ap = GetComponent<AstarPath>();
        }

        public void RecalculateGrid(int width, int height, float nodeSize, Vector2 center)
        {
            GridGraph gg = _ap.data.gridGraph;
            gg.SetDimensions(width, height, nodeSize);
            gg.center = center;
            gg.Scan();
        }

        public void UpdateGrid(Vector2 position)
        {
            StartCoroutine(NextFrameDelay(position));
        }

        private IEnumerator NextFrameDelay(Vector2 position)
        {
            yield return new WaitForEndOfFrame();

            Bounds updateBounds = new(position, new(2, 2, 1));

            _ap.UpdateGraphs(updateBounds);
        }

    }
}

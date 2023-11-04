using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class AstarManager : Singleton<AstarManager>
    {
        [SerializeField] private PlayerReference _pr;

        private Vector2 _tempPos;
        private AstarPath _ap;
        private float _distUntilGenerate = 7f;

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

            _tempPos = _pr.Position;
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

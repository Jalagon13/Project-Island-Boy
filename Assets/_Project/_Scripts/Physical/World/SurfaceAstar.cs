using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class SurfaceAstar : MonoBehaviour
    {
        private AstarPath _ap;

        private void OnEnable()
        {
            _ap = GetComponent<AstarPath>();
            ExtensionMethods.OnSpawnObject += UpdateGrid;
        }

        private void OnDisable()
        {
            ExtensionMethods.OnSpawnObject -= UpdateGrid;
        }

        public void UpdateGrid(object obj, Object go)
        {
            GameObject gameObject = go as GameObject;
            Bounds updateBounds = new(gameObject.transform.position, new(2, 2, 1));

            _ap.UpdateGraphs(updateBounds);
        }
    }
}

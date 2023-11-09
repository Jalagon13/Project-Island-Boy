using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class OverrideGridGraph : MonoBehaviour
    {
        private AstarPath _ap;

        private void Awake()
        {
            _ap = GetComponent<AstarPath>();
        }

        private void OnEnable()
        {
            Astar.SetUpActiveAstarPath(_ap);
        }
    }
}

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

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            _ap.Scan();
        }

        private void OnEnable()
        {
            _ap.scanOnStartup = true;
        }
    }
}

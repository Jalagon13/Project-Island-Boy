using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class ResourceGenerator : MonoBehaviour
    {
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private List<Resource> _resourceList;

        private void Awake()
        {
            GameSignals.DAY_START.AddListener(RegenerateResources);
        }

        private void OnDestroy()
        {
            GameSignals.DAY_START.RemoveListener(RegenerateResources);
        }

        private void RegenerateResources(ISignalParameters parameters)
        {
            // take trees and stones and randomly generate them at the start of the day

        }
    }
}

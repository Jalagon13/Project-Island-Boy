using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace IslandBoy
{
    public class HostileAI : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;

        private IAstarAI _ai;

        private void Awake()
        {
            _ai = GetComponent<IAstarAI>();
        }

        private void Update()
        {
            _ai.destination = _pr.Position;
        }
    }
}

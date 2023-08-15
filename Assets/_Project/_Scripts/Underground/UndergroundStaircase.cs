using System;
using UnityEngine;

namespace IslandBoy
{
    public class UndergroundStaircase : MonoBehaviour
    {
        private Action _goDownAction;

        public Action GoDownAction { set { _goDownAction = value; } }

        public void GoDownTheStaircase()
        {
            _goDownAction?.Invoke();
        }
    }
}

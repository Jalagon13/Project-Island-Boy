using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CaveLevel : MonoBehaviour
    {
        private int _index;

        public int Index { get { return _index; } }

        public void Initialize(int index)
        {
            _index = index;
        }
    }
}

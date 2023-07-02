using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Bed : MonoBehaviour
    {
        public void EndDay()
        {
            TimeManager.Instance.EndDay();
        }
    }
}

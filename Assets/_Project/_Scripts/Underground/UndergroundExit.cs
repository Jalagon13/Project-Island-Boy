using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class UndergroundExit : MonoBehaviour
    {
        public void ExitUnderground()
        {
            LevelManager.Instance.LoadSurface();
        }
    }
}

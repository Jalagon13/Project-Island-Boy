using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class MinerNPC : MonoBehaviour
    {
        public void EnterCave()
        {
            LevelManager.Instance.LoadUnderground();
        }
    }
}

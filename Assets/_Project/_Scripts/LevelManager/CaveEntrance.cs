using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class CaveEntrance : MonoBehaviour
    {
        public void EnterCave()
        {
            CursorManager.Instance.SetDefaultCursor();
            LevelManager.Instance.TransitionToCaveLevel();
        }
    }
}

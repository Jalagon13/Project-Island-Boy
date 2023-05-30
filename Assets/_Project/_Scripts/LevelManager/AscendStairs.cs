using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class AscendStairs : MonoBehaviour
    {
        public void ExitCavern()
        {
            CursorManager.Instance.SetDefaultCursor();
            LevelManager.Instance.TransitionToSurfaceLevel();
            LevelManager.Instance.SetPlayerToSurfaceBackpoint();
        }
    }
}

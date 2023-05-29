using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class AscendStairs : MonoBehaviour
    {
        public void ExitCavern()
        {
            LevelManager.Instance.TransitionToSurfaceLevel();
            LevelManager.Instance.SetPlayerToSurfaceBackpoint();
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class SurfaceManager : Singleton<SurfaceManager>
    {
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            Debug.Log(scene.buildIndex);
        }
    }
}

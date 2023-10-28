using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class Startup : MonoBehaviour
    {
        private void Awake()
        {
            LoadSceneAdditively("MainMenu");
        }

        private static void LoadSceneAdditively(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }
}

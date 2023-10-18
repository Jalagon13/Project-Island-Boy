using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace IslandBoy
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private string GameSceneName;
        public void StartGame()
        {
            SceneManager.LoadScene(GameSceneName);
        }

        public void LoadScene(string scene)
        {
            //GameStateManager.EndPause.Invoke();
            SceneManager.LoadScene(scene);
        }

        public void QuitGame()
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    
        #else
            Application.Quit();
        #endif
        }
    }
}

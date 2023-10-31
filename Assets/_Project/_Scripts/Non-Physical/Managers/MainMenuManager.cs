using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace IslandBoy
{
    public class MainMenuManager : MonoBehaviour
    {
        public void LoadGameWorld()
        {
            StartCoroutine(LoadScenes());
            
        }

        private IEnumerator LoadScenes()
        {
            yield return StartCoroutine(LoadSceneAdd("GameWorld"));
            yield return StartCoroutine(LoadSceneAdd("Inventory"));
            yield return StartCoroutine(LoadSceneAdd("PauseMenu"));
            yield return StartCoroutine(LoadSceneAdd("PlayerStats"));
            yield return StartCoroutine(LoadSceneAdd("PromptDisplay"));
            yield return StartCoroutine(LoadSceneAdd("SunMeter"));
            yield return StartCoroutine(LoadSceneAdd("DeathPanel"));
            yield return StartCoroutine(LoadSceneAdd("CurrencyDisplay"));

            Scene gameWorld = SceneManager.GetSceneByName("GameWorld");
            SceneManager.SetActiveScene(gameWorld);
            SceneManager.UnloadSceneAsync("MainMenu");

        }

        private IEnumerator LoadSceneAdd(string sceneName)
        {
            AsyncOperation sceneAsync = new();
            sceneAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            sceneAsync.allowSceneActivation = false;

            while (sceneAsync.progress < 0.9f)
            {
                yield return null;
            }

            sceneAsync.allowSceneActivation = true;

            while (!sceneAsync.isDone)
            {
                yield return null;
            }
        }

        private IEnumerator LoadScene(string sceneName)
        {
            AsyncOperation sceneAsync = new();
            sceneAsync = SceneManager.LoadSceneAsync(sceneName);
            sceneAsync.allowSceneActivation = false;

            while (sceneAsync.progress < 0.9f)
            {
                yield return null;
            }

            sceneAsync.allowSceneActivation = true;

            while (!sceneAsync.isDone)
            {
                yield return null;
            }
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
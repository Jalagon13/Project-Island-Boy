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
			// StartCoroutine(LoadScenes());
			LoadSceneAdditively("MainMenu");
			// LoadSceneAdditively("Camera");
		}
		
		private IEnumerator LoadScenes()
        {
            yield return StartCoroutine(LoadSceneAdd("Surface"));
            yield return StartCoroutine(LoadSceneAdd("DeathPanel"));
            yield return StartCoroutine(LoadSceneAdd("Inventory"));
            yield return StartCoroutine(LoadSceneAdd("LaunchControl"));
            yield return StartCoroutine(LoadSceneAdd("LevelControl"));
            yield return StartCoroutine(LoadSceneAdd("PauseMenu"));
            yield return StartCoroutine(LoadSceneAdd("Player"));
            yield return StartCoroutine(LoadSceneAdd("PromptDisplay"));
            yield return StartCoroutine(LoadSceneAdd("StatsDisplay"));
            yield return StartCoroutine(LoadSceneAdd("TimeManager"));

            Scene gameWorld = SceneManager.GetSceneByName("Surface");
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

		private static void LoadSceneAdditively(string sceneName)
		{
			SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
		}


	}
}

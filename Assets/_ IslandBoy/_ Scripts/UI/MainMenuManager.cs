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
			yield return StartCoroutine(LoadSceneAdd("Player"));  
			yield return StartCoroutine(LoadSceneAdd("IngameUI"));

			yield return StartCoroutine(LoadSceneAdd("Surface")); 
			Scene surfaceScene = SceneManager.GetSceneByName("Surface"); 
			yield return new WaitForSeconds(0.2f); 
			EnableAllObjectsInScene(surfaceScene, false); 
			 
			yield return StartCoroutine(LoadSceneAdd("StartCave")); 
			Scene gameWorld = SceneManager.GetSceneByName("StartCave"); 
			SceneManager.SetActiveScene(gameWorld); 
			
			GameSignals.SCENE_FINISH_SETUP.Dispatch();
			SceneManager.UnloadSceneAsync("MainMenu");
		}
		
		private void EnableAllObjectsInScene(Scene scene, bool _)
		{
			GameObject[] allObjects = scene.GetRootGameObjects();
			foreach (var go in allObjects)
			{
				go.SetActive(_);
			}
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

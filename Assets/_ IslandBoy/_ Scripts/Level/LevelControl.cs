using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

namespace IslandBoy
{
	public class LevelControl : MonoBehaviour
	{
		public static int CaveLevelToLoad = 0;
		
		private static GameObject DeployParent;
		
		public static void SetDP(GameObject dp)
		{
			DeployParent = dp;
		}
		
		public static GameObject GetDP()
		{
			return DeployParent;
		}
		
		private void Awake()
		{
			GameSignals.CHANGE_SCENE.AddListener(ChangeScene);
		}
		
		private void OnDestroy()
		{
			GameSignals.CHANGE_SCENE.RemoveListener(ChangeScene);
		}
		
		private void ChangeScene(ISignalParameters parameters)
		{
			if(parameters.HasParameter("NextScene"))
			{
				// Get active scene and disable all the gameobjects in it
				Scene activeScene = SceneManager.GetActiveScene();
				EnableAllObjectsInScene(activeScene, false);
				
				// Load next scene additively
				string name = (string)parameters.GetParameter("NextScene");
				Scene nextScene = SceneManager.GetSceneByName(name);
				if(nextScene.isLoaded)
				{
					SceneManager.SetActiveScene(nextScene);
					EnableAllObjectsInScene(nextScene, true);
				}
				else
				{
					StartCoroutine(TransitionScene(name));
				}
			}
		}
		
		private void EnableAllObjectsInScene(Scene scene, bool _)
		{
			GameObject[] allObjects = scene.GetRootGameObjects();
			foreach (var go in allObjects)
			{
				go.SetActive(_);
			}
		}
		
		private IEnumerator TransitionScene(string nextScene)
		{
			yield return StartCoroutine(LoadSceneAdd(nextScene));
			Scene scene = SceneManager.GetSceneByName(nextScene);
			SceneManager.SetActiveScene(scene);
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
	}
}

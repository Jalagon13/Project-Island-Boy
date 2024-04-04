using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

namespace IslandBoy
{
	public class LevelControl : MonoBehaviour
	{
		public static int CaveLevelToLoad = 0;

		private static GameObject DeployParent;
		private static string SurfaceScene = "Surface";
		private static bool PlayerDied;

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
			GameSignals.PLAYER_DIED.AddListener(PlayerDiedToggle);
			GameSignals.PLAYER_RESPAWN.AddListener(PlayerDiedToggleRespawn);
			GameSignals.DAY_END.AddListener(RespawnProcedure);
		}

		private void OnDestroy()
		{
			GameSignals.CHANGE_SCENE.RemoveListener(ChangeScene);
			GameSignals.PLAYER_DIED.RemoveListener(PlayerDiedToggle);
			GameSignals.PLAYER_RESPAWN.RemoveListener(PlayerDiedToggleRespawn);
			GameSignals.DAY_END.RemoveListener(RespawnProcedure);
		}

		private void ChangeScene(ISignalParameters parameters)
		{
			if (parameters.HasParameter("NextScene"))
			{
				// Get active scene and disable all the gameobjects in it
				Scene activeScene = SceneManager.GetActiveScene();
				EnableAllObjectsInScene(activeScene, false);

				// Load next scene additively
				string name = (string)parameters.GetParameter("NextScene");
				Scene nextScene = SceneManager.GetSceneByName(name);
				if (nextScene.isLoaded)
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

		private void PlayerDiedToggle(ISignalParameters parameters)
		{
			PlayerDied = true;
		}
		
		private void PlayerDiedToggleRespawn(ISignalParameters parameters)
		{
			PlayerDied = false;
		}


		private void RespawnProcedure(ISignalParameters parameters)
		{
			if (!PlayerDied) return;
			
			PlayerDied = false;

			Signal signal = GameSignals.CHANGE_SCENE;
			signal.ClearParameters();
			signal.AddParameter("NextScene", SurfaceScene);
			signal.Dispatch();
		}
	}
}

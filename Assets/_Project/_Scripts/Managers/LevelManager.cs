using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class LevelManager : Singleton<LevelManager>
    {
        private AsyncOperation _sceneAsync;
        private Scene _currentScene;
        private GameObject _playerObject;
        private Camera _camera;
        List<GameObject> _rootObjects;

        protected override void Awake()
        {
            base.Awake();
            _playerObject = GameObject.Find("Player");
            _camera = Camera.main;
            _rootObjects = new();
        }

        public void LoadUnderground()
        {
            _currentScene = SceneManager.GetActiveScene();
            StartCoroutine(Load(1));
        }

        private IEnumerator Load(int sceneIndex)
        {
            AsyncOperation scene = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            scene.allowSceneActivation = false;
            _sceneAsync = scene;

            while (scene.progress < 0.9f)
            {
                yield return null;
            }

            StartCoroutine(EnableScene(sceneIndex));
        }

        private IEnumerator EnableScene(int sceneIndex)
        {
            _sceneAsync.allowSceneActivation = true;

            while (!_sceneAsync.isDone)
            {
                yield return null;
            }

            Scene sceneToLoad = SceneManager.GetSceneByBuildIndex(sceneIndex);

            if (sceneToLoad.IsValid())
            {
                SceneManager.MoveGameObjectToScene(_playerObject, sceneToLoad);
                SceneManager.MoveGameObjectToScene(_camera.gameObject, sceneToLoad);
                SceneManager.SetActiveScene(sceneToLoad);

                DisableSurfaceGameObjects();
            }
        }

        private void DisableSurfaceGameObjects()
        {
            _currentScene.GetRootGameObjects(_rootObjects);

            foreach (var go in _rootObjects)
            {
                go.SetActive(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private PlayerReference _pr;

        private Scene _surfaceScene;
        private AsyncOperation _sceneAsync;
        private GameObject _playerObject;
        private GameObject _tileActionObject;
        private GameObject _ugStructuresObject;
        private Camera _camera;
        private List<GameObject> _rootObjects;
        private Vector2 _surfaceReturnPosition;

        public GameObject UndergroundStructureHolder { get { return _ugStructuresObject; } }

        protected override void Awake()
        {
            base.Awake();
            _surfaceScene = SceneManager.GetSceneByBuildIndex(0);
            _playerObject = GameObject.Find("Player");
            _tileActionObject = GameObject.Find("TileAction");
            _camera = Camera.main;
            _rootObjects = new();
        }

        public void LoadUnderground()
        {
            if(SceneManager.GetActiveScene().buildIndex != 0)
            {
                Debug.Log("Can not load underground because current scene is not surface");
                return;
            }

            

            _surfaceReturnPosition = _pr.Position;
            StartCoroutine(Load(1));
        }

        public void LoadSurface()
        {
            if(SceneManager.GetActiveScene().buildIndex != 1)
            {
                Debug.Log("Can not load surface because current scene is not underground");
                return;
            }

            SceneManager.MoveGameObjectToScene(_playerObject, _surfaceScene);
            SceneManager.MoveGameObjectToScene(_tileActionObject, _surfaceScene);
            SceneManager.MoveGameObjectToScene(_camera.gameObject, _surfaceScene);
            SceneManager.SetActiveScene(_surfaceScene);
            SceneManager.UnloadSceneAsync(1);

            EnableSurfaceObjects(true);
            _playerObject.transform.SetPositionAndRotation(_surfaceReturnPosition, Quaternion.identity);
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
                SceneManager.MoveGameObjectToScene(_tileActionObject, sceneToLoad);
                SceneManager.MoveGameObjectToScene(_camera.gameObject, sceneToLoad);
                SceneManager.SetActiveScene(sceneToLoad);
                
                if (sceneIndex == 1)
                {
                    EnableSurfaceObjects(false);
                }

                _ugStructuresObject = GameObject.Find("[WS] Structures");
            }
        }

        private void EnableSurfaceObjects(bool foo)
        {
            _surfaceScene.GetRootGameObjects(_rootObjects);

            foreach (var go in _rootObjects)
            {
                go.SetActive(foo);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private PlayerReference _pr;

        private Vector2 _surfaceReturnPosition;
        private AsyncOperation _sceneAsync;
        private GameObject _playerObject;
        private GameObject _tileActionObject;
        private List<GameObject> _rootObjects;
        private Light2D _globalLight;
        private Camera _camera;
        private Canvas _canvas;

        protected override void Awake()
        {
            base.Awake();
            _playerObject = GameObject.Find("Player");
            _tileActionObject = GameObject.Find("TileAction");
            _globalLight = transform.GetChild(1).GetComponent<Light2D>();

            _canvas = transform.GetChild(0).GetComponent<Canvas>();
            _canvas.gameObject.SetActive(false);
            _camera = Camera.main;
            _rootObjects = new();
        }

        public void LoadUnderground()
        {
            if(SceneManager.GetActiveScene().buildIndex != 0)
            {
                Debug.LogError("Can not load underground because current scene is not surface");
                return;
            }

            DayNightManager.Instance.GlobalVolume.enabled = false;

            _canvas.gameObject.SetActive(true);
            _globalLight.intensity = 0;
            _surfaceReturnPosition = _pr.Position;

            StartCoroutine(GenerateScene());
        }

        private IEnumerator GenerateScene()
        {
            yield return StartCoroutine(GenerateUndergroundScene());

            Scene surface = SceneManager.GetSceneByBuildIndex(0);
            Scene underground = SceneManager.GetSceneByBuildIndex(1);

            if (underground.IsValid())
            {
                SceneManager.MoveGameObjectToScene(_playerObject, underground);
                SceneManager.MoveGameObjectToScene(_tileActionObject, underground);
                SceneManager.MoveGameObjectToScene(_camera.gameObject, underground);
                SceneManager.SetActiveScene(underground);

                EnableSceneObjects(surface, false);
                EnableSceneObjects(underground, true);
            }
        }

        private IEnumerator GenerateUndergroundScene()
        {
            if (_sceneAsync != null) yield break;

            _sceneAsync = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            _sceneAsync.allowSceneActivation = false;

            while (_sceneAsync.progress < 0.9f)
            {
                yield return null;
            }

            _sceneAsync.allowSceneActivation = true;

            while (!_sceneAsync.isDone)
            {
                yield return null;
            }
        }

        public void LoadSurface()
        {
            if(SceneManager.GetActiveScene().buildIndex != 1)
            {
                Debug.LogError("Can not load surface because current scene is not underground");
                return;
            }

            _canvas.gameObject.SetActive(false);
            _globalLight.intensity = 1;

            Scene surface = SceneManager.GetSceneByBuildIndex(0);
            Scene underground = SceneManager.GetSceneByBuildIndex(1);

            DayNightManager.Instance.GlobalVolume.enabled = true;
            SceneManager.MoveGameObjectToScene(_playerObject, surface);
            SceneManager.MoveGameObjectToScene(_tileActionObject, surface);
            SceneManager.MoveGameObjectToScene(_camera.gameObject, surface);
            SceneManager.SetActiveScene(surface);

            EnableSceneObjects(underground, false);
            EnableSceneObjects(surface, true);

            _playerObject.transform.SetPositionAndRotation(_surfaceReturnPosition, Quaternion.identity);
        }

        

        private void EnableSceneObjects(Scene scene, bool foo)
        {
            scene.GetRootGameObjects(_rootObjects);

            foreach (var go in _rootObjects)
            {
                go.SetActive(foo);
            }
        }
    }
}

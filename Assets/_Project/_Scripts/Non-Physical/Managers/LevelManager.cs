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
        [SerializeField] private AudioClip _beachAmbSound;
        [SerializeField] private AudioClip _caveAmbSound;

        private Vector2 _surfaceReturnPosition;
        private AsyncOperation _sceneAsync;
        private GameObject _playerObject;
        private GameObject _tileActionObject;
        private List<GameObject> _rootObjects;
        private Light2D _globalLight;
        private Camera _camera;

        protected override void Awake()
        {
            base.Awake();
            _playerObject = GameObject.Find("Player");
            _tileActionObject = GameObject.Find("TileAction");
            _globalLight = transform.GetChild(0).GetComponent<Light2D>();

            _camera = Camera.main;
            _rootObjects = new();
        }

        private void Start()
        {
            //AudioManager.Instance.PlayClip(_beachAmbSound, true, false, 0.1f);
            //AudioManager.Instance.StopClip(_caveAmbSound);
        }

        public void LoadUnderground()
        {
            if(SceneManager.GetActiveScene().buildIndex != 0)
            {
                Debug.LogError("Can not load underground because current scene is not surface");
                return;
            }

            DayManager.Instance.GlobalVolume.enabled = false;
            AudioManager.Instance.StopClip(_beachAmbSound);
            AudioManager.Instance.PlayClip(_caveAmbSound, true, false);

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

            _globalLight.intensity = 1;

            Scene surface = SceneManager.GetSceneByBuildIndex(0);
            Scene underground = SceneManager.GetSceneByBuildIndex(1);

            DayManager.Instance.GlobalVolume.enabled = true;
            AudioManager.Instance.PlayClip(_beachAmbSound, true, false, 0.1f);
            AudioManager.Instance.StopClip(_caveAmbSound);
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

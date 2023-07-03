using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace IslandBoy
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private AudioClip _surfaceAmbSound;
        [SerializeField] private AudioClip _cavernAmbSound;
        [SerializeField] private Transform _surfaceBackPoint;

        private SurfaceLevel _surfaceLevel;
        private CavernLevel _cavernLevel;
        private GameObject _player;
        private Light2D _globalLight;
        private Canvas _caveCanvas;
        private IAppendToLevel _currentLevel;

        protected override void Awake()
        {
            base.Awake();
            _surfaceLevel = transform.GetChild(0).GetComponent<SurfaceLevel>();
            _cavernLevel = transform.GetChild(1).GetComponent<CavernLevel>();
            _globalLight = transform.GetChild(2).GetComponent<Light2D>();
            _caveCanvas =  transform.GetChild(3).GetComponent<Canvas>();
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnEnable()
        {
            AppendToLevel.AppendToLevelEvent += AppendToCurrentLevel;
        }

        private void OnDisable()
        {
            AppendToLevel.AppendToLevelEvent -= AppendToCurrentLevel;
        }

        private void Start()
        {
            TransitionToSurfaceLevel();
        }

        private void AppendToCurrentLevel(GameObject obj)
        {
            _currentLevel.Append(obj);
        }

        public void TransitionToSurfaceLevel()
        {
            _surfaceLevel.gameObject.SetActive(true);
            _cavernLevel.gameObject.SetActive(false);
            _currentLevel = _surfaceLevel.GetComponent<IAppendToLevel>();
            _globalLight.intensity = 1;
            _caveCanvas.enabled = false;
            AudioManager.Instance.StopClip(_cavernAmbSound);
            AudioManager.Instance.PlayClip(_surfaceAmbSound, true, false, 0.15f);
        }

        public void SetPlayerToSurfaceBackpoint()
        {
            _player.transform.position = _surfaceBackPoint.position;
        }

        public void TransitionToCaveLevel()
        {
            _surfaceLevel.gameObject.SetActive(false);
            _cavernLevel.gameObject.SetActive(true);
            _currentLevel = _cavernLevel.GetComponent<IAppendToLevel>();
            _globalLight.intensity = 0f;
            _caveCanvas.enabled = true;
            AudioManager.Instance.StopClip(_surfaceAmbSound);
            AudioManager.Instance.PlayClip(_cavernAmbSound, true, false, 0.75f);
            _cavernLevel.StartCavernRun();
            StartCoroutine(TransitionScreenTimer());
        }

        public void DescendToNextLevel()
        {
            _cavernLevel.TransitionToNextLevel();
            StartCoroutine(TransitionScreenTimer());
        }

        private IEnumerator TransitionScreenTimer()
        {
            var transitionPanel = _caveCanvas.transform.GetChild(3).GetComponent<RectTransform>();
            transitionPanel.gameObject.SetActive(true);
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(5f);
            Time.timeScale = 1;
            transitionPanel.gameObject.SetActive(false);
        }
    }
}

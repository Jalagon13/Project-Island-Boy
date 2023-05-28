using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace IslandBoy
{
    public class LevelManager : Singleton<LevelManager>
    {
        private SurfaceLevel _surfaceLevel;
        private CavernLevel _cavernLevel;
        private GameObject _player;
        private Light2D _globalLight; 
        private Vector2 _surfaceBackPoint;
        private IAppendToLevel _currentLevel;

        public Vector2 SurfaceBackPoint { set { _surfaceBackPoint = value; } }

        protected override void Awake()
        {
            base.Awake();
            _surfaceLevel = transform.GetChild(0).GetComponent<SurfaceLevel>();
            _cavernLevel = transform.GetChild(1).GetComponent<CavernLevel>();
            _globalLight = transform.GetChild(2).GetComponent<Light2D>();
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
            TransitionToCaveLevel();
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
            _player.transform.position = _surfaceBackPoint;
            _globalLight.intensity = 1;
        }

        public void TransitionToCaveLevel()
        {
            _surfaceLevel.gameObject.SetActive(false);
            _cavernLevel.gameObject.SetActive(true);
            _currentLevel = _cavernLevel.GetComponent<IAppendToLevel>();
            _globalLight.intensity = 1;
            _cavernLevel.StartCavernRun();
        }

        public void DescendToNextLevel()
        {
            _cavernLevel.TransitionToNextLevel();
        }
    }
}

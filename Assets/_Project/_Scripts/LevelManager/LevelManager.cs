using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class LevelManager : Singleton<LevelManager>
    {
        private SurfaceLevel _surfaceLevel;
        private CaveLevels _caveLevels;
        private GameObject _player;
        private IAppendToLevel _currentLevel;
        private Vector2 _surfaceBackPoint;

        public Vector2 SurfaceBackPoint { set { _surfaceBackPoint = value; } }

        protected override void Awake()
        {
            base.Awake();
            _surfaceLevel = transform.GetChild(0).GetComponent<SurfaceLevel>();
            _caveLevels = transform.GetChild(1).GetComponent<CaveLevels>();
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
            _caveLevels.gameObject.SetActive(false);
            _currentLevel = _surfaceLevel.GetComponent<IAppendToLevel>();
            _player.transform.position = _surfaceBackPoint;
        }

        public void TransitionToCaveLevel()
        {
            _surfaceLevel.gameObject.SetActive(false);
            _caveLevels.gameObject.SetActive(true);
            _currentLevel = _caveLevels.GetComponent<IAppendToLevel>();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class LevelManager : Singleton<LevelManager>
    {
        private SurfaceLevel _surfaceLevel;
        private CaveLevels _caveLevels;
        private IAppendToLevel _currentLevel;

        protected override void Awake()
        {
            base.Awake();
            _surfaceLevel = transform.GetChild(0).GetComponent<SurfaceLevel>();
            _caveLevels = transform.GetChild(1).GetComponent<CaveLevels>();
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
        }

        public void TransitionToCaveLevel()
        {
            _surfaceLevel.gameObject.SetActive(false);
            _caveLevels.gameObject.SetActive(true);
            _currentLevel = _caveLevels.GetComponent<IAppendToLevel>();
        }
    }
}

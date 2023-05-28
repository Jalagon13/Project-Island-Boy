using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace IslandBoy
{
    public class LevelManager : Singleton<LevelManager>
    {
        private SurfaceLevel _surface;
        private GameObject _player;
        private Light2D _globalLight; 
        private Vector2 _surfaceBackPoint;
        private IAppendToLevel _currentLevel;

        public Vector2 SurfaceBackPoint { set { _surfaceBackPoint = value; } }

        protected override void Awake()
        {
            base.Awake();
            _surface = transform.GetChild(0).GetComponent<SurfaceLevel>();
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
            TransitionToSurfaceLevel();
        }

        private void AppendToCurrentLevel(GameObject obj)
        {
            _currentLevel.Append(obj);
        }

        public void TransitionToSurfaceLevel()
        {
            _surface.gameObject.SetActive(true);
            _currentLevel = _surface.GetComponent<IAppendToLevel>();
            _player.transform.position = _surfaceBackPoint;
            _globalLight.intensity = 1;
        }

        public void TransitionToCaveLevel()
        {
            _surface.gameObject.SetActive(false);
            _globalLight.intensity = 0;
        }
    }
}

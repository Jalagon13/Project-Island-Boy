using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
    public class TreevilSpriteControl : MonoBehaviour
    {
        [SerializeField] private AudioClip _landingSound;
        [Header("Animation Events")]
        [SerializeField] private UnityEvent _onBeginningOfSpawn;
        [SerializeField] private UnityEvent _onLand;
        [SerializeField] private UnityEvent _onEndOfSpawn;

        public void OnLandGameFeel()
        {
            AudioManager.Instance.PlayClip(_landingSound, false, true);
        }

        public void OnLand()
        {
            _onLand?.Invoke();
        }

        public void BeginningOfSpawn()
        {
            _onBeginningOfSpawn?.Invoke();
        }

        public void EndOfSpawn()
        {
            _onEndOfSpawn?.Invoke();
        }
    }
}

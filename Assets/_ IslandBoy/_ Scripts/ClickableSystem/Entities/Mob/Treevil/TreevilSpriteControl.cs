using MoreMountains.Tools;
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
            MMSoundManagerSoundPlayEvent.Trigger(_landingSound, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position);
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

using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class OverrideGridGraph : MonoBehaviour
    {
        [SerializeField] private AudioClip _ambientSound;
        [SerializeField] private AudioClip _bgMusic;

        private AstarPath _ap;

        private void Awake()
        {
            _ap = GetComponent<AstarPath>();
        }

        private IEnumerator Start()
        {
            MMSoundManagerSoundPlayEvent.Trigger(_ambientSound, MMSoundManager.MMSoundManagerTracks.Music, transform.position, loop:true, volume:0.2f);
            MMSoundManagerSoundPlayEvent.Trigger(_bgMusic, MMSoundManager.MMSoundManagerTracks.Music, transform.position, loop:true, volume: 0.15f);

            yield return new WaitForSeconds(1f);

            _ap.Scan();
        }

        private void OnEnable()
        {
            _ap.scanOnStartup = true;
        }
    }
}

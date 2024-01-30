using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class BootstrapSoundSetup : MonoBehaviour
	{
		[SerializeField] private bool _playSound = true;
		[SerializeField] private AudioClip _ambientSound;
		[SerializeField] private AudioClip _bgMusic;

		private void Start()
		{
			if(!_playSound) return;
			
			MMSoundManagerSoundPlayEvent.Trigger(_ambientSound, MMSoundManager.MMSoundManagerTracks.Music, transform.position, loop: true, volume: 0.25f, persistent:true);
			MMSoundManagerSoundPlayEvent.Trigger(_bgMusic, MMSoundManager.MMSoundManagerTracks.Music, transform.position, loop: true, volume: 0.5f, persistent: true);
		}
	}
}

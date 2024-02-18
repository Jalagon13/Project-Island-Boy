using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace IslandBoy
{
	public class Environment : MonoBehaviour
	{
		[SerializeField] private int _width;
		[SerializeField] private int _height;
		[SerializeField] private Vector3 _center;
		[Header("BGM/A")]
		[SerializeField] private bool _playAmb = true;
		[SerializeField] private float _ambVolume = 1f;
		[SerializeField] private AudioClip _ambientSound;
		[SerializeField] private bool _playMusic = true;
		[SerializeField] private float _bgmVolume = 1f;
		[SerializeField] private AudioClip _bgMusic;
		[SerializeField] private float _delay = 0.5f;
		
		
		private void OnEnable()
		{
			StartCoroutine(Delay());
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForEndOfFrame();
			AStarExtensions.Instance.ReScanNodeGraph(_width, _height, _center);
			
			yield return new WaitForSeconds(_delay);
			
			if(_playAmb && _ambientSound != null)
				MMSoundManagerSoundPlayEvent.Trigger(_ambientSound, MMSoundManager.MMSoundManagerTracks.Music, transform.position, loop: true, volume: _ambVolume, persistent:false, priority:0);
			
			if(_playMusic && _bgMusic != null)
				MMSoundManagerSoundPlayEvent.Trigger(_bgMusic, MMSoundManager.MMSoundManagerTracks.Music, transform.position, loop: true, volume: _bgmVolume, persistent: false, priority:0);
			
			
		}
	}
}

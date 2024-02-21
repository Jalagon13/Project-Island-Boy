using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	
	[RequireComponent(typeof(Animator))]
	public class PlayerPartAnimator : MonoBehaviour
	{
		[SerializeField] private AnimationClip _idleClip;
		[SerializeField] private AnimationClip _moveClip;
		
		private Animator _animator;
		
		private int _idleHash, _moveHash;
		
		private void Awake() 
		{
			_animator = GetComponent<Animator>();
			_idleHash = Animator.StringToHash(_idleClip.name);
			_moveHash = Animator.StringToHash(_moveClip.name);
		}
		
		public void PlayIdle()
		{
			AnimStateManager.ChangeAnimationState(_animator, _idleHash);
		}
		
		public void PlayMove()
		{
			AnimStateManager.ChangeAnimationState(_animator, _moveHash);
		}
	}
}

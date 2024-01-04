using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class NPCStateManager : MonoBehaviour
	{
		[SerializeField] private PlayerObject _pr;

		private Action _onMove;
		private Vector2 _homePoint;
		private readonly int _hashIdle = Animator.StringToHash("[Anm] NPCIdle");
		private readonly int _hashMove = Animator.StringToHash("[Anm] NPCMove");

		public Action OnMove { get { return _onMove; } set { _onMove = value; } }
		public Vector2 HomePoint { get { return _homePoint; } }
		
		private void Awake()
		{
			_homePoint = transform.position;
		}

		private void Update()
		{
			_onMove?.Invoke();
		}

		public void ChangeToIdleState(Animator animator)
		{
			AnimStateManager.ChangeAnimationState(animator, _hashIdle);
		}

		public void ChangeToMoveState(Animator animator)
		{
			AnimStateManager.ChangeAnimationState(animator, _hashMove);
		}

		public void Seek(Vector2 pos)
		{
			
		}
	}
}

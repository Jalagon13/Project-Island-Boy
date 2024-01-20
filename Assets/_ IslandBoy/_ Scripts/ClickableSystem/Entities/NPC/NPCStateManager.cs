using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace IslandBoy
{
	public class NPCStateManager : MonoBehaviour
	{
		[SerializeField] private PlayerObject _pr;

		private Action _onMove;
		private Vector2 _homePoint;
		private AstarAI _ai;
		private readonly int _hashIdle = Animator.StringToHash("[Anm] NPCIdle");
		private readonly int _hashMove = Animator.StringToHash("[Anm] NPCMove");
		
		public Action OnMove { get { return _onMove; } set { _onMove = value; } }
		public Vector2 HomePoint => _homePoint;
		public AstarAI AI;
		
		private void Awake()
		{
			_homePoint = transform.position;
			_ai = GetComponent<AstarAI>();
		}

		private void Update()
		{
			_onMove?.Invoke();
		}

		public void ChangeToIdleState(Animator animator)
		{
			AnimStateManager.ChangeAnimationState(animator, _hashMove);
		}

		public void ChangeToMoveState(Animator animator)
		{
			AnimStateManager.ChangeAnimationState(animator, _hashMove);
		}
	}
}

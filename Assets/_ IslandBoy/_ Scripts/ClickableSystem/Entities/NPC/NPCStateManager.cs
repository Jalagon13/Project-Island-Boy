using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

namespace IslandBoy
{
	public class NPCStateManager : MonoBehaviour
	{
		[SerializeField] private PlayerObject _pr;

		private Vector2 _homePoint;
		private IAstarAI _ai;
		private bool _canMove = false;
		private readonly int _hashIdle = Animator.StringToHash("[Anm] NPCIdle");
		private readonly int _hashMove = Animator.StringToHash("[Anm] NPCMove");
		
		public Vector2 HomePoint => _homePoint;
		public IAstarAI AI => _ai;
		public bool CanMove => _canMove;
		
		private void Awake()
		{
			_homePoint = new(999,999);
			_ai = GetComponent<IAstarAI>();
		}
		
		private void Update()
		{
			
		}
		
		public void SetCanMove(bool _)
		{
			_canMove = _;
			if(_canMove)
			{
				AnimStateManager.ChangeAnimationState(transform.GetChild(0).GetComponent<Animator>(), _hashIdle);
			}
			else
			{
				AnimStateManager.ChangeAnimationState(transform.GetChild(0).GetComponent<Animator>(), _hashMove);
			}
		}
		
		public void SetHomePoint(Vector2 newPoint)
		{
			_homePoint = newPoint;
		}
		
		public void Seek(Vector2 destination)
		{
			_ai.destination = destination;
		}

		public void ChangeToIdleState(Animator animator)
		{
			AnimStateManager.ChangeAnimationState(animator, _hashIdle);
		}

		public void ChangeToMoveState(Animator animator)
		{
			AnimStateManager.ChangeAnimationState(animator, _hashMove);
		}
	}
}

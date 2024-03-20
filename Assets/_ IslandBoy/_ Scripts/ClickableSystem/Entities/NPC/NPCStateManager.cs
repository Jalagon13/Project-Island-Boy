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
		private readonly int _hashBound = Animator.StringToHash("[Anm] NPCBound");
		private Animator _anim;
		private NPC _npc;

		public Vector2 HomePoint => _homePoint;
		public IAstarAI AI => _ai;
		public bool CanMove => _canMove;
		
		private void Awake()
		{
			_homePoint = new(999,999);
			_ai = GetComponent<IAstarAI>();
			_anim = transform.GetChild(0).GetComponent<Animator>();
			_npc = GetComponent<NPC>();
		}
		
		private void Update()
		{
			if (!_npc.IsFree)
            {
				_canMove = false;
				if (_anim.GetCurrentAnimatorStateInfo(0).shortNameHash != _hashBound)
					ChangeToBoundState(_anim);
			}
		}
		
		public void SetCanMove(bool _)
		{
			_canMove = _;
			if(_canMove)
			{
				//ChangeToMoveState(_anim);
			}
			else
			{
				ChangeToIdleState(_anim);
			}
		}
		
		public void SetHomePoint(Vector2 newPoint)
		{
			_homePoint = newPoint;
		}
		
		public void Seek(Vector2 destination)
		{
			// flip rotation depending on if moving left or right
			if (destination[0] > transform.position.x)
				transform.GetChild(0).rotation = new Quaternion(0f, 180f, 0f, 1f);
			else
				transform.GetChild(0).rotation = new Quaternion(0f, 0f, 0f, 1f);

			_ai.destination = destination;
		}

		public void ChangeToBoundState(Animator animator)
		{
			AnimStateManager.ChangeAnimationState(animator, _hashBound);
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

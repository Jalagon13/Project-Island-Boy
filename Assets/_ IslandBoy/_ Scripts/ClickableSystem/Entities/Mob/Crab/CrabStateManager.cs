using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace IslandBoy
{
	public class CrabStateManager : MonoBehaviour
	{
		public PlayerObject PR;

		public readonly int HashIdle = Animator.StringToHash("[Anm] CrabIdle");
		public readonly int HashMove = Animator.StringToHash("[Anm] CrabMove");
		public readonly int HashChase = Animator.StringToHash("[Anm] CrabChase");

		private AIPath _agent;
		private Vector2 _homePosition;
		private bool _isAgro;
		
		public AIPath AI => _agent;
		public Vector2 HomePosition => _homePosition;
		public bool IsAgro => _isAgro;

		private void Awake()
		{
			_agent = GetComponent<AIPath>();
			_homePosition = transform.position;
		}
		
		public void SetAgro()
		{
			_isAgro = true;
		}

		public void ChangeToIdleState(Animator animator)
		{
			AnimStateManager.ChangeAnimationState(animator, HashIdle);
		}

		public void ChangeToMoveState(Animator animator)
		{
			AnimStateManager.ChangeAnimationState(animator, HashMove);
		}

		public void ChangeToChaseState(Animator animator)
		{
			AnimStateManager.ChangeAnimationState(animator, HashChase);
		}

		public void Seek(Vector2 pos)
		{
			_agent.destination = pos;
		}

		public bool PlayerClose(float distance)
		{
			return Vector3.Distance(gameObject.transform.position, PR.Position) < distance;
		}

		public bool CanGetToPlayer()
		{

			// WIP
			return true;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace IslandBoy
{
	public class GhostStateManager : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;

		private Vector2 _homePosition;
		private Rigidbody2D _rb;
		private bool _agitated;
		
		public Vector2 HomePosition => _homePosition;
		public PlayerObject PlayerObject => _po;
		public Rigidbody2D RigidBody => _rb;
		public bool Agitated => _agitated;
		public readonly int HashIdle = Animator.StringToHash("[Anm] GhostIdle");
		public readonly int HashMove = Animator.StringToHash("[Anm] GhostMove");
		public readonly int HashChase = Animator.StringToHash("[Anm] GhostChase");

		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
			_homePosition = transform.position;
		}
		
		private void Update() 
		{
			if(_agitated) return;
			
			if(PlayerClose(5))
			{
				if(PlayerInLineOfSight())
				{
					_agitated = true;
				}
			}
		}
		
		public void ChargePlayer(Vector3 direction, float force)
		{
			_rb.AddForce(direction * force, ForceMode2D.Impulse);
		}
		
		private bool PlayerInLineOfSight()
		{
			Vector2 origin = transform.position;
			Vector2 target = _po.Position + Vector2.up * 0.5f;
			Vector2 direction = (target - origin).normalized;
			RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, Mathf.Infinity);
			
			foreach (RaycastHit2D item in hits)
			{
				if(item.collider.gameObject.layer == 8)
				{
					return false;
				}
			}
			
			return true;
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

		public bool PlayerClose(float distance)
		{
			return Vector3.Distance(transform.position, _po.Position) < distance;
		}
	}
}

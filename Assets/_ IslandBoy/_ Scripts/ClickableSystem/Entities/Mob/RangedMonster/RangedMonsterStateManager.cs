using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Pathfinding;
using UnityEngine;

namespace IslandBoy
{
	public class RangedMonsterStateManager : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private Ammo _launchPrefab;
		[SerializeField] private MMF_Player _spawnFeedback;
		[SerializeField] private MMF_Player _shootFeedback;
		[SerializeField] private float _shootCooldown;
		
		private Vector2 _velocity = Vector2.up; // Initial direction of movement
		private Rigidbody2D _rb;
		private Animator _anim;
		private bool _isAttacking;

		
		private void Awake() 
		{
			_rb = GetComponent<Rigidbody2D>();
			_rb.velocity = new Vector2(0.5f, 0.5f) * 3;
			_spawnFeedback?.PlayFeedbacks();
			_anim = GetComponentInChildren<Animator>();
		}
		
		private IEnumerator Start() 
		{
			yield return new WaitForSeconds(_shootCooldown);
			
			_shootFeedback?.PlayFeedbacks();
			
			StartCoroutine(Start());
		}
		
		private void FixedUpdate()
		{
			_rb.velocity = _rb.velocity.normalized * 3;
			_velocity = _rb.velocity;
			
			if(_rb.velocity.sqrMagnitude == 0)
			{
				_rb.velocity = new Vector2(0.5f, 0.5f) * 5;
			}
		}

		private void Update()
		{
			if (_isAttacking)
            {
				_isAttacking = false;
				_anim.SetBool("isAttacking", false);
			}
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			System.Random rnd = new System.Random();
			var randomRotationAngle = rnd.Next(-90, 90); 
			var newAngle = Quaternion.AngleAxis(randomRotationAngle, Vector2.up) * other.contacts[0].normal;
			_rb.velocity = Vector2.Reflect(_velocity, newAngle);
		}
		
		public void OnHit()
		{
			Vector2 direction = (transform.position - (Vector3)_po.Position).normalized;
			_rb.velocity = direction;
		}
		
		public void ShootPlayer()
		{
			Ammo ammo = Instantiate(_launchPrefab, transform.position, Quaternion.identity);
			Vector3 direction = (_po.Position - (Vector2)ammo.transform.position).normalized;
			ammo.Setup(direction);
			_anim.SetBool("isAttacking", true);
			_isAttacking = true;
			// need visual indicator for mosnter spawning
		}
	}
}

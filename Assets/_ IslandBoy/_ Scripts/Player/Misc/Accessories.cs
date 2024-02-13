using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
	public class Accessories : MonoBehaviour
	{
		[SerializeField] protected PlayerObject _pr;
		[SerializeField] private ItemDatabaseObject _db;

		[Header("Accessory Icons")]
		[SerializeField] private GameObject _dashIcon;
		[SerializeField] private AudioClip _dashSound;

		// Dash vars
		private PlayerInput _dashInput;
		private AccessoryObject _dash;
		private Vector2 _direction;
		private Timer _dashTimer;
		private bool _canDash = true;
		private bool _usingDash = false;
		private float _dashCurrentTime;
		private bool _dashOnCooldown = false;
		private bool _dashEquipped;

		private void Awake()
		{
			_dashTimer = new(3);
			_dashTimer.OnTimerEnd += AllowDash;
			
			_dashInput = new PlayerInput();
			_dashInput.Player.Dash.started += PerformDash;

			foreach (ItemObject item in _db.Database)
			{
				switch (item.AccessoryType)
				{
					case AccessoryType.Dash:
						_dash = (AccessoryObject)item;
						break;
				}
			}
		}

		private void FixedUpdate()
		{
			_dashTimer.Tick(Time.deltaTime);
			// Dash();
		}

		public void EnableDash()
		{
			_dashInput.Enable();
			_dashIcon.SetActive(_canDash);
			_dashEquipped = _canDash;
		}

		public void DisableDash()
		{
			_dashInput.Disable();
			_dashIcon.SetActive(false);
			_dashEquipped = false;
			// _dashOnCooldown = false;
			// _usingDash = false;
			// _dashCurrentTime = 0;
			// GetComponent<PlayerMoveInput>().Speed = GetComponent<PlayerMoveInput>().BaseSpeed;
		}
		
		private void AllowDash()
		{
			_canDash = true;
			_dashIcon.SetActive(_dashEquipped);
		}

		private void PerformDash(InputAction.CallbackContext context)
		{
			if(_canDash)
			{
				_canDash = false;
				_dashIcon.SetActive(false);
				_dashTimer.RemainingSeconds = 3;
				_direction = _pr.GameObject.GetComponent<PlayerMoveInput>().LastNonZeroMoveDirection;
				MMSoundManagerSoundPlayEvent.Trigger(_dashSound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
				
				StartCoroutine(DashDelay());
			}
			
			// if (!_dashOnCooldown && !_usingDash && (_direction[0] != 0 || _direction[1] != 0))
			// {
			// 	_usingDash = true;
			// 	_dashCurrentTime = 0;
			// 	GetComponent<PlayerMoveInput>().enabled = false;
			// 	GetComponent<Rigidbody2D>().AddForce(_direction * 50, ForceMode2D.Impulse);
			// }
		}
		
		private IEnumerator DashDelay()
		{
			GetComponent<PlayerMoveInput>().enabled = false;
			GetComponent<Rigidbody2D>().AddForce(_direction * 80, ForceMode2D.Impulse);
			
			yield return new WaitForSeconds(.25f);
			
			GetComponent<PlayerMoveInput>().enabled = true;
		}

		private void PerformDash()
		{
			if (_dashCurrentTime >= _dash.Length)
			{
				_usingDash = false;
				_dashCurrentTime = 0;
				_dashOnCooldown = true;
				_dashIcon.SetActive(false);
				GetComponent<PlayerMoveInput>().Speed = GetComponent<PlayerMoveInput>().BaseSpeed;
			}
			else
			{
				GetComponent<PlayerMoveInput>().Speed = _dash.DashSpeed;
			}
		}

		private void Dash()
		{
			if (_dashOnCooldown)
			{
				if (_dashCurrentTime >= _dash.Cooldown)
				{
					_dashOnCooldown = false;
					_dashCurrentTime = 0;
					_dashIcon.SetActive(true);
				}
				else
					_dashCurrentTime += Time.deltaTime;
			}
			else if (_usingDash)
			{
				_dashCurrentTime += Time.deltaTime;
				PerformDash();
			}
		}
	}
}

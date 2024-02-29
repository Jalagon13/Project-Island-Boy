using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
	public class CursorControl : MonoBehaviour
	{
		[SerializeField] private PlayerObject _pr;
		[SerializeField] private SwingControl _sc;
		[SerializeField] private TilemapObject _floorTm;
		[SerializeField] private TilemapObject _wallTm;
		// [SerializeField] private float _clickCd = 0.1f;
		[SerializeField] private ItemParameter _hitParameter;
		[SerializeField] private ItemParameter _clickDistanceParameter;
		[SerializeField] private ItemParameter _miningSpeedParameter;

		private PlayerInput _input;
		private SpriteRenderer _sr;
		private Slot _focusSlotRef;
		private Clickable _currentClickable;
		private Timer _clickTimer;
		private Vector2 _previousCenterPos;
		private Vector2 _currentCenterPos;
		private bool _canUseActions = true;
		private bool _heldDown;
		private float _currentClickDistance;
		private float _startingClickDistance = 1;

		private void Awake()
		{
			_input = new();
			_input.Player.PrimaryAction.started += Hit;
			_input.Player.PrimaryAction.performed += Hold;
			_input.Player.PrimaryAction.canceled += Hold;
			_input.Player.SecondaryAction.started += Interact;
			_input.Enable();

			_clickTimer = new(0);

			_sr = GetComponent<SpriteRenderer>();
			_currentClickDistance = _startingClickDistance;

			GameSignals.FOCUS_SLOT_UPDATED.AddListener(FocusSlotUpdated);
			GameSignals.DAY_END.AddListener(DisableAbilityToHit);
			GameSignals.DAY_START.AddListener(EnableAbilityToHit);
			GameSignals.PLAYER_DIED.AddListener(DisableAbilityToHit);
			GameSignals.GAME_PAUSED.AddListener(DisableAbilityToHit);
			GameSignals.GAME_UNPAUSED.AddListener(EnableAbilityToHit);
			GameSignals.ITEM_DEPLOYED.AddListener(RefreshCd);
		}

		private void OnDestroy()
		{
			GameSignals.FOCUS_SLOT_UPDATED.RemoveListener(FocusSlotUpdated);
			GameSignals.DAY_END.RemoveListener(DisableAbilityToHit);
			GameSignals.DAY_START.RemoveListener(EnableAbilityToHit);
			GameSignals.PLAYER_DIED.RemoveListener(DisableAbilityToHit);
			GameSignals.GAME_PAUSED.RemoveListener(DisableAbilityToHit);
			GameSignals.GAME_UNPAUSED.RemoveListener(EnableAbilityToHit);
			GameSignals.ITEM_DEPLOYED.RemoveListener(RefreshCd);

			_input.Disable();
		}

		private void FixedUpdate()
		{
			transform.SetPositionAndRotation(CalcPosition(), Quaternion.identity);

			_clickTimer.Tick(Time.deltaTime);

			if (_heldDown)
				ExecuteHit();

			CheckWhenEnterNewTile();
			UpdateCurrentClickable();
			DisplayCursor();
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if(collision.TryGetComponent<Clickable>(out var clickable))
			{
				if (_focusSlotRef.ItemObject is WallObject || _focusSlotRef.ItemObject is FloorObject || _focusSlotRef.ItemObject is DeployObject)
					return;

				if ((_focusSlotRef.ToolType == clickable.BreakType && clickable.IsTierCompatibleWith(_focusSlotRef.ToolTier, clickable.BreakTier)) || clickable is Interactable)
					clickable.ShowDisplay();
				else
					clickable.HideDisplay();
			}
		}

		private void RefreshCd(ISignalParameters parameters)
		{
			_clickTimer.RemainingSeconds = CalcMiningSpeed();
		}

		private void Hold(InputAction.CallbackContext context)
		{
			_heldDown = context.performed;
		}

		private void Hit(InputAction.CallbackContext context)
		{
			ExecuteHit();
		}
		
		private void ExecuteHit()
		{
			if (Pointer.IsOverUI() || _focusSlotRef.ItemObject is not ToolObject || _clickTimer.RemainingSeconds > 0 || !_canUseActions || _focusSlotRef == null) return;
				
			_sc.PerformAnimation();
			HitTilemap();
			
			if (_currentClickable != null && _currentClickable is not Entity)
			{
				_currentClickable.OnHit(_focusSlotRef.ToolType, CalcToolHitAmount(), true, _focusSlotRef.ToolTier);
				_clickTimer.RemainingSeconds = CalcMiningSpeed();
				return;
			}
		}

		private void HitTilemap()
		{
			var pos = Vector3Int.FloorToInt(transform.position);
			
			if(_wallTm.Tilemap.HasTile(pos))
				_wallTm.DynamicTilemap.Hit(pos, _focusSlotRef.ToolType);
			else if(_floorTm.Tilemap.HasTile(pos))
				_floorTm.DynamicTilemap.Hit(pos, _focusSlotRef.ToolType);
			
			_clickTimer.RemainingSeconds = CalcMiningSpeed();
		}
		
		private void CheckWhenEnterNewTile()
		{
			_currentCenterPos = CalcCenterTile();

			if (_currentCenterPos != _previousCenterPos)
			{
				OnEnterNewTile();

				_previousCenterPos = _currentCenterPos;
			}
		}

		private void OnEnterNewTile()
		{
			_currentClickable = null;

			Signal signal = GameSignals.CURSOR_ENTERED_NEW_TILE;
			signal.ClearParameters();
			signal.AddParameter("CenterPosition", _currentCenterPos);
			signal.Dispatch();
		}

		private void DisplayCursor()
		{
			_sr.enabled = _currentClickable == null;
		}

		private void Interact(InputAction.CallbackContext context)
		{
			if (!_canUseActions) return;

			Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);

			foreach (Collider2D col in colliders)
			{
				var results = col.GetComponents<Interactable>();

				foreach (Interactable interactable in results)
				{
					interactable.Interact();
				}
			}
		}
		
		private void FocusSlotUpdated(ISignalParameters parameters)
		{
			if (parameters.HasParameter("FocusSlot"))
			{
				_focusSlotRef = (Slot)parameters.GetParameter("FocusSlot");
				_currentClickDistance = CalcClickDistance();
			}
		}

		private float CalcMiningSpeed()
		{
			if (_focusSlotRef.ItemObject == null) return 0;

			if (_focusSlotRef.ItemObject.DefaultParameterList.Contains(_miningSpeedParameter))
			{
				var index = _focusSlotRef.ItemObject.DefaultParameterList.IndexOf(_miningSpeedParameter);
				var miningSpeedParameter = _focusSlotRef.ItemObject.DefaultParameterList[index];
				var value = (float)miningSpeedParameter.Value;
				var mSpeed = value / 60;
				
				return mSpeed;
			}

			return 0.25f;
		}

		private int CalcToolHitAmount()
		{
			if (_focusSlotRef.ItemObject == null) return 0;

			if (_focusSlotRef.ItemObject.DefaultParameterList.Contains(_hitParameter))
			{
				var index = _focusSlotRef.ItemObject.DefaultParameterList.IndexOf(_hitParameter);
				var powerParameter = _focusSlotRef.ItemObject.DefaultParameterList[index];

				return (int)powerParameter.Value;
			}

			return 0;
		}

		private float CalcClickDistance()
		{
			if (_focusSlotRef == null) return _startingClickDistance;
			if (_focusSlotRef.ItemObject == null) return _startingClickDistance;

			if (_focusSlotRef.ItemObject.DefaultParameterList.Contains(_clickDistanceParameter))
			{
				var index = _focusSlotRef.ItemObject.DefaultParameterList.IndexOf(_clickDistanceParameter);
				var clickDistanceParameter = _focusSlotRef.ItemObject.DefaultParameterList[index];

				return clickDistanceParameter.Value;
			}

			return _startingClickDistance;
		}

		private void UpdateCurrentClickable()
		{
			Clickable lastestClickable = ClickableFound();

			if (lastestClickable != null)
			{
				if (_currentClickable == lastestClickable) return;

				_currentClickable = lastestClickable;
			}
			else
			{
				if (_currentClickable == null) return;
				_currentClickable = null;
			}
		}

		private Clickable ClickableFound()
		{
			Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);
			List<Clickable> clickablesFound = new();

			if (colliders.Count() > 0)
			{
				foreach (Collider2D c in colliders)
				{
					if (c.TryGetComponent(out Clickable clickable))
					{
						clickablesFound.Add(clickable);
					}
				}
			}

			return clickablesFound.Count > 0 ? clickablesFound.Last() is Entity ? null : clickablesFound.Last() : null;
		}

		private void DisableAbilityToHit(ISignalParameters parameters)
		{
			_canUseActions = false;
		}

		private void EnableAbilityToHit(ISignalParameters parameters)
		{
			_canUseActions = true;
		}

		private Vector2 CalcCenterTile()
		{
			int tileX = Mathf.FloorToInt(transform.position.x);
			int tileY = Mathf.FloorToInt(transform.position.y);

			return new Vector2(tileX + 0.5f, tileY + 0.5f);
		}

		public bool IsClear()
		{
			var colliders = Physics2D.OverlapBoxAll(CalcPosition(), new Vector2(0.5f, 0.5f), 0);

			foreach(Collider2D col in colliders)
			{
				if(col.gameObject.layer == 3) 
					return false;
			}

			return true;
		}


		public void PlaceDeployable(GameObject deployable, Sprite s = null)
		{
			var position = _currentCenterPos -= new Vector2(0.5f, 0.5f);
			var d = Instantiate(deployable, position, Quaternion.identity);
			if (s != null) d.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = s;
			
			if(LevelControl.GetDP() != null)
			{
				d.transform.SetParent(LevelControl.GetDP().transform);
			}
		}

		private Vector2 CalcPosition()
		{
			Vector2 taPosition;
			Vector2 playerPos = transform.root.transform.localPosition + new Vector3(0, -0.3f, 0);
			Vector2 direction = (_pr.MousePosition - playerPos).normalized;

			taPosition = Vector2.Distance(playerPos, _pr.MousePosition) > _currentClickDistance ? (playerPos += new Vector2(0, 0.25f)) + (direction * _currentClickDistance) : _pr.MousePosition;

			return taPosition;
		}
	}
}

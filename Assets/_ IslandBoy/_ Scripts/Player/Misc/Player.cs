using MoreMountains.Tools;
using SingularityGroup.HotReload;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
	public enum PlayerStatType
	{
		Health,
		Energy,
		Mana
	}
	
	public enum RestedStatus
	{
		Bad, // When you let day end
		Okay, // When you sleep out in the open
		Good // When you sleep in a valid house
	}

	[RequireComponent(typeof(KnockbackFeedback))]
	public class Player : MonoBehaviour
	{
		public static RestedStatus RESTED_STATUS;
		
		[Header("Player Stats")]
		[SerializeField] private PlayerObject _pr;
		[SerializeField] private int _maxHp;
		[SerializeField] private int _maxNrg;
		[SerializeField] private int _maxMp;
		[Header("Parameters")]
		[SerializeField] private float _manaGenRate;
		[SerializeField] private float _energyRestorePercent;
		[SerializeField] private float _hpRestorePercent;
		[SerializeField] private float _iFrameDuration;
		[SerializeField] private float _deathTimer;
		[SerializeField] private AudioClip _damageSound;

		private KnockbackFeedback _knockback;
		private Collider2D _playerCollider;
		private Slot _focusSlot;
		private Timer _iFrameTimer;
		private GameObject _sr;
		private Vector2 _spawnPoint;
		private int _currentHp, _currentNrg, _currentMp, _armorHead, _armorChest, _armorLeg;
		private bool _canDepleteEnergy;

		[SerializeField] private int _skinNum;
			

		private void Awake()
		{
			_knockback = GetComponent<KnockbackFeedback>();
			_playerCollider = GetComponent<Collider2D>();
			_iFrameTimer = new Timer(_iFrameDuration);
			_spawnPoint = transform.position;
			_sr = transform.GetChild(0).gameObject;

			GameSignals.CLICKABLE_CLICKED.AddListener(OnSwing);
			GameSignals.DAY_START.AddListener(PlacePlayerAtSpawnPoint);
			GameSignals.DAY_START.AddListener(ResetEnergy);
			GameSignals.FOCUS_SLOT_UPDATED.AddListener(FocusSlotUpdated);
			GameSignals.BED_TIME_EXECUTED.AddListener(ChangeSpawnPoint);
			GameSignals.ENABLE_STARTING_MECHANICS.AddListener(AllowEnergyDeplete);
			GameSignals.ENABLE_PAUSE.AddListener(EnablePauseInput);
			GameSignals.DISABLE_PAUSE.AddListener(DisablePauseInput);
			GameSignals.PLAYER_RESPAWN.AddListener(ResetHealth);
		}

		private void OnDestroy()
		{
			GameSignals.CLICKABLE_CLICKED.RemoveListener(OnSwing);
			GameSignals.DAY_START.RemoveListener(PlacePlayerAtSpawnPoint);
			GameSignals.DAY_START.RemoveListener(ResetEnergy);
			GameSignals.FOCUS_SLOT_UPDATED.RemoveListener(FocusSlotUpdated);
			GameSignals.BED_TIME_EXECUTED.RemoveListener(ChangeSpawnPoint);
			GameSignals.ENABLE_STARTING_MECHANICS.RemoveListener(AllowEnergyDeplete);
			GameSignals.ENABLE_PAUSE.RemoveListener(EnablePauseInput);
			GameSignals.DISABLE_PAUSE.RemoveListener(DisablePauseInput);
			GameSignals.PLAYER_RESPAWN.RemoveListener(ResetHealth);
		}

		private void Start()
		{
			_currentHp = _maxHp;
			_currentNrg = 0;
			_currentMp = _maxMp;
			_pr.Defense = 0;

			DispatchHpChange();
			DispatchNrgChange();
			DispatchMpChange();

			StartCoroutine(RegenOneMana());

			_pr.Skin = _skinNum;
		}

		private void Update()
		{
			_iFrameTimer.Tick(Time.deltaTime);
			_pr.Position = transform.position;
		}

		private IEnumerator RegenOneMana()
		{
			yield return new WaitForSeconds(_manaGenRate);

			if(_currentMp < _maxMp)
			{
				AddToMp(1);
			}

			StartCoroutine(RegenOneMana());
		}
		
		private void ResetHealth(ISignalParameters parameters)
		{
			_currentHp = _maxHp;
			DispatchHpChange();
		}

		private void AllowEnergyDeplete(ISignalParameters parameters)
		{
			_canDepleteEnergy = true;
		}
		
		private void PlacePlayerAtSpawnPoint(ISignalParameters parameters)
		{
			transform.SetPositionAndRotation(_spawnPoint, Quaternion.identity);
		}

		private void ResetEnergy(ISignalParameters parameters)
		{
			_currentNrg = 0;
			DispatchNrgChange();
		}


		private void ChangeSpawnPoint(ISignalParameters parameters)
		{
			_spawnPoint = transform.position;
		}

		private void FocusSlotUpdated(ISignalParameters parameters)
		{
			if (parameters.HasParameter("FocusSlot"))
			{
				_focusSlot = (Slot)parameters.GetParameter("FocusSlot");
			}
		}

		private void OnSwing(ISignalParameters parameters)
		{
			if(!_canDepleteEnergy) return;
			
			if(parameters.HasParameter("EnergyLost"))
			{
				var energyLost = (int)parameters.GetParameter("EnergyLost");
				AddToNrg(-energyLost);
			}
			else
			{
				AddToNrg(-1);
			}
		}

		#region HP Functions
		public bool CanHealHp()
		{
			bool fullHp = _currentHp >= _maxHp;

			return !fullHp;
		}
		public void HealHp(int amount, bool consumeFocusSlot = true)
		{
			_currentHp += amount;
			if (_currentHp > _maxHp)
				_currentHp = _maxHp;

			PopupMessage.Create(transform.position, $"+{amount} Health", Color.green, new(0.5f, 0.5f), 0.5f);

			Signal signal = GameSignals.PLAYER_HP_CHANGED;
			signal.ClearParameters();
			signal.AddParameter("CurrentHp", _currentHp);
			signal.AddParameter("MaxHp", _maxHp);
			signal.Dispatch();

			if(consumeFocusSlot)
				_focusSlot.InventoryItem.Count--;
		}
		public void AddToHp(int amount)
		{
			_currentHp += amount;
			if (_currentHp > _maxHp)
				_currentHp = _maxHp;

			if (_currentHp <= 0)
			{
				_currentHp = 0;
				StartCoroutine(PlayerDead());
			}

			Signal signal = GameSignals.PLAYER_HP_CHANGED;
			signal.ClearParameters();
			signal.AddParameter("CurrentHp", _currentHp);
			signal.AddParameter("MaxHp", _maxHp);
			signal.Dispatch();
		}
		public void DispatchHpChange()
		{
			Signal signal = GameSignals.PLAYER_HP_CHANGED;
			signal.ClearParameters();
			signal.AddParameter("CurrentHp", _currentHp);
			signal.AddParameter("MaxHp", _maxHp);
			signal.Dispatch();
		}
		#endregion

		#region NRG Functions
		public bool CanHealNrg()
		{
			bool fullNrg = _currentNrg >= _maxNrg;

			return !fullNrg;
		}
		public void HealNrg(int amount, bool consumeFocusSlot = true)
		{
			_currentNrg += amount;
			if (_currentNrg > _maxNrg)
				_currentNrg = _maxNrg;

			PopupMessage.Create(transform.position, $"+{amount} Energy", Color.yellow, new(0.5f, 0.5f), 0.5f);
			
			DispatchNrgChange();

			if (consumeFocusSlot)
				_focusSlot.InventoryItem.Count--;
		}
		public void AddToNrg(int amount)
		{
			_currentNrg += amount;

			if (_currentNrg > _maxNrg)
				_currentNrg = _maxNrg;

			if (_currentNrg <= 0)
			{
				_currentNrg = 0;
				StartCoroutine(PlayerDead());
			}

			Signal signal = GameSignals.PLAYER_NRG_CHANGED;
			signal.ClearParameters();
			signal.AddParameter("CurrentNrg", _currentNrg);
			signal.AddParameter("MaxNrg", _maxNrg);
			signal.Dispatch();
		}

		public void DispatchNrgChange()
		{
			Signal signal = GameSignals.PLAYER_NRG_CHANGED;
			signal.ClearParameters();
			signal.AddParameter("CurrentNrg", _currentNrg);
			signal.AddParameter("MaxNrg", _maxNrg);
			signal.Dispatch();
		}
		#endregion

		#region MP Functions
		public bool CanHealMp()
		{
			bool fullMp = _currentMp >= _maxMp;

			return !fullMp;
		}
		public void HealMp(int amount)
		{
			_currentMp += amount;
			if (_currentMp > _maxMp)
				_currentMp = _maxMp;

			PopupMessage.Create(transform.position, $"+{amount} Mana", Color.blue, new(0.5f, 0.5f), 0.5f);

			Signal signal = GameSignals.PLAYER_MP_CHANGED;
			signal.ClearParameters();
			signal.AddParameter("CurrentMp", _currentMp);
			signal.AddParameter("MaxMp", _maxMp);
			signal.Dispatch();

			_focusSlot.InventoryItem.Count--;
		}

		public bool HasEnoughManaToCast(int spellCost)
		{
			return _currentMp >= spellCost;
		}

		public void AddToMp(int amount)
		{
			_currentMp += amount;

			if (_currentMp > _maxMp)
				_currentMp = _maxMp;

			if (_currentMp <= 0)
				_currentMp = 0;

			Signal signal = GameSignals.PLAYER_MP_CHANGED;
			signal.ClearParameters();
			signal.AddParameter("CurrentMp", _currentMp);
			signal.AddParameter("MaxMp", _maxMp);
			signal.Dispatch();
		}
		public void DispatchMpChange()
		{
			Signal signal = GameSignals.PLAYER_MP_CHANGED;
			signal.ClearParameters();
			signal.AddParameter("CurrentMp", _currentMp);
			signal.AddParameter("MaxMp", _maxMp);
			signal.Dispatch();
		}
		#endregion

		public void Damage(int amount, Vector2 damagerPosition)
		{
			if (!CanDamage()) return;

			amount = calcdamage(amount);
				 
			_currentHp -= amount;
			_iFrameTimer.RemainingSeconds = _iFrameDuration;

			DispatchHpChange();
			DispatchPlayerDamaged(amount, damagerPosition);

			PopupMessage.Create(transform.position, $"{amount}", Color.red, new(0.5f, 0.5f), 1f);
			MMSoundManagerSoundPlayEvent.Trigger(_damageSound, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position);

			if (_currentHp <= 0)
				StartCoroutine(PlayerDead());
			else
				_knockback.PlayFeedback(damagerPosition, 5f);
		}

		private void DispatchPlayerDamaged(int dmgAmount, Vector2 damagerPosition)
		{
			Signal signal = GameSignals.PLAYER_DAMAGED;
			signal.ClearParameters();
			signal.AddParameter("DamageAmount", dmgAmount);
			signal.AddParameter("DamagerPosition", damagerPosition);
			signal.Dispatch();
		}


		private int calcdamage(int damage)
		{
			if (damage <= _pr.Defense)
				return 1;
			else
				return damage - _pr.Defense;
		}

		public bool CanDamage()
		{
			return _iFrameTimer.RemainingSeconds <= 0;
		}

		private IEnumerator PlayerDead()
		{
			GameSignals.PLAYER_DIED.Dispatch();
			HidePlayer();

			yield return new WaitForSeconds(_deathTimer);

			// Get the current active scene
			Scene currentScene = SceneManager.GetActiveScene();
	   		int sceneIndex = currentScene.buildIndex;
			
			GameSignals.PLAYER_RESPAWN.Dispatch();
			if(sceneIndex == 2) // Surface Death
			{
				transform.SetPositionAndRotation(_spawnPoint, Quaternion.identity);
			}
			else if(sceneIndex == 3) // Cave Death
			{
				GameSignals.PLAYER_RESPAWN_IN_CAVE.Dispatch();
			}
			
			ShowPlayer();
		}

		public void NextSkin()
		{
			int num = _pr.Skin + 1;
			if (num > 1)
				num = 0;
			SetSkin(num);
		}

		public void SetSkin(int num)
		{
			_pr.Skin = num;
		}

		/// <summary>
		/// Hides/disables the player sprite
		/// </summary>
		private void HidePlayer()
		{
			_playerCollider.enabled = false;
			_sr.SetActive(false);
		}

		/// <summary>
		/// Shows/enables the player sprite
		/// </summary>
		private void ShowPlayer()
		{
			_playerCollider.enabled = true;
			_sr.SetActive(true);
		}

		private void DisablePauseInput(ISignalParameters parameters) // When player select screen is active, hide the player sprites
		{
			HidePlayer();
		}

		private void EnablePauseInput(ISignalParameters parameters) // When player select screen is finished, show the player sprites
		{
			ShowPlayer();
		}
	}
}

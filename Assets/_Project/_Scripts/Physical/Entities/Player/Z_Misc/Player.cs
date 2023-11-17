using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public enum PlayerStatType
    {
        Health,
        Energy,
        Mana
    }

    [RequireComponent(typeof(KnockbackFeedback))]
    public class Player : MonoBehaviour
    {
        [Header("Player Stats")]
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private int _maxHp;
        [SerializeField] private int _maxNrg;
        [SerializeField] private int _maxMp;
        [Header("Player Consume Cooldowns")]
        [SerializeField] private float _healHpCooldown;
        [SerializeField] private float _healNrgCooldown;
        [SerializeField] private float _healMpCooldown;
        [Header("Parameters")]
        [SerializeField] private float _manaGenRate;
        [SerializeField] private float _iFrameDuration;
        [SerializeField] private float _deathTimer;
        [SerializeField] private AudioClip _damageSound;

        private KnockbackFeedback _knockback;
        private Collider2D _playerCollider;
        private Vector2 _spawnPoint;
        private Slot _focusSlot;
        private Timer _iFrameTimer, _hpCdTimer, _nrgCdTimer, _mpCdTimer;
        private int _currentHp, _currentNrg, _currentMp;

        private void Awake()
        {
            _knockback = GetComponent<KnockbackFeedback>();
            _playerCollider = GetComponent<Collider2D>();
            _iFrameTimer = new Timer(_iFrameDuration);
            _spawnPoint = transform.position;

            GameSignals.CLICKABLE_CLICKED.AddListener(OnSwing);
            GameSignals.DAY_START.AddListener(PlacePlayerAtSpawnPoint);
            GameSignals.FOCUS_SLOT_UPDATED.AddListener(FocusSlotUpdated);
            GameSignals.BED_TIME_EXECUTED.AddListener(ChangeSpawnPoint);
        }

        private void OnDestroy()
        {
            GameSignals.CLICKABLE_CLICKED.RemoveListener(OnSwing);
            GameSignals.DAY_START.RemoveListener(PlacePlayerAtSpawnPoint);
            GameSignals.FOCUS_SLOT_UPDATED.RemoveListener(FocusSlotUpdated);
            GameSignals.BED_TIME_EXECUTED.RemoveListener(ChangeSpawnPoint);
        }

        private void Start()
        {
            _currentHp = _maxHp;
            _currentNrg = _maxNrg;
            _currentMp = _maxMp;

            DispatchHpChange();
            DispatchNrgChange();
            DispatchMpChange();

            _hpCdTimer = new Timer(_healHpCooldown);
            _nrgCdTimer = new Timer(_healNrgCooldown);
            _mpCdTimer = new Timer(_healMpCooldown);

            StartCoroutine(RegenOneMana());
        }

        private void Update()
        {
            _iFrameTimer.Tick(Time.deltaTime);
            _hpCdTimer.Tick(Time.deltaTime);
            _nrgCdTimer.Tick(Time.deltaTime);
            _mpCdTimer.Tick(Time.deltaTime);
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

        private void PlacePlayerAtSpawnPoint(ISignalParameters parameters)
        {
            transform.SetPositionAndRotation(_spawnPoint, Quaternion.identity);
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
            AddToNrg(-1);
        }

        #region HP Functions
        private bool CanHealHp()
        {
            bool inCooldown = _hpCdTimer.RemainingSeconds > 0;
            bool fullHp = _currentHp >= _maxHp;

            return !inCooldown && !fullHp;
        }
        public void HealHp(int amount)
        {
            if (!CanHealHp()) return;

            _currentHp += amount;
            if (_currentHp > _maxHp)
                _currentHp = _maxHp;

            _hpCdTimer.RemainingSeconds = _healHpCooldown;

            PopupMessage.Create(transform.position, $"+{amount} Health", Color.green, new(0.5f, 0.5f), 0.5f);

            Signal signal = GameSignals.PLAYER_HP_CHANGED;
            signal.ClearParameters();
            signal.AddParameter("CurrentHp", _currentHp);
            signal.AddParameter("MaxHp", _maxHp);
            signal.AddParameter("HpTimer", _hpCdTimer);
            signal.Dispatch();

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
        private void DispatchHpChange()
        {
            Signal signal = GameSignals.PLAYER_HP_CHANGED;
            signal.ClearParameters();
            signal.AddParameter("CurrentHp", _currentHp);
            signal.AddParameter("MaxHp", _maxHp);
            if (_hpCdTimer != null)
                signal.AddParameter("HpTimer", _hpCdTimer);
            signal.Dispatch();
        }
        #endregion

        #region NRG Functions
        private bool CanHealNrg()
        {
            bool inCooldown = _nrgCdTimer.RemainingSeconds > 0;
            bool fullHp = _currentNrg >= _maxNrg;

            return !inCooldown && !fullHp;
        }
        public void HealNrg(int amount)
        {
            if (!CanHealNrg()) return;

            _currentNrg += amount;
            if (_currentNrg > _maxNrg)
                _currentNrg = _maxNrg;

            _nrgCdTimer.RemainingSeconds = _healNrgCooldown;

            PopupMessage.Create(transform.position, $"+{amount} Energy", Color.yellow, new(0.5f, 0.5f), 0.5f);

            Signal signal = GameSignals.PLAYER_NRG_CHANGED;
            signal.ClearParameters();
            signal.AddParameter("CurrentNrg", _currentNrg);
            signal.AddParameter("MaxNrg", _maxNrg);
            if (_nrgCdTimer != null)
                signal.AddParameter("NrgTimer", _nrgCdTimer);
            signal.Dispatch();

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
        private void DispatchNrgChange()
        {
            Signal signal = GameSignals.PLAYER_NRG_CHANGED;
            signal.ClearParameters();
            signal.AddParameter("CurrentNrg", _currentNrg);
            signal.AddParameter("MaxNrg", _maxNrg);
            if (_nrgCdTimer != null)
                signal.AddParameter("NrgTimer", _nrgCdTimer);
            signal.Dispatch();
        }
        #endregion

        #region MP Functions
        private bool CanHealMp()
        {
            bool inCooldown = _mpCdTimer.RemainingSeconds > 0;
            bool fullHp = _currentMp >= _maxMp;

            return !inCooldown && !fullHp;
        }
        public void HealMp(int amount)
        {
            if (!CanHealMp()) return;

            _currentMp += amount;
            if (_currentMp > _maxMp)
                _currentMp = _maxMp;

            _mpCdTimer.RemainingSeconds = _healMpCooldown;

            PopupMessage.Create(transform.position, $"+{amount} Mana", Color.blue, new(0.5f, 0.5f), 0.5f);

            Signal signal = GameSignals.PLAYER_MP_CHANGED;
            signal.ClearParameters();
            signal.AddParameter("CurrentMp", _currentMp);
            signal.AddParameter("MaxMp", _maxMp);
            if (_mpCdTimer != null)
                signal.AddParameter("MpTimer", _mpCdTimer);
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
        private void DispatchMpChange()
        {
            Signal signal = GameSignals.PLAYER_MP_CHANGED;
            signal.ClearParameters();
            signal.AddParameter("CurrentMp", _currentMp);
            signal.AddParameter("MaxMp", _maxMp);
            if (_mpCdTimer != null)
                signal.AddParameter("MpTimer", _mpCdTimer);
            signal.Dispatch();
        }
        #endregion

        public void Damage(int amount, Vector2 damagerPosition)
        {
            if (!CanDamage()) return;
                 
            _currentHp -= amount;
            _iFrameTimer.RemainingSeconds = _iFrameDuration;

            DispatchHpChange();
            DispatchPlayerDamaged(amount, damagerPosition);

            PopupMessage.Create(transform.position, $"{amount}", Color.red, new(0.5f, 0.5f), 1f);
            AudioManager.Instance.PlayClip(_damageSound, false, true);

            if (_currentHp <= 0)
                StartCoroutine(PlayerDead());
            else
                _knockback.PlayFeedback(damagerPosition);
        }

        private void DispatchPlayerDamaged(int dmgAmount, Vector2 damagerPosition)
        {
            Signal signal = GameSignals.PLAYER_DAMAGED;
            signal.ClearParameters();
            signal.AddParameter("DamageAmount", dmgAmount);
            signal.AddParameter("DamagerPosition", damagerPosition);
            signal.Dispatch();
        }


        //WIP
        //private int CalcDamage(int damage)
        //{
        //    int dmg = damage - PR.Defense;

        //    if (dmg < 1)
        //        dmg = 1;

        //    return dmg;
        //}

        public bool CanDamage()
        {
            return _iFrameTimer.RemainingSeconds <= 0;
        }

        private IEnumerator PlayerDead()
        {
            GameSignals.PLAYER_DIED.Dispatch();
            _playerCollider.enabled = false;

            yield return new WaitForSeconds(_deathTimer);

            GameSignals.DAY_END.Dispatch();
            _playerCollider.enabled = true;
        }
    }
}
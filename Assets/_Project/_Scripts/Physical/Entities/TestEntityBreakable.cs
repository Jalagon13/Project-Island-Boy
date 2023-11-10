using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class TestEntityBreakable : MonoBehaviour, IBreakable
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private float _maxHitPoints;
        [SerializeField] private ToolType _harvestType;
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private AudioClip _deathSound;
        [SerializeField] private LootTable _lootTable;

        private float _currentHitPoints;

        public float MaxHitPoints { get { return _maxHitPoints; } set { _maxHitPoints = value; } }
        public float CurrentHitPoints { get { return _currentHitPoints; } set { _currentHitPoints = value; } }
        public ToolType BreakType { get { return _harvestType; } set { _harvestType = value; } }

        private void Awake()
        {
            _currentHitPoints = _maxHitPoints;
        }

        public void Hit(ToolType incomingToolType)
        {
            _currentHitPoints -= incomingToolType == _harvestType ? 2 : 1;

            GameSignals.RSC_HIT.Dispatch();

            if (_currentHitPoints <= 0)
                Break();

            AudioManager.Instance.PlayClip(_hitSound, false, true, 0.7f);

            if (transform.TryGetComponent(out KnockbackFeedback knockback))
                knockback.PlayFeedback(_pr.Position);
        }

        public void Break()
        {
            AudioManager.Instance.PlayClip(_deathSound, false, true, 0.75f);
            _lootTable.SpawnLoot(transform.position);

            Destroy(gameObject);
        }
    }
}

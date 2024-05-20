using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IslandBoy
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private PlayerObject _po;
        [SerializeField] private int _maxHealth;
        
        private HealthView _healthView;
        private int _currentHealth;
        
        public int CurrentHealth => _currentHealth;
        
        private void Awake()
        {
            _currentHealth = _maxHealth;
            
            // GameSignals.PLAYER_DAMAGED.AddListener(Damage);
            // GameSignals.PLAYER_HEALED.AddListener(Heal);// might change to ON_COSUME
            // GameSignals.PLAYER_DIED.AddListener(Die);
            // GameSignals.PLAYER_RESPAWNED.AddListener(RestoreAllHealth);
        }
        
        private void OnDestroy()
        {
            // GameSignals.PLAYER_DAMAGED.RemoveListener(Damage);
            // GameSignals.PLAYER_HEALED.RemoveListener(Heal);
            // GameSignals.PLAYER_DIED.RemoveListener(Die);
            // GameSignals.PLAYER_RESPAWNED.RemoveListener(RestoreAllHealth);
        }
        
        private void Start()
        {
            // Future note to self: This may cause some issues when creating a scene loading bootstrap
            _healthView = FindObjectOfType<HealthView>();
        }

        // for testing vvvvv
        [Button("Restore")]
        public void TestRestore()
        {
            GameSignals.PLAYER_RESPAWNED.Dispatch();
        }
        [Button("Heal")]
        public void TestHeal()
        {
            GameSignals.PLAYER_HEALED.Dispatch();
        }
        [Button("Damage")]
        public void TestDamage()
        {
            GameSignals.PLAYER_DAMAGED.Dispatch();
        }
        // for testing ^^^^^

        private void Heal(ISignalParameters parameters)
        {
            //int healthValue = (int)parameters.GetParameter("HealthValue");
            //ItemObject consumeObject = (ConsumableObject)parameters.GetParameter("ConsumeItem");

            _currentHealth++;//= healthValue;
            
            if(_currentHealth > _maxHealth)
                _currentHealth = _maxHealth;
            //_po.PlayerInventory.RemoveItem(consumeObject, 1);
            UpdateView();
        }

        private void Damage(ISignalParameters parameters)
        {
            _currentHealth--;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                GameSignals.PLAYER_DIED.Dispatch();
            }
            UpdateView();
        }

        private void Die(ISignalParameters parameters)
        {
            // TBD
            Debug.Log("You Died");
        }

        private void RestoreAllHealth(ISignalParameters parameters)
        {
            _currentHealth = _maxHealth;
            UpdateView();
        }


        private void UpdateView()
        {
            _healthView.UpdateHealth(_currentHealth);
        }
    }
}

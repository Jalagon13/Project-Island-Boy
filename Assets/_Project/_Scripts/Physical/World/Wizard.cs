using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
    public class Wizard : MonoBehaviour
    {
        [SerializeField] private int _unlockFee;
        [SerializeField] private TextMeshProUGUI _entranceText;
        [SerializeField] private UnityEvent _onUnlock;

        private bool _unlocked;

        private void Awake()
        {
            _entranceText.text = $"Desert Biome Entrance<br>{_unlockFee} XP to make the spikes disappear.";
        }

        public void TryUnlockDesert()
        {
            if(_unlocked)
            {
                PopupMessage.Create(transform.position, $"I already unlocked this biome for you", Color.yellow, Vector2.up, 1f);
                return;
            }

            if(PlayerExperience.Experience.Count >= _unlockFee)
            {
                PopupMessage.Create(transform.position, $"Desert Biome unlocked! Have fun!", Color.green, Vector2.up, 1f);
                PlayerExperience.AddExerpience(-_unlockFee);

                _unlocked = true;
                _onUnlock?.Invoke();
            }
            else
            {
                PopupMessage.Create(transform.position, $"You need more XP", Color.yellow, Vector2.up, 1f);
            }
        }
    }
}

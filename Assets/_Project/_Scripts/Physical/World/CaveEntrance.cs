using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class CaveEntrance : MonoBehaviour
    {
        [SerializeField] private int _unlockFee;
        [SerializeField] private TextMeshProUGUI _entranceText;

        private bool _unlocked;

        private void Awake()
        {
            _entranceText.text = $"I need {_unlockFee} XP to access this Mine Shaft";
        }

        public void TryUnlockCave()
        {
            if(_unlocked)
            {
                PopupMessage.Create(transform.position, $"I already unlocked this cave entrance", Color.yellow, Vector2.up, 1f);
                return;
            }

            if(PlayerExperience.Experience.Count >= _unlockFee)
            {
                _unlocked = true;
                PopupMessage.Create(transform.position, $"Cave Entrance Unlocked!", Color.green, Vector2.up, 1f);
                PlayerExperience.AddExerpience(-_unlockFee);
            }
            else
            {
                PopupMessage.Create(transform.position, $"I need more XP", Color.yellow, Vector2.up, 1f);
            }
        }

        public void EnterCave()
        {
            if (_unlocked)
            {
                PopupMessage.Create(transform.position, $"Cave still under construction...", Color.green, Vector2.up, 1f);
            }
            else
            {
                PopupMessage.Create(transform.position, $"I need to unlock this cave first", Color.red, Vector2.up, 1f);
            }
        }
    }
}

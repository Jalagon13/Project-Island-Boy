using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class ExpUnlockable : MonoBehaviour
    {
        [SerializeField] private int _unlockFee;
        [TextArea()]
        [SerializeField] private string _description;
        [SerializeField] private TextMeshProUGUI _entranceText;
        [SerializeField] private UnityEvent _onUnlock;

        private bool _unlocked;

        private void Awake()
        {
            _entranceText.text = $"I need {_unlockFee} XP to unlock<br>{_description}";
        }

        public void TryUnlockDesert()
        {
            if(_unlocked)
            {
                PopupMessage.Create(transform.position, $"I already unlocked this.", Color.yellow, Vector2.up, 1f);
                return;
            }

            if(PlayerExperience.Experience.Count >= _unlockFee)
            {
                PopupMessage.Create(transform.position, $"{_description} unlocked!", Color.green, Vector2.up, 1f);
                PlayerExperience.AddExerpience(-_unlockFee);

                _unlocked = true;
                _onUnlock?.Invoke();
            }
            else
            {
                PopupMessage.Create(transform.position, $"You need more XP", Color.yellow, Vector2.up, 1f);
            }
        }

        // super temporary
        public void EndGame()
        {
            // Transition to end scene.
            SceneManager.LoadScene("End");
        }
    }
}

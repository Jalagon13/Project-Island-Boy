using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace IslandBoy
{
    public class DefenseDisplay : MonoBehaviour
    {
        [SerializeField] private PlayerObject _pr;
        [SerializeField] private MMF_Player _tutorialFeedback;
        private GameObject _display;
        private TMPro.TextMeshProUGUI _defenseText;

        void Awake()
        {
            _display = transform.GetChild(0).gameObject;
            _defenseText = _display.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            GameSignals.UPDATE_DEFENSE.AddListener(UpdateDefense);
            GameSignals.ENABLE_STARTING_MECHANICS.AddListener(EnableDisplay);
            _display.SetActive(false);
        }

        private void OnDestroy()
        {
            GameSignals.UPDATE_DEFENSE.RemoveListener(UpdateDefense);
            GameSignals.ENABLE_STARTING_MECHANICS.RemoveListener(EnableDisplay);
        }

        private void UpdateDefense(ISignalParameters parameters)
        {
            _defenseText.text = _pr.Defense.ToString();
        }

        private void EnableDisplay(ISignalParameters parameters)
        {
            _tutorialFeedback?.PlayFeedbacks();
        }
    }
}

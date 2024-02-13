using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class DefenseDisplay : MonoBehaviour
    {
        [SerializeField] private PlayerObject _pr;
        private GameObject _display;
        private TMPro.TextMeshProUGUI _defenseText;

        void Awake()
        {
            _display = transform.GetChild(0).gameObject;
            _defenseText = _display.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            GameSignals.INVENTORY_OPEN.AddListener(EnableDisplay);
            GameSignals.INVENTORY_CLOSE.AddListener(DisableDisplay);
            GameSignals.UPDATE_DEFENSE.AddListener(UpdateDefense);
            _display.SetActive(false);
        }

        private void OnDestroy()
        {
            GameSignals.INVENTORY_OPEN.RemoveListener(EnableDisplay);
            GameSignals.INVENTORY_CLOSE.RemoveListener(DisableDisplay);
            GameSignals.UPDATE_DEFENSE.RemoveListener(UpdateDefense);
        }

        private void UpdateDefense(ISignalParameters parameters)
        {
            _defenseText.text = _pr.Defense.ToString();
        }

        private void EnableDisplay(ISignalParameters parameters)
        {
            _display.SetActive(true);
        }

        private void DisableDisplay(ISignalParameters parameters)
        {
            _display.SetActive(false);
        }
    }
}

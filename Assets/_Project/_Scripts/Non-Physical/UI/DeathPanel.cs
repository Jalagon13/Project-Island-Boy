using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class DeathPanel : MonoBehaviour
    {
        private RectTransform _deathPanel;

        private void Awake()
        {
            _deathPanel = transform.GetChild(0).GetComponent<RectTransform>();
            GameSignals.PLAYER_DIED.AddListener(EnableDeathPanel);
            GameSignals.DAY_END.AddListener(DisableDeathPanel);
        }

        private void OnDestroy()
        {
            GameSignals.PLAYER_DIED.RemoveListener(EnableDeathPanel);
            GameSignals.DAY_END.RemoveListener(DisableDeathPanel);
        }

        private void EnableDeathPanel(ISignalParameters parameters)
        {
            _deathPanel.gameObject.SetActive(true);
        }

        private void DisableDeathPanel(ISignalParameters parameters)
        {
            _deathPanel.gameObject.SetActive(false);
        }
    }
}

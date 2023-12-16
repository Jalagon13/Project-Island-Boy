using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class MouseSlotControl : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;

        private MouseSlot _mouseSlot;
        private bool _instructionsEnabled;

        private void Awake()
        {
            GameSignals.MOUSE_SLOT_HAS_ITEM.AddListener(EnableInstructions);
            GameSignals.MOUSE_SLOT_GIVES_ITEM.AddListener(DisableInstructions);
        }

        private void OnDestroy()
        {
            GameSignals.MOUSE_SLOT_HAS_ITEM.RemoveListener(EnableInstructions);
            GameSignals.MOUSE_SLOT_GIVES_ITEM.RemoveListener(DisableInstructions);
        }

        private void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            transform.position = Camera.main.WorldToScreenPoint(_pr.MousePosition);
        }

        private void EnableInstructions(ISignalParameters parameters)
        {
            if (parameters.HasParameter("MouseSlot"))
            {
                _mouseSlot = (MouseSlot)parameters.GetParameter("MouseSlot");

                if (_mouseSlot.ItemObject is DeployObject || _mouseSlot.ItemObject is WallObject || _mouseSlot.ItemObject is WallObject)
                {
                    //Debug.Log("Enable Instructions");
                }
            }
        }

        private void DisableInstructions(ISignalParameters parameters = null)
        {
            //Debug.Log("Disable Instructions");
        }
    }
}

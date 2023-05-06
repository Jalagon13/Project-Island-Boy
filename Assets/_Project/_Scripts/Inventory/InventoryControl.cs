using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class InventoryControl : MonoBehaviour
    {
        private PlayerInput _input;
        private RectTransform _mainInventory;
        private bool _inventoryOpen;

        private void Awake()
        {
            _input = new PlayerInput();
            _mainInventory = transform.GetChild(1).GetComponent<RectTransform>();
            _input.Player.ToggleInventory.started += ToggleInventory;

        }
        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void Start()
        {
            CloseInventory();
        }

        public void ToggleInventory(InputAction.CallbackContext context)
        {
            if (_inventoryOpen)
                CloseInventory();
            else
                OpenInventory();
        }

        private void CloseInventory()
        {
            _mainInventory.gameObject.SetActive(false);
            _inventoryOpen = false;
        }

        private void OpenInventory()
        {
            _mainInventory.gameObject.SetActive(true);
            _inventoryOpen = true;
        }
    }
}

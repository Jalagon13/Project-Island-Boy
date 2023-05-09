using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class InventoryControl : MonoBehaviour
    {
        private Inventory _inventory;
        private MouseItemHolder _mouseItemHolder;
        private PlayerInput _input;
        private RectTransform _mainInventory;
        private bool _inventoryOpen;


        private void Awake()
        {
            _input = new PlayerInput();
            _inventory = GetComponent<Inventory>();
            _mainInventory = transform.GetChild(1).GetComponent<RectTransform>();
            _mouseItemHolder = transform.GetChild(2).GetComponent<MouseItemHolder>();

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
            if (_mouseItemHolder.HasItem()) return;

            _mainInventory.gameObject.SetActive(false);
            _inventoryOpen = false;

            foreach (InventorySlot slot in _inventory.InventorySlots)
            {
                slot.InventoryOpen = false;
            }
        }

        private void OpenInventory()
        {
            _mainInventory.gameObject.SetActive(true);
            _inventoryOpen = true;

            foreach (InventorySlot slot in _inventory.InventorySlots)
            {
                slot.InventoryOpen = true;
            }
        }
    }
}

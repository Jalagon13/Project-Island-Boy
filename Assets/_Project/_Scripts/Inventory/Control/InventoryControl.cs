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
        private CraftSlotsControl _craftSlotsControl;
        private bool _inventoryOpen;


        private void Awake()
        {
            _input = new PlayerInput();
            _inventory = GetComponent<Inventory>();
            _craftSlotsControl = GetComponent<CraftSlotsControl>();
            _mainInventory = transform.GetChild(0).GetComponent<RectTransform>();
            _mouseItemHolder = transform.GetChild(2).GetComponent<MouseItemHolder>();

            _input.Player.ToggleInventory.started += ToggleInventory;
        }
        private void OnEnable()
        {
            _input.Enable();
            CraftStation.OnCraftStationInteract += CraftStationInteract;
        }

        private void OnDisable()
        {
            _input.Disable();
            CraftStation.OnCraftStationInteract -= CraftStationInteract;
        }

        private void Start()
        {
            CloseInventory();
        }

        private void CraftStationInteract(RecipeDatabaseObject rdb, AugmentDatabaseObject adb)
        {
            OpenInventory();

            _craftSlotsControl.RefreshCraftingMenu(rdb, adb);
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

            _craftSlotsControl.RefreshCraftingMenu(_craftSlotsControl.DefaultRdb, null);
            _mainInventory.gameObject.SetActive(false);
            _inventoryOpen = false;

            foreach (Slot slot in _inventory.InventorySlots)
            {
                slot.InventoryOpen = false;
            }
        }

        public void OpenInventory()
        {
            _mainInventory.gameObject.SetActive(true);
            _inventoryOpen = true;

            foreach (Slot slot in _inventory.InventorySlots)
            {
                slot.InventoryOpen = true;
            }
        }
    }
}

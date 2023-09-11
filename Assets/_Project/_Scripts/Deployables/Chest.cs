using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class Chest : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private PlayerReference _pr;

        private Canvas _slotCanvas;

        private void Awake()
        {
            _slotCanvas = transform.GetChild(2).GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            _pr.Inventory.InventoryControl.OnInventoryClosed += CloseChest;
        }

        private void OnDisable()
        {
            _pr.Inventory.InventoryControl.OnInventoryClosed -= CloseChest;
        }

        private void Start()
        {
            EnableChestSlots(false);
        }

        private void OnDestroy()
        {
            foreach (Transform transform in _slotCanvas.transform.GetChild(0))
            {
                Slot slot = transform.GetComponent<Slot>();
                if(slot.ItemObject != null)
                {
                    Vector3 offset = new(0.5f, 0.5f);
                    WorldItemManager.Instance.SpawnItem(this.transform.position += offset, slot.ItemObject, slot.InventoryItem.Count);
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                HandleInventoryControl();
                EnableChestSlots(true);
            }
        }

        private void CloseChest(object obj, EventArgs args)
        {
            EnableChestSlots(false);
        }

        public void EnableChestSlots(bool val)
        {
            _slotCanvas.gameObject.SetActive(val);
        }

        private void HandleInventoryControl()
        {
            _pr.Inventory.InventoryControl.ChestInteract(this);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class MouseSlot : Slot
    {
        private bool _gotItemThisFrame;
        private bool _gaveItemThisFrame;

        public GameObject ItemGo 
        { 
            get 
            {
                GameObject item = transform.GetChild(0).gameObject;
                return item != null ? item : null;
            } 
        }

        private void Update()
        {
            DispatchHandle();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {

        }

        // note refactor this later 
        private void DispatchHandle()
        {
            if (HasItem())
            {
                if (_gotItemThisFrame) return;

                Signal signal = GameSignals.MOUSE_SLOT_HAS_ITEM;
                signal.ClearParameters();
                signal.AddParameter("MouseSlot", this);
                signal.Dispatch();

                _gotItemThisFrame = true;
                _gaveItemThisFrame = false;
            }
            else
            {
                if (_gaveItemThisFrame) return;

                GameSignals.MOUSE_SLOT_GIVES_ITEM.Dispatch();
                _gaveItemThisFrame = true;
                _gotItemThisFrame = false;
            }
        }





        public void DeleteMouseItem()
        {
            if(transform.GetChild(0).gameObject != null)
                Destroy(transform.GetChild(0).gameObject);
        }

        public void CreateMouseItem(GameObject itemGo, ItemObject itemObject, int count = 1)
        {
            if (!HasItem())
            {
                GameObject newItemGo = Instantiate(itemGo, transform);
                InventoryItem item = newItemGo.GetComponent<InventoryItem>();
                item.Initialize(itemObject, itemObject.DefaultParameterList, count);
            }
        }

        public bool TryToCraftItem(GameObject itemGo, ItemObject outputItem, int outputAmount)
        {
            if (HasItem())
            {
                if (ItemObject.Stackable)
                {
                    if(ItemObject == outputItem)
                    {
                        if(InventoryItem.Count < _pr.Inventory.MaxStack && (_pr.Inventory.MaxStack - InventoryItem.Count) >= outputAmount)
                        {
                            InventoryItem.Count += outputAmount;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                CreateMouseItem(itemGo, outputItem, outputAmount);
                return true;
            }
        }

        public void GiveItemToSlot(Transform slot)
        {
            if (HasItem())
            {
                var item = transform.GetChild(0);
                item.SetParent(slot, false);
            }
        }

        public void GiveCopyOfItemToSlot(Transform slot)
        {
            var copy = Instantiate(transform.GetChild(0));
            copy.SetParent(slot, false);
        }
    }
}

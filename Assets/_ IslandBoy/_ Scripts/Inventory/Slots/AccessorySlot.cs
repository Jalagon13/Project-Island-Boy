using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class AccessorySlot : Slot
    {
        [SerializeField] private ItemParameter _defenseParameter;
        [SerializeField] private ItemParameter _durabilityParameter;

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift))
            {
                MoveItemIntoInventory();
            }
            else if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
            {
                if (_mouseItemHolder.HasItem())
                {
                    if (_mouseItemHolder.ItemObject.AccessoryType != AccessoryType.None)
                    {
                        if (HasItem())
                        {
                            //UnEquipArmor();
                            SwapThisItemAndMouseItem();
                            //EquipArmor();
                        }
                        else
                        {
                            _mouseItemHolder.GiveItemToSlot(transform);
                            TooltipManager.Instance.Show(ItemObject.GetDescription(), ItemObject.Name);
                            PlaySound();
                            //EquipArmor();
                        }
                    }
                }
                else
                {
                    if (HasItem())
                    {
                        GiveThisItemToMouseHolder();
                        TooltipManager.Instance.Hide();
                        //UnEquipArmor();
                    }
                }
            }

            GameSignals.SLOT_CLICKED.Dispatch();
        }

        private void DecreaseDurability(int decreaseAmount) 
        {
            if (InventoryItem == null || decreaseAmount == 0) return;

            var itemParams = InventoryItem.CurrentParameters;

            int index = itemParams.IndexOf(_durabilityParameter);
            float newValue = itemParams[index].Value - decreaseAmount;
            itemParams[index] = new ItemParameter
            {
                Parameter = _durabilityParameter.Parameter,
                Value = newValue
            };

            InventoryItem.UpdateDurabilityCounter();

            StartCoroutine(FrameDelay());
        }

        private IEnumerator FrameDelay()
        {
            yield return new WaitForEndOfFrame();

            if (!HasItem())
            {
                //UnEquipArmor();
            }
        }

        private int GetDefenseFromItem()
        {
            var itemParams = InventoryItem.CurrentParameters;

            if (itemParams.Contains(_defenseParameter))
            {
                int index = itemParams.IndexOf(_defenseParameter);
                return (int)itemParams[index].Value;
            }

            return 0;
        }
    }
}

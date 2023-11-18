using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class UpgradeSlot : Slot
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log($"Mouse Item holder null from upgrade slot? {_mouseItemHolder == null}");
                if (_mouseItemHolder.HasItem())
                {
                    if (_mouseItemHolder.ItemObject is ToolObject)
                    {
                        if (HasItem())
                        {
                            RemoveTool();
                            SwapThisItemAndMouseItem();
                            AddTool();
                        }
                        else
                        {
                            _mouseItemHolder.GiveItemToSlot(transform);
                            TooltipManager.Instance.Show(ItemObject.GetDescription(), ItemObject.Name);
                            PlaySound();
                            AddTool();
                        }
                    }
                }
                else
                {
                    if (HasItem())
                    {
                        GiveThisItemToMouseHolder();
                        TooltipManager.Instance.Hide();
                        RemoveTool();
                    }
                }
            }
        }

        private void AddTool()
        {
            Debug.Log("Tool Added");
        }

        private void RemoveTool()
        {
            Debug.Log("Tool Removed");
        }
    }
}

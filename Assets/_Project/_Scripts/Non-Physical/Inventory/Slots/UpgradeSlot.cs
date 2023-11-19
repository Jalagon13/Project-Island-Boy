using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IslandBoy
{
    public class UpgradeSlot : Slot
    {
        [SerializeField] private GameObject _inventoryItemPrefab;
        [SerializeField] private TextMeshProUGUI _upgradeText;
        [SerializeField] private TextMeshProUGUI _needText;
        [SerializeField] private Button _upgradeButton;

        private CraftingRecipeObject _upgradeRecipe;
        private int _xpNeedAmount;

        private void Start()
        {
            EnableUpgradeUI(false);
        }

        private void OnDisable()
        {
            if (HasItem())
            {
                _pr.Inventory.AddItem(ItemObject, InventoryItem.Count, ItemObject.DefaultParameterList);

                ClearSlot();
                EnableUpgradeUI(false);
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
            {
                if (_mouseItemHolder.HasItem())
                {
                    if (_mouseItemHolder.ItemObject is ToolObject)
                    {
                        if (HasItem())
                        {
                            ToolRemoved();
                            SwapThisItemAndMouseItem();
                            ToolAdded();
                        }
                        else
                        {
                            _mouseItemHolder.GiveItemToSlot(transform);
                            //TooltipManager.Instance.Show(ItemObject.GetDescription(), ItemObject.Name);
                            PlaySound();
                            ToolAdded();
                        }
                    }
                }
                else
                {
                    if (HasItem())
                    {
                        GiveThisItemToMouseHolder();
                        TooltipManager.Instance.Hide();
                        ToolRemoved();
                    }
                }
            }
        }

        private void ToolAdded()
        {
            if (ItemObject is not ToolObject) return;

            ToolObject tool = (ToolObject)ItemObject;
            _upgradeRecipe = tool.UpgradeRecipe;
            _xpNeedAmount = tool.XpForUpgrade;

            if (tool.UpgradeRecipe != null) // if tool has an upgradable item
            {
                string needList = string.Empty;

                foreach (ItemAmount itemAmount in tool.UpgradeRecipe.ResourceList)
                {
                    needList += $"<br>* {itemAmount.Item.Name} ({itemAmount.Amount})";
                }

                needList += $"<br>* {tool.XpForUpgrade} XP";

                _upgradeText.text = $"Upgrade:<br>{tool.UpgradeRecipe.OutputItem.Name}";
                _needText.text = $"Need:{needList}";

                EnableUpgradeUI(true);
            }
        }

        private void ToolRemoved()
        {
            _upgradeRecipe = null;
            _xpNeedAmount = 0;

            EnableUpgradeUI(false);
        }

        public void TryToUpgradeTool()
        {

            if (_mouseItemHolder.HasItem())
            {
                PopupMessage.Create(_pr.Position, "My mouse slot is full", Color.yellow, Vector2.up, 1f);
                return;
            }

            bool canCraft = false;

            foreach (ItemAmount ia in _upgradeRecipe.ResourceList)
            {
                canCraft = _pr.Inventory.Contains(ia.Item, ia.Amount);

                if (!canCraft) break;
            }

            if (PlayerExperience.Experience.Count < _xpNeedAmount)
            {
                PopupMessage.Create(_pr.Position, "I need more XP to craft this", Color.yellow, Vector2.up, 1f);
                return;
            }

            if (_upgradeRecipe.ResourceList.Count <= 0)
                canCraft = true;

            if (canCraft)
                UpgradeTool();
            else
                PopupMessage.Create(_pr.Position, "I'm missing some resources", Color.yellow, Vector2.up, 1f);
        }


        private void UpgradeTool()
        {
            if(_mouseItemHolder.TryToCraftItem(_inventoryItemPrefab, _upgradeRecipe.OutputItem, _upgradeRecipe.OutputAmount))
            {
                foreach (ItemAmount ia in _upgradeRecipe.ResourceList)
                {
                    _pr.Inventory.RemoveItem(ia.Item, ia.Amount);
                }

                PlayerExperience.AddExerpience(-_xpNeedAmount);
                AudioManager.Instance.PlayClip(_popSound, false, true);
                GameSignals.ITEM_CRAFTED.Dispatch();

                Destroy(InventoryItem.gameObject);
                ToolRemoved();
            }
        }

        private void EnableUpgradeUI(bool _)
        {
            _upgradeButton.gameObject.SetActive(_);
            _upgradeText.enabled = _;
            _needText.enabled = _;
        }
    }
}

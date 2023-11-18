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
            _upgradeButton.gameObject.SetActive(false);
            _upgradeText.enabled = false;
            _needText.enabled = false;
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

                _upgradeButton.gameObject.SetActive(true);
                _upgradeText.enabled = true;
                _needText.enabled = true;
            }
        }

        private void ToolRemoved()
        {
            Debug.Log("Tool Removed");

            _upgradeRecipe = null;
            _xpNeedAmount = 0;

            _upgradeButton.gameObject.SetActive(false);
            _upgradeText.enabled = false;
            _needText.enabled = false;
        }

        public void TryToUpgradeTool()
        {
            Debug.Log("Trying to upgrade tool");

            if (_mouseItemHolder.HasItem())
            {
                PopupMessage.Create(_pr.Position, "Mouse slot full!", Color.yellow, Vector2.up, 1f);
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
                PopupMessage.Create(_pr.Position, "Need more XP!", Color.yellow, Vector2.up, 1f);
                return;
            }

            if (_upgradeRecipe.ResourceList.Count <= 0)
                canCraft = true;

            if (canCraft)
                UpgradeTool();
            else
                PopupMessage.Create(_pr.Position, "Resources not met!", Color.yellow, Vector2.up, 1f);
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
    }
}

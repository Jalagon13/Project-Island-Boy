using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class TCCraftingUI : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private RectTransform _holder;
        [SerializeField] private TextMeshProUGUI _outputText;
        [SerializeField] private TextMeshProUGUI _ingredientText;
        [SerializeField] private TextMeshProUGUI _craftText;
        [Header("Buttons")]
        [SerializeField] private CraftBtn _craftBtn;
        [SerializeField] private IncreaseBtn _increaseBtn;
        [SerializeField] private DecreaseBtn _decreaseBtn;

        private CraftingRecipeObject _recipeToDisplay;
        private TimedConverter _tc;
        private int _recipeAmount;

        private void OnEnable()
        {
            if (_holder == null) return;
            _holder.gameObject.SetActive(false);
        }

        public void InjectRecipe(CraftingRecipeObject recipeObject)
        {
            _tc = transform.root.GetComponent<TimedConverter>();
            _holder.gameObject.SetActive(true);
            _recipeToDisplay = recipeObject;
            _outputText.text = $"{_recipeToDisplay.OutputItem.Name}<br>{_recipeToDisplay.OutputItem.GetDescription()}";
            _recipeAmount = 1;

            UpdateIngTexts();
            UpdateButtons();
            UpgradeCraftText();
        }
        public void IncreaseButton()
        {
            _recipeAmount++;
            UpdateButtons();
            UpdateIngTexts();
            UpgradeCraftText();
        }

        public void DecreaseButton()
        {
            _recipeAmount--;
            UpdateButtons();
            UpdateIngTexts();
            UpgradeCraftText();
        }

        public void CraftButton()
        {
            _tc.StartCrafting(_recipeToDisplay, _recipeAmount);
            UpdateButtons();
            UpdateIngTexts();
            UpgradeCraftText();
        }

        private void UpdateButtons()
        {
            if (CheckIfCanCraft())
            {
                // Enable Craft button
                _craftBtn.EnableButton();

                if (CanIncrease())
                {
                    // Enable Increase button
                    _increaseBtn.EnableButton();
                }
                else
                {
                    // Disable Increase button
                    _increaseBtn.DisableButton();
                }

                if (CanDecrease())
                {
                    // Enable Decrease button
                    _decreaseBtn.EnableButton();
                }
                else
                {
                    // Disable Decrease button
                    _decreaseBtn.DisableButton();
                }
            }
            else
            {
                // Disable all buttons
                _craftBtn.DisableButton();
                _increaseBtn.DisableButton();
                _decreaseBtn.DisableButton();
                _recipeAmount = 0;
            }
        }

        private void UpdateIngTexts()
        {
            string ingText = string.Empty;

            foreach (var ia in _recipeToDisplay.ResourceList)
            {
                ingText += $"{ ia.Item.Name} [{_pr.Inventory.GetItemAmount(ia.Item)}/{ia.Amount * _recipeAmount}]<br>";
            }

            _ingredientText.text = ingText;
        }

        private void UpgradeCraftText()
        {
            string amountText = string.Empty;

            if (_recipeAmount == 1)
                amountText = $"{1}";
            else
                amountText = $"(x{_recipeToDisplay.OutputAmount * _recipeAmount})";

            string outputText = _recipeAmount == 1 ? (_recipeToDisplay.OutputAmount * _recipeAmount) > 1 ? 
                $"(x{_recipeToDisplay.OutputAmount * _recipeAmount})" : string.Empty : amountText;
            _craftText.text = $"Craft {_recipeAmount} {outputText}";
        }

        private bool CheckIfCanCraft()
        {
            bool canCraft = false;

            foreach (ItemAmount ia in _recipeToDisplay.ResourceList)
            {
                canCraft = _pr.Inventory.Contains(ia.Item, ia.Amount * _recipeAmount);

                if (!canCraft) break;
            }

            if (_recipeToDisplay.ResourceList.Count <= 0)
                canCraft = true;

            return canCraft;
        }

        private bool CanDecrease()
        {
            foreach (var ia in _recipeToDisplay.ResourceList)
            {
                int subAmount = (ia.Amount * _recipeAmount) - ia.Amount;

                if (subAmount <= 0)
                    return false;
            }

            return true;
        }

        private bool CanIncrease()
        {
            foreach (var ia in _recipeToDisplay.ResourceList)
            {
                int amountInInv = _pr.Inventory.GetItemAmount(ia.Item);

                if (amountInInv > ia.Amount)
                {
                    // check if can increase
                    int newAmount = ia.Amount * (_recipeAmount + 1);
                    if (newAmount > amountInInv)
                        return false;
                }
            }

            return true;
        }


    }
}

using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class TCCraftingUI : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private RectTransform _holder;
        [SerializeField] private AudioClip _populateSound;
        [Header("UI")]
        [SerializeField] private Image _outputImage;
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

        public void PopulateRecipe(CraftingRecipeObject recipeObject)
        {
            _tc = transform.root.GetComponent<TimedConverter>();
            _holder.gameObject.SetActive(true);
            _recipeToDisplay = recipeObject;
            _outputText.text = $"{_recipeToDisplay.OutputItem.Name}";
            _recipeAmount = 1;
            _outputImage.sprite = recipeObject.OutputItem.UiDisplay;

            MMSoundManagerSoundPlayEvent.Trigger(_populateSound, MMSoundManager.MMSoundManagerTracks.UI, default);

            UpdateTexts();
        }

        public void IncreaseButton()
        {
            _recipeAmount++;
            UpdateTexts();
        }

        public void DecreaseButton()
        {
            _recipeAmount--;
            UpdateTexts();
        }

        public void CraftButton()
        {
            _tc.StartCrafting(_recipeToDisplay, _recipeAmount);
            UpdateTexts();
        }

        public void ResetCraftingUI()
        {
            _recipeAmount = 1;
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            UpdateIngTexts();
            UpdateButtons();
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
            string ingText = "Recipe:<br>";

            foreach (var ia in _recipeToDisplay.ResourceList)
            {
                string text = $"{ ia.Item.Name} [{_pr.Inventory.GetItemAmount(ia.Item)}/{ia.Amount * _recipeAmount}]";

                if(_pr.Inventory.GetItemAmount(ia.Item) >= (ia.Amount * _recipeAmount))
                    ingText += $"<color=white>{text}<color=white><br>";
                else
                    ingText += $"<color=red>{text}<color=red><br>";

            }

            _ingredientText.text = ingText;
        }

        private void UpgradeCraftText()
        {
            string amountText = string.Empty;

            if (_recipeAmount == 1)
                amountText = $"{1}";
            else
                amountText = _recipeAmount == _recipeToDisplay.OutputAmount * _recipeAmount ? string.Empty : $"(x{_recipeToDisplay.OutputAmount * _recipeAmount})";

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

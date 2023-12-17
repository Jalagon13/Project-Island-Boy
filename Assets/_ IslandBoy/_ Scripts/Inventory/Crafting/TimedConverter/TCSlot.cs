using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class TCSlot : MonoBehaviour
    {
        private CraftingRecipeObject _recipe;
        //private RectTransform _rscPanel;
        //private RectTransform _rscSlots;
        private CraftSlotImageHover _hoverImage;
        private Image _outputImage;
        private TextMeshProUGUI _amountText;

        public void Initialize(CraftingRecipeObject recipe)
        {
            SetGlobals(recipe);
            _recipe = recipe;
        }

        private void SetGlobals(CraftingRecipeObject recipe)
        {
            _outputImage = transform.GetChild(0).GetComponent<Image>();
            _hoverImage = transform.GetChild(0).GetComponent<CraftSlotImageHover>();
            //_rscPanel = transform.GetChild(1).GetComponent<RectTransform>();
            //_rscSlots = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
            _amountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            //_playerInventory = transform.root.GetChild(0).GetComponent<Inventory>();
            _recipe = recipe;
            _outputImage.sprite = recipe.OutputItem.UiDisplay;
            _hoverImage.SetItemDescription(recipe.OutputItem);
            _amountText.text = recipe.OutputAmount == 1 ? string.Empty : recipe.OutputAmount.ToString();
        }

        public void DisplayCraftingUI() // connected to this button
        {
            var tc = transform.root.GetComponent<TimedConverter>();
            tc.RefreshCraftingUI(_recipe);
        }
    }
}

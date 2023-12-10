using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class TCCraftingUI : MonoBehaviour
    {
        private CraftingRecipeObject _recipeToDisplay;
        private RectTransform _holder;

        //private void OnDisable()
        //{
        //    if (_holder == null) return;
        //    _holder.gameObject.SetActive(false);
        //}

        public void InjectRecipe(CraftingRecipeObject recipeObject)
        {
            _holder = transform.GetChild(0).GetComponent<RectTransform>();

            _recipeToDisplay = recipeObject;
            _holder.gameObject.SetActive(true);
            Debug.Log(_recipeToDisplay.OutputItem.Name);
        }
    }
}

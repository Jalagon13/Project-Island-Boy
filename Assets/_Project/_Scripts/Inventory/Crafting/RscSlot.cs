using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class RscSlot : MonoBehaviour
    {
        [SerializeField] private Sprite _experienceOrbSprite;

        private Image _rscImage;
        private TextMeshProUGUI _countText;
        private ItemObject _item;
        private RscSlotImageHover _imageHover;

        public void Initialize(ItemAmount ia)
        {
            _rscImage = transform.GetChild(1).GetComponent<Image>();
            _countText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            _imageHover = transform.GetChild(1).GetComponent<RscSlotImageHover>();
            _imageHover.OutputItem = ia.Item;

            _item = ia.Item;
            _rscImage.sprite = ia.Item.UiDisplay;
            _countText.text = ia.Amount.ToString();
        }

        public void InitializeExpSlot(AugmentRecipe ar)
        {
            _rscImage = transform.GetChild(1).GetComponent<Image>();
            _countText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            _imageHover = transform.GetChild(1).GetComponent<RscSlotImageHover>();
            _imageHover.SetCustomDescription(string.Empty, "Experience Level");

            _rscImage.sprite = _experienceOrbSprite;
            _countText.text = $"{ar.LevelsRequired}";
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class RscSlot : MonoBehaviour
    {
        private Image _rscImage;
        private TextMeshProUGUI _countText;
        private ItemObject _item;
        private RscSlotImageHover _imageHover;

        public void Initialize(ItemObject item, int amount)
        {
            _rscImage = transform.GetChild(1).GetComponent<Image>();
            _countText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            _imageHover = transform.GetChild(1).GetComponent<RscSlotImageHover>();
            _imageHover.OutputItem = item;

            _item = item;
            _rscImage.sprite = item.UiDisplay;
            _countText.text = amount.ToString();
        }
    }
}

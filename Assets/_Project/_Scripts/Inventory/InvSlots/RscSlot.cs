using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class RscSlot : MonoBehaviour
    {
        private Image _rscImage;
        private TextMeshProUGUI _countText;

        public void Initialize(ItemAmount ia)
        {
            _rscImage = transform.GetChild(1).GetComponent<Image>();
            _countText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            _rscImage.sprite = ia.Item.UiDisplay;
            _countText.text = ia.Amount.ToString();
        }
    }
}

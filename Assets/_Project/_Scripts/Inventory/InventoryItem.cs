using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class InventoryItem : MonoBehaviour, IInventoryItemInitializer
    {
        private Image _image;
        private TextMeshProUGUI _countText;
        private ItemObject _item;
        private int _count;

        public ItemObject Item { get { return _item; } }
        public int Count { get { return _count; } }

        public void Initialize(ItemObject item)
        {
            _image = GetComponent<Image>();
            _countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            _item = item;
            _image.sprite = item.UIDisplay;
            _count = 1;
            UpdateCounter();
        }

        public void IncrementCount()
        {
            _count++;
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            _countText.text = _count == 1 ? string.Empty : _count.ToString();
        }
    }
}

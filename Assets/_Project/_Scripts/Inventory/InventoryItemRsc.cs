using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class InventoryItemRsc : MonoBehaviour/*, IInventoryItemDrop*/
    {
        private Image _image;
        private TextMeshProUGUI _countText;
        private ResourceObject _item;
        private int _count;

        public ResourceObject Item { get { return _item; } }
        public int Count { get { return _count; }
            set 
            { 
                _count = value;

                UpdateCounter();

                if (_count <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void Initialize(ResourceObject item)
        {
            _image = GetComponent<Image>();
            _countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            _item = item;
            _image.sprite = item.UIDisplay;
            _count = 1;
            UpdateCounter();
        }

        public void DropInventoryItem(Vector2 position)
        {
            
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

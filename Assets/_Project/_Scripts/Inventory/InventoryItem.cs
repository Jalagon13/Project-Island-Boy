using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class InventoryItem : MonoBehaviour/*, IInventoryItemDrop*/
    {
        [SerializeField] private GameObject _durabilityCounterGo;
        [SerializeField] private ItemParameter _durabilityParameter;

        private Image _image;
        private TextMeshProUGUI _countText;
        private ItemObject _item;
        private List<ItemParameter> _currentParameters;
        private int _count;

        public ItemObject Item { get { return _item; } }
        public List<ItemParameter> CurrentParameters { get { return _currentParameters; } }

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

        public void Initialize(ItemObject item, List<ItemParameter> itemParameters = null, int count = 1)
        {
            _image = GetComponent<Image>();
            _countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _item = item;
            _image.sprite = item.UiDisplay;
            _count = count;
            _currentParameters = itemParameters;

            SetUpDurabilityCounter();
            UpdateCounter();
        }

        public void SetUpDurabilityCounter()
        {
            if(_currentParameters == null) return;
            if (!_currentParameters.Contains(_durabilityParameter)) return;

            int index = _item.DefaultParameterList.IndexOf(_durabilityParameter);
            int currentDurability = (int)_currentParameters[index].Value;
            int maxDurability = (int)_item.DefaultParameterList[index].Value;

            GameObject durabilityCounterGo = Instantiate(_durabilityCounterGo, transform);
            DurabilityCounter counter = durabilityCounterGo.GetComponent<DurabilityCounter>();

            counter.UpdateDurabilityCounter(maxDurability, currentDurability);
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

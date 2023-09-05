using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private GameObject _durabilityCounterGo;
        [SerializeField] private ItemParameter _durabilityParameter;

        private Image _image;
        private TextMeshProUGUI _countText;
        private ItemObject _item;
        private List<ItemParameter> _currentParameters;
        private List<IAugment> _augmentList = new();
        private GameObject _durabilityCounterRef;
        private int _count, _augmentOnItem;
        private bool _hasAugments, _itemReadyToExecute;

        public ItemObject Item { get { return _item; } }
        public List<ItemParameter> CurrentParameters { get { return _currentParameters; } }
        public bool HasAugments { get { return _hasAugments; } }
        public int AugmentsOnItem { get { return _augmentOnItem; } }

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
            _image.sprite = item.UiDisplay;
            _count = count;
            _item = item;
            _currentParameters = itemParameters != null ? new(itemParameters) : new();

            UpdateCounter();
        }

        public void InitializeAugment(AugmentObject augmentItem)
        {
            if (_augmentOnItem >= 3) return;

            GameObject ag = Instantiate(augmentItem.Augment, transform);
            IAugment augment = ag.GetComponent<IAugment>();
            _augmentList.Add(augment);
            _augmentOnItem++;
            _hasAugments = true;
        }

        public void ReadyItem()
        {
            _itemReadyToExecute = true;
        }

        public void ExecuteAugments(TileAction ta)
        {
            if (!_itemReadyToExecute) return;

            foreach (IAugment augment in _augmentList)
            {
                augment.Execute(ta);
            }

            _itemReadyToExecute = false;
        }

        public void UpdateDurabilityCounter()
        {
            if(_currentParameters == null) return;
            if (!_currentParameters.Contains(_durabilityParameter)) return;

            int index = _item.DefaultParameterList.IndexOf(_durabilityParameter);
            float currentDurability = _currentParameters[index].Value;
            float maxDurability = _item.DefaultParameterList[index].Value;

            if(_durabilityCounterRef == null)
            {
                _durabilityCounterRef = Instantiate(_durabilityCounterGo, transform);
            }

            DurabilityCounter counter = _durabilityCounterRef.GetComponent<DurabilityCounter>();
            counter.UpdateDurabilityCounter(maxDurability, currentDurability);


            if (currentDurability <= 0)
                Destroy(gameObject);
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

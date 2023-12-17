using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private AudioClip _readySound;
        [SerializeField] private GameObject _durabilityCounterGo;
        [SerializeField] private GameObject _cooldownCounterGo;
        [SerializeField] private ItemParameter _durabilityParameter;

        private Image _image;
        private TextMeshProUGUI _countText;
        private ItemObject _item;
        private List<ItemParameter> _currentParameters;
        private FillControl _durabilityFC;
        private FillControl _cooldownFC;
        private Timer _cooldownTimer;
        private float _cooldown;
        private int _count, _augmentOnItem;
        private bool _hasAugments;

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

        private void FixedUpdate()
        {
            if(_cooldownTimer != null)
            {
                _cooldownTimer.Tick(Time.deltaTime);
                _cooldownFC.UpdateFill(_cooldown, _cooldownTimer != null ? _cooldownTimer.RemainingSeconds : 0);
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

        public void UpdateDurabilityCounter()
        {
            if(_currentParameters == null) return;
            if (!_currentParameters.Contains(_durabilityParameter)) return;

            int index = _item.DefaultParameterList.IndexOf(_durabilityParameter);
            float currentDurability = _currentParameters[index].Value;
            float maxDurability = _item.DefaultParameterList[index].Value;

            if(_durabilityFC == null)
            {
                GameObject dc = Instantiate(_durabilityCounterGo, transform);
                _durabilityFC = dc.GetComponent<FillControl>();
            }

            _durabilityFC.UpdateFill(maxDurability, currentDurability);


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

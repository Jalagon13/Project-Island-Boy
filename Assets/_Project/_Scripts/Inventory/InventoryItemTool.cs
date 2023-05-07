using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class InventoryItemTool : MonoBehaviour, IInventoryItemInitializer
    {
        private Image _image;
        private Image _durabilityFill;
        private ToolObject _item;
        private int _currentDurability;

        public ItemObject Item { get { return _item; } }

        public void Initialize(ItemObject item)
        {
            _image = GetComponent<Image>();
            _durabilityFill = transform.GetChild(0).GetChild(1).GetComponent<Image>();

            _item = (ToolObject)item;
            _image.sprite = item.UIDisplay;
            _currentDurability = _item.DurabilityReference;
            UpdateDurabilityCounter();
        }

        private void UpdateDurabilityCounter()
        {
            _durabilityFill.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _item.MaxDurability, _currentDurability));
        }
    }
}

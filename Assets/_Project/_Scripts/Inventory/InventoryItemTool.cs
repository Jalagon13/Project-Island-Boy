using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class InventoryItemTool : MonoBehaviour/*, IInventoryItemDrop*/
    {
        private Image _image;
        private Image _durabilityFill;
        private ToolObject _item;
        private int _currentDurability;

        public ToolObject Item { get { return _item; } }

        public void Initialize(ToolObject item, int durability)
        {
            _image = GetComponent<Image>();
            _durabilityFill = transform.GetChild(0).GetChild(1).GetComponent<Image>();

            _item = item;
            _image.sprite = item.UIDisplay;
            _currentDurability = durability;
            UpdateDurabilityCounter();
        }

        public void DropInventoryItem(Vector2 position)
        {
            
        }

        private void UpdateDurabilityCounter()
        {
            _durabilityFill.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _item.MaxDurability, _currentDurability));
        }
    }
}

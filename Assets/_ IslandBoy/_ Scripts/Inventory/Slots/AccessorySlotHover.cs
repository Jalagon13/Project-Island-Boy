using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class AccessorySlotHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private ItemParameter _powerParameter;
        [SerializeField] private ItemParameter _coolDownParameter;

        private AccessorySlot _slot;

        private void Awake()
        {
            _slot = GetComponent<AccessorySlot>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_slot.MouseItemHolder.HasItem() || _slot.ItemObject == null) return;

            TooltipManager.Instance.Show(_slot.ItemObject.GetDescription(), _slot.ItemObject.Name);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.Hide();
        }
    }
}

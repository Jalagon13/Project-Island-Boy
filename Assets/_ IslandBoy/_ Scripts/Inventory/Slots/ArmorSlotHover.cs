using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class ArmorSlotHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private ItemParameter _powerParameter;
        [SerializeField] private ItemParameter _coolDownParameter;

        private ArmorSlot _slot;

        private void Awake()
        {
            _slot = GetComponent<ArmorSlot>();
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

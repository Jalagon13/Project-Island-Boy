using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class CraftSlotImageHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public ItemObject OutputItem { get; set; }

        private string _content;
        private string _header;

        private void OnDisable()
        {
            TooltipManager.Instance.Hide();
        }

        public void SetItemDescription(ItemObject item)
        {
            _content = item.GetDescription();
            _header = item.Name;
        }

        public void SetCustomDescription(string content, string header)
        {
            _content = content;
            _header = header;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.Instance.Show(_content, _header, new Vector2(-0.1f, 1.45f));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.Hide();
        }
    }
}

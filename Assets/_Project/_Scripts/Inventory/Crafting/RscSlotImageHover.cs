using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class RscSlotImageHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public ItemObject OutputItem { get; set; }

        private bool _customContent = false;
        private string _content;
        private string _header;

        private void OnDisable()
        {
            TooltipManager.Instance.Hide();
        }

        public void SetCustomDescription(string content, string header)
        {
            _customContent = true;
            _content = content;
            _header = header;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.Instance.Show(_customContent ? _content : OutputItem.GetDescription(),
                _customContent ? _header : OutputItem.Name, new Vector2(-0.1f, 1.45f));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.Hide();
        }
    }
}

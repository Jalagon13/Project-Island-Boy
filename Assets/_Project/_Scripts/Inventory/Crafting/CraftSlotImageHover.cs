using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class CraftSlotImageHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public ItemObject OutputItem { get; set; }

        private void OnDisable()
        {
            TooltipManager.Instance.Hide();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.Instance.Show(OutputItem.GetDescription(), OutputItem.Name, new Vector2(-0.1f, 1.45f));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.Hide();
        }
    }
}

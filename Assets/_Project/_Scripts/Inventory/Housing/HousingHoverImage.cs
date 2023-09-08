using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class HousingHoverImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            string header = "House Scanner";
            string content = "Adventurers will move into your island if there is vacant housing available.";

            TooltipManager.Instance.Show(content, header, new Vector2(1.1f, -0.1f));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.Hide();
        }
    }
}

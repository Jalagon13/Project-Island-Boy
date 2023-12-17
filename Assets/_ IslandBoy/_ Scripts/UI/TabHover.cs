using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class TabHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField] private string _header;
        [SerializeField] private string _content;

        private void OnDisable()
        {
            TooltipManager.Instance.Hide();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.Instance.Show(_content, _header, new Vector2(0.5f, -0.3f));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.Hide();
        }
    }
}

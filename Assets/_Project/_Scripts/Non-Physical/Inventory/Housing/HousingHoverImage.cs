using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class HousingHoverImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private string _header;
        private string _content;

        private void Start()
        {
            
        }

        private void OnDisable()
        {
            TooltipManager.Instance.Hide();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.Instance.Show(_content, _header, new Vector2(0.95f, -0.35f));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.Hide();
        }

        public void Initialize(string header, string content)
        {
            _header = header;
            _content = content;
        }
    }
}

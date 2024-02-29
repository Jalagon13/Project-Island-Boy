using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class BedNpcButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _arrowIcon;
        [SerializeField] private Color _selectColor;
        [SerializeField] private Color _deselectColor;
        private Image _image;
        private bool _selected;
        private Button _button;

        void Awake()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_selected) 
                _image.color = _selectColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_selected)
                _image.color = _deselectColor;
        }

        public void Select()
        {
            _arrowIcon.SetActive(true);
            _image.color = _selectColor;
            _selected = true;
        }

        public void Deselect()
        {
            _arrowIcon.SetActive(false);
            _image.color = _deselectColor;
            _selected = false;
        }
    }
}

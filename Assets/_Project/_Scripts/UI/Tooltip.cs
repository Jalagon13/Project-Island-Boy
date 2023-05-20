using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace IslandBoy
{
    [ExecuteInEditMode()]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TMP_Text _headerField;
        [SerializeField] private TMP_Text _contentField;
        [SerializeField] private LayoutElement _layoutElement;

        public void SetText(string content, string header = "")
        {
            _headerField.gameObject.SetActive(!string.IsNullOrEmpty(header));

            if (_headerField.gameObject.activeSelf)
                _headerField.text = header;

            _contentField.text = content;
        }

        private void Update()
        {
            if (Application.isEditor)
                _layoutElement.enabled = Math.Max(_headerField.preferredWidth, _contentField.preferredWidth) >= _layoutElement.preferredWidth; ;

            if (Application.isPlaying)
            {
                var mousePos = Mouse.current.position.ReadValue();

                transform.position = mousePos;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class PopupMessage : MonoBehaviour
    {
        private static int _sortingOrder;
        private const float DISAPPEAR_TIMER_MAX = 0.5f;

        private TextMeshPro _textMesh;
        private Timer _floatTimter;
        private float _disappearTimer;
        private bool _canShrink;
        private Color _textColor;
        private Vector3 _moveVector;

        private void Awake()
        {
            _textMesh = GetComponent<TextMeshPro>();
        }

        public static PopupMessage Create(Vector2 position, string text, Color textColor, Vector2 textOffset, float floatTimer = 0.01f)
        {
            Transform popupTransform = Instantiate(GameAssets.Instance.pfDamagePopup, position + textOffset, Quaternion.identity);

            PopupMessage popupMessage = popupTransform.GetComponent<PopupMessage>();
            popupMessage.Setup(text, textColor, floatTimer);

            return popupMessage;
        }

        public void Setup(string text, Color textColor, float floatTimer = 0)
        {
            _textMesh.text = text;
            _textMesh.color = textColor;
            _textColor = textColor;
            _disappearTimer = DISAPPEAR_TIMER_MAX;
            _sortingOrder++;
            _textMesh.sortingOrder = _sortingOrder;
            _floatTimter = new(floatTimer);
            _floatTimter.OnTimerEnd += () => { _canShrink = true; };
            _moveVector = new Vector3(0, 1f) * 10f;
        }

        private void Update()
        {
            transform.position += _moveVector * Time.deltaTime;
            _moveVector -= _moveVector * 6f * Time.deltaTime;
            _floatTimter.Tick(Time.deltaTime);

            if (_disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f)
            {
                float increaseScaleAmount = 1.25f;
                transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
            }
            else
            {
                if (!_canShrink) 
                    return;

                float decreaseScaleAmount = 1.25f;
                transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
            }

            _disappearTimer -= Time.deltaTime;
            if (_disappearTimer < 0)
            {
                float disappearSpeed = 3f;
                _textColor.a -= disappearSpeed * Time.deltaTime;
                //_textMesh.color = _textColor;

                if (_textColor.a < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}

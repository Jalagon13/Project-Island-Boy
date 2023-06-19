using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class DamagePopup : MonoBehaviour
    {
        private static int _sortingOrder;
        private const float DISAPPEAR_TIMER_MAX = 0.5f;

        private TextMeshPro _textMesh;
        private float _disappearTimer;
        private Color _textColor;
        private Vector3 _moveVector;

        private void Awake()
        {
            _textMesh = GetComponent<TextMeshPro>();
        }

        public static DamagePopup Create(Vector2 position, int damageAmount, float textoffset = 0f)
        {
            Transform damagePopupTransform = Instantiate(GameAssets.I.pfDamagePopup, position + new Vector2(0, textoffset), Quaternion.identity);

            DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
            damagePopup.Setup(damageAmount);

            return damagePopup;
        }

        public void Setup(int damageAmount)
        {
            _textMesh.text = damageAmount.ToString();
            _textColor = _textMesh.color;
            _disappearTimer = DISAPPEAR_TIMER_MAX;
            _sortingOrder++;
            _textMesh.sortingOrder = _sortingOrder;
            _moveVector = new Vector3(0, 1f) * 10f;
        }

        private void Update()
        {
            transform.position += _moveVector * Time.deltaTime;
            _moveVector -= _moveVector * 6f * Time.deltaTime;

            if (_disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f)
            {
                float increaseScaleAmount = 1.25f;
                transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
            }
            else
            {
                float decreaseScaleAmount = 1.25f;
                transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
            }

            _disappearTimer -= Time.deltaTime;
            if (_disappearTimer < 0)
            {
                float disappearSpeed = 3f;
                _textColor.a -= disappearSpeed * Time.deltaTime;
                _textMesh.color = _textColor;

                if (_textColor.a < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}

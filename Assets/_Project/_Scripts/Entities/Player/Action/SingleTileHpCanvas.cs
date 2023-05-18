using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class SingleTileHpCanvas : MonoBehaviour
    {
        private RectTransform _hpCanvas;
        private Image _fillImage;

        private void Awake()
        {
            _hpCanvas = transform.GetChild(1).GetComponent<RectTransform>();
            _fillImage = _hpCanvas.GetChild(0).GetChild(0).GetComponent<Image>();
        }

        private void Start()
        {
            HideHpCanvas();
        }

        public void ShowHpCanvas(float maxHp, float currentHp)
        {
            _hpCanvas.gameObject.SetActive(true);
            _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, maxHp, currentHp));
        }

        public void HideHpCanvas()
        {
            _hpCanvas.gameObject.SetActive(false);
        }
    }
}

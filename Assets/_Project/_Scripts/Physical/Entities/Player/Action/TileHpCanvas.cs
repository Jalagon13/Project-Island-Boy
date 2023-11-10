using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class TileHpCanvas : MonoBehaviour
    {
        private RectTransform _holder;
        private Image _fillImage;
        private List<IBreakable> _breakables;
        private TextMeshProUGUI _lifeText;
        private Vector2 _currentCenterPos;
        private bool _overInteractable;

        private void Awake()
        {
            _lifeText = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
            _holder = transform.GetChild(0).GetComponent<RectTransform>();
            _fillImage = _holder.GetChild(1).GetComponent<Image>();

            GameSignals.TILE_ACTION_ENTERED_NEW_TILE.AddListener(UpdateCanvas);
            GameSignals.RSC_HIT.AddListener(ClickPerformed);
        }

        private void OnDestroy()
        {
            GameSignals.TILE_ACTION_ENTERED_NEW_TILE.RemoveListener(UpdateCanvas);
            GameSignals.RSC_HIT.RemoveListener(ClickPerformed);
        }

        private void Start()
        {
            DisableHolder();
        }

        private void ClickPerformed(ISignalParameters parameters)
        {
            DisableHolder();

            if (_breakables == null) return;
            if (_breakables.Count <= 0)
            {
                DisableHolder();
                return;
            }

            UpdateBreakable(true);
        }

        private void UpdateCanvas(ISignalParameters parameter)
        {
            DisableHolder();

            if (parameter.HasParameter("Breakables") && parameter.HasParameter("CurrentCenterPos") && parameter.HasParameter("OverInteractable"))
            {
                _breakables = (List<IBreakable>)parameter.GetParameter("Breakables");
                _currentCenterPos = (Vector2)parameter.GetParameter("CurrentCenterPos");
                _overInteractable = (bool)parameter.GetParameter("OverInteractable");
                UpdateBreakable(false);
            }
        }

        private void UpdateBreakable(bool fromClick)
        {
            transform.position = _currentCenterPos;

            foreach (var breakable in _breakables)
            {
                if(breakable.CurrentHitPoints <= 0)
                {
                    DisableHolder();
                    return;
                }

                EnableHolder(breakable.MaxHitPoints, breakable.CurrentHitPoints, fromClick, _overInteractable);
                _lifeText.text = breakable.CurrentHitPoints.ToString();
            }
        }

        private void EnableHolder(float maxHp, float currentHp, bool fromClick, bool onlyShowOutline)
        {
            _holder.gameObject.SetActive(true);

            _holder.transform.GetChild(0).gameObject.SetActive(!onlyShowOutline);
            _holder.transform.GetChild(1).gameObject.SetActive(!onlyShowOutline);
            _holder.transform.GetChild(2).gameObject.SetActive(!onlyShowOutline);
            _holder.transform.GetChild(3).gameObject.SetActive(onlyShowOutline ? false : !fromClick);
            _holder.transform.GetChild(4).gameObject.SetActive(true);

            _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, maxHp, currentHp));
        }

        private void DisableHolder()
        {
            _holder.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class TooltipManager : Singleton<TooltipManager>
    {
        private Tooltip _tooltip;

        protected override void Awake()
        {
            base.Awake();
            _tooltip = transform.GetChild(0).GetComponent<Tooltip>();

            GameSignals.INVENTORY_CLOSE.AddListener(CloseToolTip);
        }

        private void OnDestroy()
        {
            GameSignals.INVENTORY_CLOSE.RemoveListener(CloseToolTip);
        }

        private void Start()
        {
            Hide();
        }

        private void CloseToolTip(ISignalParameters parameters)
        {
            Hide();
        }

        public void Show(string content, string header = "", Vector2 pivot = default)
        {
            if(pivot == default)
            {
                pivot = new Vector2(-0.1f, -0.1f);
            }
            _tooltip.SetPivot(pivot);
            _tooltip.SetText(content, header);
            _tooltip.gameObject.SetActive(true);

            if (string.IsNullOrEmpty(header))
                Hide();
        }

        public void Hide()
        {
            _tooltip.gameObject.SetActive(false);
        }
    }
}

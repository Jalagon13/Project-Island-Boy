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
        }

        private void Start()
        {
            Hide();
        }

        public void Show(string content, string header = "")
        {
            Instance._tooltip.SetText(content, header);
            Instance._tooltip.gameObject.SetActive(true);

            if (string.IsNullOrEmpty(header))
                Hide();
        }

        public void Hide()
        {
            Instance._tooltip.gameObject.SetActive(false);
        }
    }
}

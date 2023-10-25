using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class TabControl : MonoBehaviour
    {
        private RectTransform _craftTab;
        private RectTransform _houseTab;

        private void Awake()
        {
            _craftTab = transform.GetChild(0).GetComponent<RectTransform>();
            _houseTab = transform.GetChild(1).GetComponent<RectTransform>();
        }

        public void DisableAllTabs()
        {
            _craftTab.gameObject.SetActive(false);
            _houseTab.gameObject.SetActive(false);
        }

        public void OpenCraftTab()
        {
            _craftTab.gameObject.SetActive(true);
            _houseTab.gameObject.SetActive(false);
        }

        public void OpenHouseTab()
        {
            _houseTab.gameObject.SetActive(true);
            _craftTab.gameObject.SetActive(false);
        }
    }
}

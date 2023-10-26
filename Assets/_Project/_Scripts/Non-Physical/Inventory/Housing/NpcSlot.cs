using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IslandBoy
{
    public class NpcSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _rscSlotPrefab;

        private RectTransform _furniturePanel;
        private RectTransform _rscSlots;

        private void Awake()
        {
            _furniturePanel = transform.GetChild(1).GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _furniturePanel.gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _furniturePanel.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _furniturePanel.gameObject.SetActive(false);
        }

        public void Initialize(NpcObject npc)
        {
            _rscSlots = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
            var hover = transform.GetChild(0).GetComponent<HousingHoverImage>();
            var image = transform.GetChild(0).GetComponent<Image>();
            var description = $"{npc.Description}<br>Status: Not moved in";

            hover.Initialize(npc.Name, description);
            image.sprite = npc.Icon;

            InitializeResourceSlots(npc);
        }

        private void InitializeResourceSlots(NpcObject npc)
        {
            if (_rscSlots.transform.childCount > 0)
            {
                foreach (Transform child in _rscSlots.transform)
                {
                    Destroy(child);
                }
            }

            npc.FurnitureCheckList.ForEach(deployObject =>
            {
                GameObject rs = Instantiate(_rscSlotPrefab, _rscSlots.transform);
                RscSlot rescSlot = rs.GetComponent<RscSlot>();
                rescSlot.Initialize(deployObject, 1);
            });
        }
    }
}

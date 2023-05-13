using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IslandBoy
{
    public class CraftSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _rscSlotPrefab;

        private Image _outputImage;
        private RectTransform _rscPanel;
        private RectTransform _rscSlots;

        private void OnEnable()
        {
            Inventory.AddItemEvent += CraftCanCheck;
        }

        private void OnDisable()
        {
            Inventory.AddItemEvent -= CraftCanCheck;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _rscPanel.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _rscPanel.gameObject.SetActive(false);
        }

        public void Initialize(Recipe recipe)
        {
            _outputImage = transform.GetChild(0).GetComponent<Image>();
            _rscPanel = transform.GetChild(1).GetComponent<RectTransform>();
            _rscSlots = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
            
            _outputImage.sprite = recipe.OutputItem.UiDisplay;

            if(_rscSlots.transform.childCount > 0)
            {
                foreach (Transform child in _rscSlots.transform)
                {
                    Destroy(child);
                }
            }

            foreach (ItemAmount ia in recipe.ResourceList)
            {
                GameObject rs = Instantiate(_rscSlotPrefab, _rscSlots.transform);
                RscSlot rescSlot = rs.GetComponent<RscSlot>();
                rescSlot.Initialize(ia);
            }
        }

        private void CraftCanCheck()
        {
            Debug.Log("Check");
        }
    }
}

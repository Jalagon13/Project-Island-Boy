using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IslandBoy
{
    public class CraftSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _rscSlotPrefab;
        [SerializeField] private Color _craftableColor;
        [SerializeField] private Color _unCraftableColor;

        private RectTransform _rscPanel;
        private RectTransform _rscSlots;
        private Image _craftSlotBackround;
        private Image _outputImage;
        private Recipe _recipe;
        private bool _canCraft;

        public bool CanCraft { get { return _canCraft; } }
        public Recipe Recipe { get { return _recipe; } }

        private void Awake()
        {
            _craftSlotBackround = GetComponent<Image>();
            _outputImage = transform.GetChild(0).GetComponent<Image>();
            _rscPanel = transform.GetChild(1).GetComponent<RectTransform>();
            _rscSlots = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
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
            _recipe = recipe;
            _outputImage.sprite = recipe.OutputItem.UiDisplay;

            Inventory.AddItemEvent += CheckIfCanCraft;
            DropPanel.OnDropEvent += CheckIfCanCraft;
            InventorySlot.SlotClickedEvent += CheckIfCanCraft;
            CheckIfCanCraft();

            if (_rscSlots.transform.childCount > 0)
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

        private void CheckIfCanCraft()
        {
            bool canCraft = false;

            foreach (ItemAmount ia in _recipe.ResourceList)
            {
                canCraft = _pr.Inventory.Contains(ia.Item, ia.Amount);

                if (!canCraft) break;
            }

            if (canCraft)
                SetCraftable();
            else
                SetUnCraftable();
        }

        private void SetCraftable()
        {
            _craftSlotBackround.color = _craftableColor;
            _outputImage.color = Color.white;
            _canCraft = true;
        }

        private void SetUnCraftable()
        {
            _craftSlotBackround.color = _unCraftableColor;
            _outputImage.color = Color.black;
            _canCraft = false;
        }
    }
}

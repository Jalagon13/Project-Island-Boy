using TMPro;
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
        private CraftSlotImageHover _hoverImage;
        private Image _craftSlotBackround;
        private Image _outputImage;
        private Recipe _recipe;
        private TextMeshProUGUI _amountText;
        private bool _canCraft;

        public bool CanCraft { get { return _canCraft; } }
        public Recipe Recipe { get { return _recipe; } }

        private void Awake()
        {
            _craftSlotBackround = GetComponent<Image>();
            _outputImage = transform.GetChild(0).GetComponent<Image>();
            _hoverImage = transform.GetChild(0).GetComponent<CraftSlotImageHover>();
            _rscPanel = transform.GetChild(1).GetComponent<RectTransform>();
            _rscSlots = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
            _amountText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        }

        private void OnDisable()
        {
            _rscPanel.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Inventory.AddItemEvent -= CheckIfCanCraft;
            DropPanel.OnDropEvent -= CheckIfCanCraft;
            InventorySlot.SlotClickedEvent -= CheckIfCanCraft;
            CraftSlotCraftControl.ItemCraftedEvent -= CheckIfCanCraft;
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
            _hoverImage.OutputItem = recipe.OutputItem;
            _amountText.text = recipe.OutputAmount == 1 ? string.Empty : recipe.OutputAmount.ToString();

            Inventory.AddItemEvent += CheckIfCanCraft;
            DropPanel.OnDropEvent += CheckIfCanCraft;
            InventorySlot.SlotClickedEvent += CheckIfCanCraft;
            CraftSlotCraftControl.ItemCraftedEvent += CheckIfCanCraft;

            InitializeResourceSlots();
            CheckIfCanCraft();

            _rscPanel.gameObject.SetActive(false);
        }

        private void InitializeResourceSlots()
        {
            if (_rscSlots.transform.childCount > 0)
            {
                foreach (Transform child in _rscSlots.transform)
                {
                    Destroy(child);
                }
            }

            foreach (ItemAmount ia in _recipe.ResourceList)
            {
                GameObject rs = Instantiate(_rscSlotPrefab, _rscSlots.transform);
                RscSlot rescSlot = rs.GetComponent<RscSlot>();
                rescSlot.Initialize(ia);
            }
        }

        public void CheckIfCanCraft()
        {
            bool canCraft = false;

            foreach (ItemAmount ia in _recipe.ResourceList)
            {
                canCraft = _pr.Inventory.Contains(ia.Item, ia.Amount);

                if (!canCraft) break;
            }

            if(_recipe.ResourceList.Count <= 0)
                canCraft = true;

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

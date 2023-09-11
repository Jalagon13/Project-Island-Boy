using System;
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

        private RectTransform _rscPanel;
        private RectTransform _rscSlots;
        private CraftSlotImageHover _hoverImage;
        private CraftSlotCraftControl _cscc;
        private Image _craftSlotBackround;
        private Image _outputImage;
        private Recipe _recipe;
        private TextMeshProUGUI _amountText;
        private bool _canCraft;

        public bool CanCraft { get { return _canCraft; } }
        public Recipe Recipe { get { return _recipe; } }

        private void Awake()
        {
            _cscc = GetComponent<CraftSlotCraftControl>();
        }

        private void OnDisable()
        {
            _rscPanel.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            //DropPanel.OnDropEvent -= CheckIfCanCraft;
            _pr.Inventory.OnItemAdded -= CheckIfCanCraft;
            _cscc.OnItemCrafted -= CheckIfCanCraft;
            foreach (Slot slot in _pr.Inventory.InventorySlots)
            {
                slot.OnSlotClicked -= CheckIfCanCraft;
            }
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
            _craftSlotBackround = GetComponent<Image>();
            _outputImage = transform.GetChild(0).GetComponent<Image>();
            _hoverImage = transform.GetChild(0).GetComponent<CraftSlotImageHover>();
            _rscPanel = transform.GetChild(1).GetComponent<RectTransform>();
            _rscSlots = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
            _amountText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            _recipe = recipe;
            _outputImage.sprite = recipe.OutputItem.UiDisplay;
            _hoverImage.SetItemDescription(recipe.OutputItem);
            _amountText.text = recipe.OutputAmount == 1 ? string.Empty : recipe.OutputAmount.ToString();

            //DropPanel.OnDropEvent += CheckIfCanCraft;
            _pr.Inventory.OnItemAdded += CheckIfCanCraft;
            _cscc.OnItemCrafted += CheckIfCanCraft;
            foreach (Slot slot in _pr.Inventory.InventorySlots)
            {
                slot.OnSlotClicked += CheckIfCanCraft;
            }

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

        public void CheckIfCanCraft(object obj = null, EventArgs args = null)
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
            _outputImage.color = Color.white;
            _canCraft = true;
        }

        private void SetUnCraftable()
        {
            _outputImage.color = new Color(0.25f, 0.25f, 0.25f, 1);
            _canCraft = false;
        }
    }
}

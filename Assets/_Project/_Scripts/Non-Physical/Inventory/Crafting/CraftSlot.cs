using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IslandBoy
{
    public class CraftSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _rscSlotPrefab;

        private RectTransform _rscPanel;
        private RectTransform _rscSlots;
        private CraftSlotImageHover _hoverImage;
        private Image _outputImage;
        private Recipe _recipe;
        private TextMeshProUGUI _amountText;
        private Inventory _playerInventory;
        private bool _canCraft;

        public bool CanCraft { get { return _canCraft; } }
        public Recipe Recipe { get { return _recipe; } }

        private void OnDisable()
        {
            _rscPanel.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GameSignals.ITEM_CRAFTED.RemoveListener(CheckIfCanCraft);
            GameSignals.ITEM_ADDED.RemoveListener(CheckIfCanCraft);
            GameSignals.SLOT_CLICKED.RemoveListener(CheckIfCanCraft);
            GameSignals.INVENTORY_OPEN.RemoveListener(CheckIfCanCraft);
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
            GameSignals.ITEM_CRAFTED.AddListener(CheckIfCanCraft);
            GameSignals.ITEM_ADDED.AddListener(CheckIfCanCraft);
            GameSignals.SLOT_CLICKED.AddListener(CheckIfCanCraft);
            GameSignals.INVENTORY_OPEN.AddListener(CheckIfCanCraft);

            SetGlobals(recipe);
            InitializeResourceSlots();

            _rscPanel.gameObject.SetActive(false);
        }

        private void SetGlobals(Recipe recipe)
        {
            _outputImage = transform.GetChild(0).GetComponent<Image>();
            _hoverImage = transform.GetChild(0).GetComponent<CraftSlotImageHover>();
            _rscPanel = transform.GetChild(1).GetComponent<RectTransform>();
            _rscSlots = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
            _amountText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            _playerInventory = transform.root.GetChild(0).GetComponent<Inventory>();
            _recipe = recipe;
            _outputImage.sprite = recipe.OutputItem.UiDisplay;
            _hoverImage.SetItemDescription(recipe.OutputItem);
            _amountText.text = recipe.OutputAmount == 1 ? string.Empty : recipe.OutputAmount.ToString();
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
                rescSlot.Initialize(ia.Item, ia.Amount);
            }
        }

        public void CheckIfCanCraft(ISignalParameters parameters)
        {
            bool canCraft = false;

            foreach (ItemAmount ia in _recipe.ResourceList)
            {
                canCraft = _playerInventory.Contains(ia.Item, ia.Amount);

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

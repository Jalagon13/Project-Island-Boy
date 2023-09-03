using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IslandBoy
{
    public class AugmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _rscSlotPrefab;

        private RectTransform _rscPanel;
        private RectTransform _rscSlots;
        private CraftSlotImageHover _hoverImage;
        private Image _craftSlotBackround;
        private Image _outputImage;
        private AugmentRecipe _ar;
        private TextMeshProUGUI _amountText;
        private bool _canCraft;

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
            _pr.LevelSystem.OnLevelChanged -= LevelSystem_OnLevelChanged;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _rscPanel.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _rscPanel.gameObject.SetActive(false);
        }

        public void TryToCraft()
        {
            Debug.Log("Craft Augment");
        }



        public void Initialize(AugmentRecipe ar)
        {
            _ar = ar;
            _outputImage.sprite = ar.RecipeSprite;
            _hoverImage.SetCustomDescription(ar.Description, ar.Name);
            _amountText.text = string.Empty;
            _pr.LevelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;

            InitializeResourceSlots();
            CheckIfCanCraft();

            _rscPanel.gameObject.SetActive(false);
        }

        private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
        {
            CheckIfCanCraft();
        }

        private void CheckIfCanCraft()
        {
            bool canCraft = _pr.LevelSystem.LevelNumber >= _ar.LevelsRequired;

            if (canCraft)
                SetCraftable();
            else
                SetUnCraftable();
        }

        private void SetCraftable()
        {
            //_craftSlotBackround.color = _craftableColor;
            _outputImage.color = Color.white;
            _canCraft = true;
        }

        private void SetUnCraftable()
        {
            //_craftSlotBackround.color = _unCraftableColor;
            _outputImage.color = new Color(0.25f, 0.25f, 0.25f, 1);
            _canCraft = false;
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

            GameObject rs = Instantiate(_rscSlotPrefab, _rscSlots.transform);
            RscSlot rescSlot = rs.GetComponent<RscSlot>();
            rescSlot.InitializeExpSlot(_ar);
        }
    }
}

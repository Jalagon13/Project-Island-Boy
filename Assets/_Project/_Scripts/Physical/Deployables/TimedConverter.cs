using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class TimedConverter : Interactable
    {
        [Header("Timed Converter Parameters")]
        [SerializeField] private float _craftTimerSec;
        [SerializeField] private CraftingDatabaseObject _cdb;
        [Header("Timed Converter References")]
        [SerializeField] private GameObject _tcSlotPrefab;
        [SerializeField] private RectTransform _slotHolder;
        [SerializeField] private RectTransform _progressInfo;
        [SerializeField] private TextMeshProUGUI _craftText;
        [SerializeField] private TCCraftingUI _craftingUI;
        [Header("Game feel Parameters")]
        [SerializeField] private MMF_Player _craftingOnGoingFeedback;
        [SerializeField] private MMF_Player _craftingDoneFeedback;

        private Canvas _tcCanvas;
        private bool _appQuitting;

        private CraftingRecipeObject _inProgressRecipe;
        private Timer _craftingTimer;
        private int _inProgressCount;
        private bool _craftingInProgress;

        public bool CraftingInProgress { get { return _craftingInProgress; } }

        protected override void Awake()
        {
            base.Awake();

            _tcCanvas = transform.GetChild(3).GetComponent<Canvas>();
            _craftingTimer = new(_craftTimerSec);

            GameSignals.INVENTORY_CLOSE.AddListener(CloseUI);
        }

        private void OnDestroy()
        {
            GameSignals.INVENTORY_CLOSE.RemoveListener(CloseUI);

            if (_appQuitting) return;

            for (int i = 0; i < _inProgressCount; i++)
            {
                foreach (ItemAmount ia in _inProgressRecipe.ResourceList)
                {
                    GameAssets.Instance.SpawnItem(transform.position + new Vector3(0.5f, 0.5f, 0f), ia.Item, ia.Amount);
                }
            }

            ResetProcess();
        }

        private void OnApplicationQuit()
        {
            _appQuitting = true;
        }

        public override IEnumerator Start()
        {
            OnPlayerExitRange += () => DisableUI();
            DisableUI();

            return base.Start();
        }

        protected override void Update()
        {
            base.Update();

            _craftingTimer.Tick(Time.deltaTime);

            if(_craftingInProgress)
                _craftText.text = $"Crafting: {_inProgressRecipe.OutputItem.Name} [{_inProgressCount}]<br>{Math.Round(_craftingTimer.RemainingSeconds, 1)} sec";
        }

        public override void Interact()
        {
            if (!_canInteract || _tcCanvas.gameObject.activeInHierarchy || Pointer.IsOverUI()) return;

            EnableUI();
            ResetCraftSlots();
            SetUpRecipes();
            EnableInstructions(false);
        }

        public void RefreshCraftingUI(CraftingRecipeObject recipe)
        {
            _craftingUI.PopulateRecipe(recipe);
        }

        public void StartCrafting(CraftingRecipeObject incomingRecipe, int amount)
        {
            if (_craftingInProgress)
            {
                if (incomingRecipe.OutputItem.Name == _inProgressRecipe.OutputItem.Name)
                {
                    AddToCraftQueue(amount);
                }
                else
                {
                    EmptyCraftQueue();
                    OverrideCraftingInProgress(incomingRecipe);
                    AddToCraftQueue(amount);
                }
            }
            else
            {
                OverrideCraftingInProgress(incomingRecipe);
                AddToCraftQueue(amount);
            }
        }

        private void OverrideCraftingInProgress(CraftingRecipeObject incomingRecipe)
        {
            _inProgressRecipe = incomingRecipe;
            _craftingOnGoingFeedback?.PlayFeedbacks();
            _craftingTimer.RemainingSeconds = incomingRecipe.CraftingTimer == 0 ? 2f : incomingRecipe.CraftingTimer;
            _craftingTimer.OnTimerEnd -= CraftItem;
            _craftingTimer.OnTimerEnd += CraftItem;
        }

        public void EmptyCraftQueue()
        {
            for (int i = 0; i < _inProgressCount; i++)
            {
                foreach (ItemAmount ia in _inProgressRecipe.ResourceList)
                {
                    _pr.Inventory.AddItem(ia.Item, ia.Amount);
                }
            }

            ResetProcess();
        }

        private void AddToCraftQueue(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _inProgressCount++;
                _craftingInProgress = true;
                _progressInfo.gameObject.SetActive(true);

                foreach (ItemAmount ia in _inProgressRecipe.ResourceList)
                {
                    _pr.Inventory.RemoveItem(ia.Item, ia.Amount);
                }
            }

            GameSignals.ITEM_CRAFTED.Dispatch();
        }

        private void CraftItem()
        {
            Vector3 offset = transform.position + new Vector3(0.5f, 0f, 0f);
            PopupMessage.Create(offset, $"+{_inProgressRecipe.OutputAmount} {_inProgressRecipe.OutputItem.Name}", Color.green, default, 1f);
            GameAssets.Instance.SpawnItem(offset, _inProgressRecipe.OutputItem, _inProgressRecipe.OutputAmount);
            _craftingDoneFeedback?.PlayFeedbacks();

            _inProgressCount--;

            if (_inProgressCount > 0)
            {
                _craftingTimer.RemainingSeconds = _craftTimerSec;
                _craftingInProgress = true;
            }
            else
            {
                ResetProcess();
            }
        }

        private void ResetProcess()
        {
            _inProgressCount = 0;
            _inProgressRecipe = null;
            _craftingInProgress = false;
            _craftingTimer.OnTimerEnd -= CraftItem;
            _craftingOnGoingFeedback?.StopFeedbacks();
            _craftingDoneFeedback?.StopFeedbacks();
            _progressInfo.gameObject.SetActive(false);
            _craftingUI.ResetCraftingUI();

            transform.GetChild(0).localScale = Vector3.one;
        }

        private void SetUpRecipes()
        {
            if (_cdb == null) return;

            for (int i = 0; i < _cdb.Database.Length; i++)
            {
                GameObject cs = Instantiate(_tcSlotPrefab, _slotHolder.transform);
                TCSlot tcSlot = cs.GetComponent<TCSlot>();
                tcSlot.Initialize(_cdb.Database[i]);
            }
        }

        private void ResetCraftSlots()
        {
            if (_slotHolder.transform.childCount > 0)
            {
                foreach (Transform child in _slotHolder.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        private void DispatchTcSignal()
        {
            Signal signal = GameSignals.TIMED_CONVERTER_INTERACT;
            signal.ClearParameters();
            signal.AddParameter("TimedConverter", this);
            signal.Dispatch();
        }

        private void CloseUI(ISignalParameters parameters)
        {
            DisableUI();
        }

        public void EnableUI()
        {
            DispatchTcSignal();

            _tcCanvas.gameObject.SetActive(true);
            _progressInfo.gameObject.SetActive(_craftingInProgress);
        }

        public void DisableUI()
        {
            _tcCanvas.gameObject.SetActive(false);
        }

        public override void ShowDisplay()
        {
            EnableYellowCorners(true);
            EnableInstructions(true);
        }

        public override void HideDisplay()
        {
            EnableProgressBar(false);
            EnableAmountDisplay(false);
            EnableInstructions(false);
            EnableYellowCorners(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class TimedConverter : Interactable
    {
        [Header("Timed Converter Parameters")]
        [SerializeField] private float _craftTimerSec;
        [SerializeField] private CraftingDatabaseObject _cdb;
        [SerializeField] private GameObject _tcSlotPrefab;
        [SerializeField] private RectTransform _slotHolder;
        [SerializeField] private RectTransform _progressInfo;

        private Canvas _tcCanvas;
        private bool _appQuitting;

        private CraftingRecipeObject _inProgressRecipe;
        private Timer _craftingTimer;
        private int _inProgressCount;
        private bool _craftingInProgress;

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

            _craftingTimer.OnTimerEnd -= CraftItem;
            EnableUI(false);
        }

        private void OnApplicationQuit()
        {
            _appQuitting = true;
        }

        public override IEnumerator Start()
        {
            OnPlayerExitRange += () => EnableUI(false);
            EnableUI(false);

            return base.Start();
        }

        protected override void Update()
        {
            base.Update();

            _craftingTimer.Tick(Time.deltaTime);
        }

        public override void Interact()
        {
            if (!_canInteract) return;

            EnableUI(true);
            ResetCraftSlots();
            SetUpRecipes();
            DispatchTcSignal();
            EnableInstructions(false);
        }

        public void TryToStartCraftingProcess(CraftingRecipeObject recipe)
        {
            Debug.Log($"Timed Conversion started for: {recipe.OutputItem}");

            // if conversion in progress
            //  if current output is the same as this recipe output,
            //      add another stack
            //      and take away items from player
            //  if current output is different as this recipe output,
            //      give back player's items,
            //      override recipe with new recipe
            //      take items from player for new recipe
            // if conversion is NOT in progress
            //  take items from player for new recipe
            //  set current recipe to new recipe

            StartCrafting(recipe);
        }

        private void StartCrafting(CraftingRecipeObject recipe)
        {
            _inProgressRecipe = recipe;
            _inProgressCount++;
            _craftingInProgress = true;
            _craftingTimer.RemainingSeconds = _craftTimerSec;
            _craftingTimer.OnTimerEnd += CraftItem;

            foreach (ItemAmount ia in _inProgressRecipe.ResourceList)
            {
                _pr.Inventory.RemoveItem(ia.Item, ia.Amount);
            }
        }

        private void CraftItem()
        {
            Debug.Log("Spawn Item here!");
            _inProgressCount--;
            _craftingInProgress = _inProgressCount > 0;
        }

        private void SetUpRecipes()
        {
            if (_cdb == null) return;

            for (int i = 0; i < _cdb.Database.Length; i++)
            {
                GameObject cs = Instantiate(_tcSlotPrefab, _slotHolder.transform);

                CraftSlot craftSlot = cs.GetComponent<CraftSlot>();
                craftSlot.Initialize(_cdb.Database[i]);
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
            EnableUI(false);
        }

        public void EnableUI(bool val)
        {
            _tcCanvas.gameObject.SetActive(val);
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

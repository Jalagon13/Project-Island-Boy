using UnityEngine;

namespace IslandBoy
{
	public class CraftSlotsControl : MonoBehaviour
	{
		[SerializeField] private CraftingDatabaseObject _defaultCDB;
		[SerializeField] private GameObject _craftSlotPrefab;
		[SerializeField] private RectTransform _craftSlotsRect;
		[SerializeField] private TCCraftingUI _craftingUI;

		private MouseSlot _mouseItemHolder;

		private void Awake()
		{
			_mouseItemHolder = transform.GetChild(3).GetChild(0).GetComponent<MouseSlot>();
			GameSignals.INVENTORY_CLOSE.AddListener(RefreshCraftSlotsToDefault);
		}
		
		private void OnDestroy() 
		{
			GameSignals.INVENTORY_CLOSE.RemoveListener(RefreshCraftSlotsToDefault);
		}
		
		private void Start() 
		{
			RefreshCraftingMenu(_defaultCDB);
		}

		public void RefreshCraftingMenu(CraftingDatabaseObject cdb)
		{
			ResetCraftSlots();
			SetUpCDB(cdb);
		}
		
		private void SetUpCDB(CraftingDatabaseObject cdb)
		{
			if (cdb == null) return;

			for (int i = 0; i < cdb.Database.Length; i++)
			{
				GameObject cs = Instantiate(_craftSlotPrefab, _craftSlotsRect.transform);

				TCSlot tcSlot = cs.GetComponent<TCSlot>();
				tcSlot.Initialize(cdb.Database[i], RefreshCraftingUI);
				
				// CraftSlot craftSlot = cs.GetComponent<CraftSlot>();
				// craftSlot.Initialize(cdb.Database[i]);

				// CsSlotCraft craftSlotCraftControl = cs.GetComponent<CsSlotCraft>();
				// craftSlotCraftControl.MouseItemHolder = _mouseItemHolder;
			}
		}
		
		public void RefreshCraftingUI(CraftingRecipeObject recipe)
		{
			_craftingUI.PopulateRecipe(recipe);
		}

		private void ResetCraftSlots()
		{
			if (_craftSlotsRect.transform.childCount > 0)
			{
				foreach (Transform child in _craftSlotsRect.transform)
				{
					Destroy(child.gameObject);
				}
			}
		}
		
		public void RefreshCraftSlotsToDefault(ISignalParameters parameters)
		{
			RefreshCraftingMenu(_defaultCDB);
		}
	}
}

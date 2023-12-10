using UnityEngine;

namespace IslandBoy
{
    public class CraftSlotsControl : MonoBehaviour
    {

        [SerializeField] private GameObject _craftSlotPrefab;
        [SerializeField] private RectTransform _craftSlotsRect;

        private MouseSlot _mouseItemHolder;

        private void Awake()
        {
            _mouseItemHolder = transform.GetChild(3).GetChild(0).GetComponent<MouseSlot>();
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

                CraftSlot craftSlot = cs.GetComponent<CraftSlot>();
                craftSlot.Initialize(cdb.Database[i]);

                CsSlotCraft craftSlotCraftControl = cs.GetComponent<CsSlotCraft>();
                craftSlotCraftControl.MouseItemHolder = _mouseItemHolder;
            }
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
    }
}

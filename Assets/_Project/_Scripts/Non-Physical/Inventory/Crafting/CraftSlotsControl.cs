using UnityEngine;

namespace IslandBoy
{
    public class CraftSlotsControl : MonoBehaviour
    {

        [SerializeField] private GameObject _craftSlotPrefab;
        [SerializeField] private RectTransform _craftSlotsRect;

        private MouseItemHolder _mouseItemHolder;

        private void Awake()
        {
            _mouseItemHolder = transform.GetChild(3).GetComponent<MouseItemHolder>();
        }

        public void RefreshCraftingMenu(RecipeDatabaseObject rdb)
        {
            ResetCraftSlots();
            SetupRDB(rdb);
        }

        private void SetupRDB(RecipeDatabaseObject rdb)
        {
            if (rdb == null) return;

            for (int i = 0; i < rdb.Database.Length; i++)
            {
                GameObject cs = Instantiate(_craftSlotPrefab, _craftSlotsRect.transform);

                CraftSlot craftSlot = cs.GetComponent<CraftSlot>();
                craftSlot.Initialize(rdb.Database[i]);

                CraftSlotCraftControl craftSlotCraftControl = cs.GetComponent<CraftSlotCraftControl>();
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

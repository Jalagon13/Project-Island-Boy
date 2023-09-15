using UnityEngine;

namespace IslandBoy
{
    public class CraftSlotsControl : MonoBehaviour
    {

        [SerializeField] private GameObject _craftSlotPrefab;
        [SerializeField] private GameObject _augmentSlotPrefab;
        [SerializeField] private RectTransform _craftSlotsRect;

        private MouseItemHolder _mouseItemHolder;

        private void Awake()
        {
            _mouseItemHolder = transform.GetChild(2).GetComponent<MouseItemHolder>();
        }

        public void RefreshCraftingMenu(RecipeDatabaseObject rdb, RuneDatabaseObject adb)
        {
            ResetCraftSlots();

            if (rdb != null)
                SetupRDB(rdb);

            if (adb != null)
                SetupADB(adb);
        }

        private void SetupADB(RuneDatabaseObject adb)
        {
            for (int i = 0; i < adb.Database.Length; i++)
            {
                GameObject augSlotGo = Instantiate(_augmentSlotPrefab, _craftSlotsRect.transform);

                RuneSlot augSlot = augSlotGo.GetComponent<RuneSlot>();
                augSlot.Initialize(adb.Database[i]);
                augSlot.MouseItemHolder = _mouseItemHolder;
            }
        }

        private void SetupRDB(RecipeDatabaseObject rdb)
        {
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

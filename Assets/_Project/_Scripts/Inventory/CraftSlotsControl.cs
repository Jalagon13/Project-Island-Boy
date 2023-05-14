using UnityEngine;

namespace IslandBoy
{
    public class CraftSlotsControl : MonoBehaviour
    {
        [SerializeField] private RecipeDatabaseObject _rdb;
        [SerializeField] private GameObject _craftSlotPrefab;
        [SerializeField] private RectTransform _craftSlotsRect;

        private void Start()
        {
            RefreshCraftingMenu();
        }

        private void RefreshCraftingMenu()
        {
            if (_craftSlotsRect.transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    Destroy(child);
                }
            }

            for (int i = 0; i < _rdb.Database.Length; i++)
            {
                GameObject cs = Instantiate(_craftSlotPrefab, _craftSlotsRect.transform);
                CraftSlot craftSlot = cs.GetComponent<CraftSlot>();
                craftSlot.Initialize(_rdb.Database[i]);
            }
        }
    }
}

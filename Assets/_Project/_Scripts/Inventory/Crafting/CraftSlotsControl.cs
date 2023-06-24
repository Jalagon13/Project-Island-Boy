using UnityEngine;

namespace IslandBoy
{
    public class CraftSlotsControl : MonoBehaviour
    {
        [SerializeField] private RecipeDatabaseObject _defaultRdb;
        [SerializeField] private GameObject _craftSlotPrefab;
        [SerializeField] private RectTransform _craftSlotsRect;

        private MouseItemHolder _mouseItemHolder;

        public RecipeDatabaseObject DefaultRdb { get { return _defaultRdb; } }

        private void Awake()
        {
            _mouseItemHolder = transform.GetChild(2).GetComponent<MouseItemHolder>();
        }

        public void RefreshCraftingMenu(RecipeDatabaseObject rdb)
        {
            if (_craftSlotsRect.transform.childCount > 0)
            {
                foreach (Transform child in _craftSlotsRect.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            for (int i = 0; i < rdb.Database.Length; i++)
            {
                GameObject cs = Instantiate(_craftSlotPrefab, _craftSlotsRect.transform);

                CraftSlot craftSlot = cs.GetComponent<CraftSlot>();
                craftSlot.Initialize(rdb.Database[i]);

                CraftSlotCraftControl craftSlotCraftControl = cs.GetComponent<CraftSlotCraftControl>();
                craftSlotCraftControl.MouseItemHolder = _mouseItemHolder;
            }
        }
    }
}

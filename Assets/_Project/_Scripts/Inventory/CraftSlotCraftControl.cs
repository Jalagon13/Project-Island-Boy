using UnityEngine;

namespace IslandBoy
{
    public class CraftSlotCraftControl : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private AudioClip _popSound;
        [SerializeField] private GameObject _inventoryItemPrefab;

        private MouseItemHolder _mouseItemHolder;
        private CraftSlot _cs;

        public MouseItemHolder MouseItemHolder { set { _mouseItemHolder = value; } }

        private void Awake()
        {
            _cs = GetComponent<CraftSlot>();
        }

        public void TryToCraft() // connected to slot button
        {
            if (!_cs.CanCraft) return;
            if (_mouseItemHolder.HasItem()) return;

            _mouseItemHolder.CreateMouseItem(_inventoryItemPrefab, _cs.Recipe.OutputItem, _cs.Recipe.OutputAmount);

            foreach (ItemAmount ia in _cs.Recipe.ResourceList)
            {
                _pr.Inventory.RemoveItem(ia.Item, ia.Amount);
            }

            AudioManager.Instance.PlayClip(_popSound, false, true);

            _cs.CheckIfCanCraft();
        }
    }
}

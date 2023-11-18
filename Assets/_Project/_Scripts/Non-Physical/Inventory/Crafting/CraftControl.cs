using System;
using System.Collections;
using UnityEngine;

namespace IslandBoy
{
    public class CraftControl : MonoBehaviour
    {
        [SerializeField] private AudioClip _popSound;
        [SerializeField] private GameObject _inventoryItemPrefab;

        private MouseItemHolder _mouseItemHolder;
        private Inventory _playerInventory;
        private CraftSlot _cs;

        public MouseItemHolder MouseItemHolder { set { _mouseItemHolder = value; } }

        private void Awake()
        {
            _cs = GetComponent<CraftSlot>();
            _playerInventory = transform.root.GetChild(0).GetComponent<Inventory>();
        }

        public void TryToCraft() // connected to slot button
        {
            if (!_cs.CanCraft) return;
            if (!_mouseItemHolder.TryToCraftItem(_inventoryItemPrefab, _cs.Recipe.OutputItem, _cs.Recipe.OutputAmount)) return;

            foreach (ItemAmount ia in _cs.Recipe.ResourceList)
            {
                _playerInventory.RemoveItem(ia.Item, ia.Amount);
            }

            AudioManager.Instance.PlayClip(_popSound, false, true);
            GameSignals.ITEM_CRAFTED.Dispatch();
        }
    }
}

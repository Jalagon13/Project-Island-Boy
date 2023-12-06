using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class TcSlotCraft : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private AudioClip _popSound;
        [SerializeField] private GameObject _inventoryItemPrefab;

        private CraftSlot _cs;
        private TimedConverter _tc;

        private void Awake()
        {
            _cs = GetComponent<CraftSlot>();
            _tc = transform.root.GetComponent<TimedConverter>();
        }

        public void TryToCraft() // connected to slot button
        {
            if (!_cs.CanCraft)
            {
                PopupMessage.Create(_pr.Position, $"I'm missing resources to craft this.", Color.yellow, Vector2.up, 1f);
                return;
            }

            
            _tc.TryToStartCraftingProcess(_cs.Recipe);

            //if (!_mouseItemHolder.TryToCraftItem(_inventoryItemPrefab, _cs.Recipe.OutputItem, _cs.Recipe.OutputAmount)) return;

            //foreach (ItemAmount ia in _cs.Recipe.ResourceList)
            //{
            //    _playerInventory.RemoveItem(ia.Item, ia.Amount);
            //}

            //MMSoundManagerSoundPlayEvent.Trigger(_popSound, MMSoundManager.MMSoundManagerTracks.UI, transform.position);
            //GameSignals.ITEM_CRAFTED.Dispatch();
        }
    }
}

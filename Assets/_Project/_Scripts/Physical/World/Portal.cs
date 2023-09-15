using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private PortalSlot _portalSlotPrefab;
        [SerializeField] private RectTransform _reqHolderRect;
        [SerializeField] private List<ItemAmount> _upgradeRequirements;

        private int _slotsComplete;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            PopulateSlots();
        }

        private void PopulateSlots()
        {
            foreach (ItemAmount ia in _upgradeRequirements)
            {
                PortalSlot ps = Instantiate(_portalSlotPrefab, _reqHolderRect.transform);
                ps.Initialize(ia.Item, ia.Amount);
                ps.OnSlotComplete += AddToSlotCompleteQuota;
            }
        }

        private void AddToSlotCompleteQuota(object sender, EventArgs e)
        {
            _slotsComplete++;

            if (_slotsComplete >= _upgradeRequirements.Count)
                CompletePortal();

            PortalSlot ps = sender as PortalSlot;
            ps.OnSlotComplete -= AddToSlotCompleteQuota;
        }

        private void CompletePortal()
        {
            // put portal complete logic here.
            Debug.Log("Portal coplete!");
        }
    }
}

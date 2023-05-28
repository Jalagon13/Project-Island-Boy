using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class LockedHatch : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private ItemObject _keyItem;
        [SerializeField] private GameObject _descendStairsPrefab;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right || !_pr.PlayerInRange(transform.position + new Vector3(0.5f, 0.5f))) return;
            if(_pr.SelectedSlot.ItemObject == null) return;

            if(_pr.SelectedSlot.ItemObject == _keyItem)
            {
                _pr.SelectedSlot.InventoryItem.Count--;
                Unlock();
            }

        }

        private void Unlock()
        {
            Instantiate(_descendStairsPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

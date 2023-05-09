using UnityEngine;

namespace IslandBoy
{
    public class MouseItemHolder : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _inventoryItemPrefab;

        public InventoryItem InventoryItem 
        { 
            get 
            { 
                return transform.GetChild(0).GetComponent<InventoryItem>();
            } 
        }

        private void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            transform.position = Camera.main.WorldToScreenPoint(_pr.MousePositionReference);
        }

        public void DeleteMouseItem()
        {
            if(transform.GetChild(0).gameObject != null)
                Destroy(transform.GetChild(0).gameObject);
        }

        public void CreateMouseItem(ItemObject item)
        {
            if (!HasItem())
            {
                GameObject newItemGo = Instantiate(_inventoryItemPrefab, transform);
                IInventoryItemInitializer inventoryItem = newItemGo.GetComponent<IInventoryItemInitializer>();
                inventoryItem.Initialize(item);
            }
        }

        public ItemObject MouseItemObject()
        {
            if (HasItem())
            {
                var inventoryItem = transform.GetChild(0);
                var item = inventoryItem.GetComponent<IInventoryItemInitializer>();

                return item.Item;
            }

            Debug.LogError($"MouseItemObject callback from [{name}]. Can not get Item in Mouse Holder because there is no item.");
            return null;
        }

        public bool HasItem()
        {
            return transform.childCount > 0;
        }

        public void GiveItemToSlot(Transform slot)
        {
            if (HasItem())
            {
                var item = transform.GetChild(0);
                item.SetParent(slot, false);
            }
        }
    }
}

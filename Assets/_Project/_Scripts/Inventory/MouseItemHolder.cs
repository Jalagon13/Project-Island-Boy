using UnityEngine;

namespace IslandBoy
{
    public class MouseItemHolder : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;

        public ItemObject ItemObject
        {
            get
            {
                if (HasItem())
                {
                    var inventoryItem = transform.GetChild(0);

                    InventoryItem item = inventoryItem.GetComponent<InventoryItem>();

                    return item.Item;
                }

                Debug.LogError($"MouseItemObject callback from [{name}]. Can not get Item in Mouse Holder because there is no item.");
                return null;
            }
        }

        public InventoryItem InventoryItem 
        { 
            get 
            { 
                return transform.GetChild(0).GetComponent<InventoryItem>();
            } 
        }

        public GameObject ItemGo 
        { 
            get 
            {
                GameObject item = transform.GetChild(0).gameObject;
                return item != null ? item : null;
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

        public void CreateMouseItem(GameObject itemGo, ItemObject itemObject, int count = 1)
        {
            if (!HasItem())
            {
                GameObject newItemGo = Instantiate(itemGo, transform);
                InventoryItem item = newItemGo.GetComponent<InventoryItem>();
                item.Initialize(itemObject, itemObject.DefaultParameterList, count);
            }
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

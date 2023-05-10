using UnityEngine;

namespace IslandBoy
{
    public class MouseItemHolder : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _invItemRscPrefab;
        [SerializeField] private GameObject _invItemToolPrefab;

        public InventoryItemRsc InventoryItem 
        { 
            get 
            { 
                return transform.GetChild(0).GetComponent<InventoryItemRsc>();
            } 
        }

        public GameObject MouseItemGo 
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

        public void CreateMouseItem(ItemObject item)
        {
            if (!HasItem())
            {
                GameObject newItemGo = Instantiate(item is ToolObject ? _invItemToolPrefab : _invItemRscPrefab, transform);

                if(newItemGo.TryGetComponent(out InventoryItemRsc rsc))
                {
                    rsc.Initialize((ResourceObject)item);
                }
                else if(newItemGo.TryGetComponent(out InventoryItemTool tool))
                {
                    tool.Initialize((ToolObject)item, 3);
                }
            }
        }

        public ItemObject MouseItemObject()
        {
            if (HasItem())
            {
                var inventoryItem = transform.GetChild(0);

                if (inventoryItem.TryGetComponent(out InventoryItemRsc rsc))
                {
                    return rsc.Item;
                }
                else if (inventoryItem.TryGetComponent(out InventoryItemTool tool))
                {
                    return tool.Item;
                }
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

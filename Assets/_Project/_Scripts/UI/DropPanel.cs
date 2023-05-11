using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class DropPanel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private MouseItemHolder _mouseItemHolder;
        [SerializeField] private SingleTileAction _sta;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Right && _mouseItemHolder.HasItem() && _sta.IsClear())
            {
                WorldItemManager.Instance.SpawnItem(_sta.gameObject.transform.position, _mouseItemHolder.MouseItemObject(), 
                    _mouseItemHolder.InventoryItem.Count, _mouseItemHolder.InventoryItem.CurrentParameters);

                Destroy(_mouseItemHolder.MouseItemGo);
            }
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class DropPanel : MonoBehaviour, IPointerClickHandler
    {
        //public static event Action OnDropEvent;

        [SerializeField] private MouseItemHolder _mih;
        [SerializeField] private CursorControl _sta;

        public void OnPointerClick(PointerEventData eventData)
        {
            //if(eventData.button == PointerEventData.InputButton.Right && _mih.HasItem() && _sta.IsClear())
            //{
            //    WorldItemManager.Instance.SpawnItem(_sta.gameObject.transform.position, _mih.ItemObject, 
            //        _mih.InventoryItem.Count, _mih.InventoryItem.CurrentParameters);

            //    OnDropEvent?.Invoke();
            //    _mih.DeleteMouseItem();
            //}
        }
    }
}

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
                // spawn item at _sta.gameObject.transform.position
                // maybe make IInventoryInitializer have a spawnItem function that returns an Item prefab and dynamically populates it?
                Debug.Log(_sta.gameObject.transform.position);
            }
        }
    }
}

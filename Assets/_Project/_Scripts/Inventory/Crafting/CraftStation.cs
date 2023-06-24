using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class CraftStation : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<RecipeDatabaseObject> OnCraftStationInteract;

        [SerializeField] private RecipeDatabaseObject _rdb;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("ASL:KJF");
                OnCraftStationInteract?.Invoke(_rdb);
            }
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class CraftStation : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<RecipeDatabaseObject, AugmentDatabaseObject> OnCraftStationInteract;

        [SerializeField] private PlayerReference _pr;
        [SerializeField] private RecipeDatabaseObject _rdb;
        [SerializeField] private AugmentDatabaseObject _adb;

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("THis working?");
            if(eventData.button == PointerEventData.InputButton.Right && _pr.PlayerInRange(transform.position))
            {
                Debug.Log("Inside it");
                OnCraftStationInteract?.Invoke(_rdb, _adb);
            }
        }
    }
}

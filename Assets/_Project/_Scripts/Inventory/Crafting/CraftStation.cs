using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class CraftStation : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private RecipeDatabaseObject _rdb;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("POG");
            }
        }
    }
}

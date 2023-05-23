using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class CaveDescend : MonoBehaviour, IPointerClickHandler
    {
        private int _levelIndexPointer = -10;

        private void Start()
        {
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_levelIndexPointer == -10)
            {
                // create new level
                // set _indexPointer to new level index
                // transition to cave level based on indexPointer
            }
            else
            {
                // transition to cave level based on _levelIndexPointer
            }
        }
    }
}

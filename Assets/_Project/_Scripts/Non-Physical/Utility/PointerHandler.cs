using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public static class PointerHandler
    {
        // gets data on raycast of all gameobject's layers and returns true if finds a gameobject of layerNum
        public static bool IsOverLayer(int layerNum)
        {
            PointerEventData eventDataCurrentPosition = new(EventSystem.current)
            {
                position = Mouse.current.position.ReadValue()
            };

            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            foreach (RaycastResult raycastResult in results)
            {
                if (raycastResult.gameObject.layer == layerNum)
                    return true;
            }

            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class NPCMiner : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private ItemObject _diamondItem;
        [SerializeField] private int _cost;

        public void EnterCave()
        {
            //if (!_pr.Inventory.Contains(_diamondItem, _cost)) return;
            //_pr.Inventory.RemoveItem(_diamondItem, _cost);
            //CursorManager.Instance.SetDefaultCursor();

            LevelManager.Instance.LoadUnderground();
        }
    }
}

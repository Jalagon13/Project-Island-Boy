using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class MinerEntity : Entity
    {
        public void EnterUnderground()
        {
            PR.Inventory.InventoryControl.CloseInventory();
            LevelManager.Instance.LoadUnderground();
        }
    }
}

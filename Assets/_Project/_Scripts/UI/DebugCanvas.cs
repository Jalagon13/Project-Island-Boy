using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class DebugCanvas : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;

        public void GimmeItem(ItemObject item)
        {
            WorldItemManager.Instance.SpawnItem(_pr.PositionReference, item);
        }
    }
}

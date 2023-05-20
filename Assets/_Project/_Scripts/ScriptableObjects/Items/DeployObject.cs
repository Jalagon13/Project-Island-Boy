using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class DeployObject : ItemObject
    {
        [SerializeField] private GameObject _prefabToDeploy;

        public override ToolType ToolType => ToolType.Ax;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            control.DeployPrefab(_prefabToDeploy);
        }
    }
}

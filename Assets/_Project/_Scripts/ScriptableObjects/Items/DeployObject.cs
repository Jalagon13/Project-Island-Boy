using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Deployable", menuName = "Create Item/New Deployable")]
    public class DeployObject : ItemObject
    {
        [SerializeField] private GameObject _prefabToDeploy;

        public override ToolType ToolType => ToolType.Ax;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            if(control.SingleTileAction.IsClear())
                control.SingleTileAction.PlaceDeployable(_prefabToDeploy);
        }
    }
}

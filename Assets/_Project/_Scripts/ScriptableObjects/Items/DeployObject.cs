using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Deployable", menuName = "Create Item/New Deployable")]
    public class DeployObject : ItemObject
    {
        [SerializeField] private GameObject _prefabToDeploy;
        [SerializeField] private AudioClip _deploySound;

        public override ToolType ToolType => ToolType.Ax;

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            if (control.SingleTileAction.IsClear())
            {
                AudioManager.Instance.PlayClip(_deploySound, false, true);
                control.PR.SelectedSlot.InventoryItem.Count--;
                control.SingleTileAction.PlaceDeployable(_prefabToDeploy);
            }
        }

        public override string GetDescription()
        {
            return $"- Can be placed<br>{Description}";
        }
    }
}

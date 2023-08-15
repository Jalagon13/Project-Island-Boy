using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Deployable", menuName = "Create Item/New Deployable")]
    public class DeployObject : ItemObject
    {
        [SerializeField] private GameObject _prefabToDeploy;
        [SerializeField] private RuleTile _exclusiveDeployTile;
        [SerializeField] private AudioClip _deploySound;
        [SerializeField] private Tilemap _test;

        public override ToolType ToolType => _baseToolType;

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            if (PointerHandler.IsOverLayer(5)) return;

            if(_exclusiveDeployTile != null)
            {
                var taPos = Vector3Int.FloorToInt(control.TileAction.gameObject.transform.position);

                if (control.TMR.GroundTilemap.GetTile(taPos) != _exclusiveDeployTile) return;
            }

            if (control.TileAction.IsClear() && !control.WallTilemap.HasTile(Vector3Int.FloorToInt(control.TileAction.gameObject.transform.position)))
            {
                AudioManager.Instance.PlayClip(_deploySound, false, true);
                control.PR.SelectedSlot.InventoryItem.Count--;
                control.TileAction.PlaceDeployable(_prefabToDeploy);
            }
        }

        public override string GetDescription()
        {
            return $"• Can be placed<br>{Description}";
        }
    }
}

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

        public override ToolType ToolType => _baseToolType;
        public override AmmoType AmmoType => _baseAmmoType;
        public override ArmorType ArmorType => _baseArmorType;
        public override GameObject AmmoPrefab => null;
        public override int ConsumeValue => 0;

        public override void ExecutePrimaryAction(SelectedSlotControl control)
        {
            if (PointerHandler.IsOverLayer(5)) return;

            if(_exclusiveDeployTile != null)
            {
                var taPos = Vector3Int.FloorToInt(control.TileAction.gameObject.transform.position);

                if (control.TMR.GroundTilemap.GetTile(taPos) != _exclusiveDeployTile) return;
            }

            bool wallTmHasTile = control.TMR.WallTilemap.HasTile(Vector3Int.FloorToInt(control.TileAction.gameObject.transform.position));
            bool groundTmHasTile = control.TMR.GroundTilemap.HasTile(Vector3Int.FloorToInt(control.TileAction.gameObject.transform.position));
            bool tilActionClear = control.TileAction.IsClear();

            if (tilActionClear && !wallTmHasTile && groundTmHasTile)
            {
                control.SelectedSlot.InventoryItem.Count--;
                AudioManager.Instance.PlayClip(_deploySound, false, true);
                ObjectPlacedDispatch();
            }
        }

        public override void ExecuteSecondaryAction(SelectedSlotControl control)
        {

        }

        private void ObjectPlacedDispatch()
        {
            Signal signal = GameSignals.OBJECT_PLACED;
            signal.ClearParameters();
            signal.AddParameter("ObjectPlaced", _prefabToDeploy);
            signal.Dispatch();
        }

        public override string GetDescription()
        {
            return $"{Description}<br>• Left Click to place";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New World Tile", menuName = "Create Item/New World Tile")]
    public class WorldTileObject : ItemObject
    {
        [SerializeField] private RuleTileExtended _worldTile;

        public override ToolType ToolType => _baseToolType;
        public override AmmoType AmmoType => _baseAmmoType;
        public override GameObject AmmoPrefab => null;
        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            var mousePos = Vector3Int.FloorToInt(control.TileAction.gameObject.transform.position);

            if (!control.TMR.GroundTilemap.HasTile(mousePos))
            {
                control.TMR.GroundTilemap.SetTile(mousePos, _worldTile);
                control.PR.SelectedSlot.InventoryItem.Count--;
                AudioManager.Instance.PlayClip(_worldTile.PlaceSound, false, true);
            }
        }

        public override string GetDescription()
        {
            return $"• World Tile, can extend island<br>{Description} ";
        }
    }
}

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

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            var mousePos = Vector3Int.FloorToInt(control.SingleTileAction.gameObject.transform.position);

            if (!control.IslandTilemap.HasTile(mousePos))
            {
                control.IslandTilemap.SetTile(mousePos, _worldTile);
                control.PR.SelectedSlot.InventoryItem.Count--;
                AudioManager.Instance.PlayClip(_worldTile.PlaceSound, false, true);
            }
        }

        public override string GetDescription()
        {
            return Description;
        }
    }
}
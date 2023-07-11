using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Wall", menuName = "Create Item/New Wall")]
    public class WallObject : ItemObject
    {
        [SerializeField] private RuleTileExtended _wallTile;

        public override ToolType ToolType => _baseToolType;

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            var pos = Vector3Int.FloorToInt(control.TileAction.gameObject.transform.position);

            if (!control.WallTilemap.HasTile(pos) && control.TileAction.IsClear() && control.IslandTilemap.HasTile(pos))
            {
                control.WallTilemap.SetTile(pos, _wallTile);
                control.PR.SelectedSlot.InventoryItem.Count--;
                AudioManager.Instance.PlayClip(_wallTile.PlaceSound, false, true);
            }
        }

        public override string GetDescription()
        {
            return Description;
        }
    }
}

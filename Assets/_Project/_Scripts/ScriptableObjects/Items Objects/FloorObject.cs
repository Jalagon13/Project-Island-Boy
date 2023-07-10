using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Floor", menuName = "Create Item/New Floor")]
    public class FloorObject : ItemObject
    {
        [SerializeField] private RuleTileExtended _floorTile;

        public override ToolType ToolType => _baseToolType;

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            var mousePos = Vector3Int.FloorToInt(control.TileAction.gameObject.transform.position);

            if (!control.WallTilemap.HasTile(mousePos) && !control.FloorTilemap.HasTile(mousePos))
            {
                control.FloorTilemap.SetTile(mousePos, _floorTile);
                control.PR.SelectedSlot.InventoryItem.Count--;
                AudioManager.Instance.PlayClip(_floorTile.PlaceSound, false, true);
            }
        }

        public override string GetDescription()
        {
            return Description;
        }
    }
}

using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Floor", menuName = "Create Item/New Floor")]
    public class FloorObject : ItemObject
    {
        [SerializeField] private RuleTileExtended _floorTile;

        public override ToolType ToolType => _baseToolType;
        public override AmmoType AmmoType => _baseAmmoType;
        public override ArmorType ArmorType => _baseArmorType;
        public override GameObject AmmoPrefab => null;
        public override int ConsumeValue => 0;

        public override void ExecutePrimaryAction(SelectedSlotControl control)
        {
            var pos = Vector3Int.FloorToInt(control.TileAction.gameObject.transform.position);

            if (!control.TMR.WallTilemap.HasTile(pos) && !control.TMR.FloorTilemap.HasTile(pos) && control.TMR.GroundTilemap.HasTile(pos))
            {
                control.TMR.FloorTilemap.SetTile(pos, _floorTile);
                control.SelectedSlot.InventoryItem.Count--;
                AudioManager.Instance.PlayClip(_floorTile.PlaceSound, false, true);
            }
        }

        public override void ExecuteSecondaryAction(SelectedSlotControl control)
        {

        }

        public override string GetDescription()
        {
            return Description;
        }
    }
}

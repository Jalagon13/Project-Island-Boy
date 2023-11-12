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

        }

        public override void ExecuteSecondaryAction(SelectedSlotControl control)
        {
            if (PointerHandler.IsOverLayer(5)) return;

            var pos = Vector3Int.FloorToInt(control.CursorControl.gameObject.transform.position);

            if (!control.TMR.WallTilemap.HasTile(pos) && !control.TMR.FloorTilemap.HasTile(pos) && control.TMR.GroundTilemap.HasTile(pos))
            {
                control.FocusSlot.InventoryItem.Count--;
                control.TMR.FloorTilemap.SetTile(pos, _floorTile);

                AudioManager.Instance.PlayClip(_floorTile.PlaceSound, false, true);
            }
        }

        public override string GetDescription()
        {
            float clickDistance = 0;

            foreach (var item in DefaultParameterList)
            {
                switch (item.Parameter.ParameterName)
                {
                    case "ClickDistance":
                        clickDistance = item.Value;
                        break;
                }
            }

            return $"• Right Click to place<br>• {clickDistance} build distance";
        }
    }
}

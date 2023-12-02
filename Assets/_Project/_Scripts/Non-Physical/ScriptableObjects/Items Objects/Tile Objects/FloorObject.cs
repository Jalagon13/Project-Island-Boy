using MoreMountains.Tools;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Floor", menuName = "Create Item/New Floor")]
    public class FloorObject : ItemObject
    {
        [SerializeField] private RuleTileExtended _floorTile;

        public override ToolType ToolType => _baseToolType;
        public override ArmorType ArmorType => _baseArmorType;

        public override void ExecutePrimaryAction(FocusSlotControl control)
        {
            var pos = Vector3Int.FloorToInt(control.CursorControl.gameObject.transform.position);

            if (!control.TMR.WallTilemap.HasTile(pos) && !control.TMR.FloorTilemap.HasTile(pos) && control.TMR.GroundTilemap.HasTile(pos))
            {
                control.FocusSlot.InventoryItem.Count--;
                control.TMR.FloorTilemap.SetTile(pos, _floorTile);
                GameSignals.ITEM_DEPLOYED.Dispatch();
                MMSoundManagerSoundPlayEvent.Trigger(_floorTile.PlaceSound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
            }
        }

        public override void ExecuteSecondaryAction(FocusSlotControl control)
        {

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

            return $"• Left Click to place<br>• {clickDistance} build distance";
        }
    }
}

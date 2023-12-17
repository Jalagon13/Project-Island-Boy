using MoreMountains.Tools;
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
        public override ArmorType ArmorType => _baseArmorType;

        public override void ExecutePrimaryAction(FocusSlotControl control)
        {
            var pos = Vector3Int.FloorToInt(control.CursorControl.gameObject.transform.position);

            if (!control.WallTm.Tilemap.HasTile(pos) && control.CursorControl.IsClear())
            {
                control.FocusSlot.InventoryItem.Count--;
                control.WallTm.Tilemap.SetTile(pos, _wallTile);

                GameSignals.ITEM_DEPLOYED.Dispatch();

                MMSoundManagerSoundPlayEvent.Trigger(_wallTile.PlaceSound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
                _wallTile.UpdatePathfinding(new(pos.x + 0.5f, pos.y + 0.5f));
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

            return $"• Left Click to place<br>• {clickDistance} build distance<br>{Description}";
        }
    }
}

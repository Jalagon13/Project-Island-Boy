using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Wall", menuName = "Create Item/New Wall")]
    public class WallObject : ItemObject
    {
        [SerializeField] private RuleTile _wallTile;
        [SerializeField] private AudioClip _placeSound;

        public override ToolType ToolType => _baseToolType;

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            var mousePos = Vector3Int.FloorToInt(control.PR.MousePositionReference);

            if (!control.WallTilemap.HasTile(mousePos))
            {
                control.WallTilemap.SetTile(mousePos, _wallTile);
                control.PR.SelectedSlot.InventoryItem.Count--;
                AudioManager.Instance.PlayClip(_placeSound, false, true);
            }
        }

        public override string GetDescription()
        {
            return Description;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Floor", menuName = "Create Item/New Floor")]
    public class FloorObject : ItemObject
    {
        [SerializeField] private TileBase _floorTile;
        [SerializeField] private AudioClip _placeSound;

        public override ToolType ToolType => _baseToolType;

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            var mousePos = Vector3Int.FloorToInt(control.PR.MousePositionReference);

            if (!control.WallTilemap.HasTile(mousePos) && !control.FloorTilemap.HasTile(mousePos))
            {
                control.FloorTilemap.SetTile(mousePos, _floorTile);
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

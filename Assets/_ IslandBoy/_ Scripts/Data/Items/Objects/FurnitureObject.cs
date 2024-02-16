using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Furniture", menuName = "Create Item/New Furniture")]
    public class FurnitureObject : DeployObject
    {
        [SerializeField] private PlayerObject _pr;
        [SerializeField] private List<Sprite> _sprites;

        private int _index = 0;

        public override void ExecutePrimaryAction(FocusSlotControl control)
        {
            bool wallTmHasTile = control.WallTm.Tilemap.HasTile(Vector3Int.FloorToInt(control.CursorControl.gameObject.transform.position));
            bool groundTmHasTile = control.GroundTm.Tilemap.HasTile(Vector3Int.FloorToInt(control.CursorControl.gameObject.transform.position));
            bool tilActionClear = control.CursorControl.IsClear();

            if (tilActionClear && !wallTmHasTile && groundTmHasTile)
            {
                control.FocusSlot.InventoryItem.Count--;
                GameSignals.ITEM_DEPLOYED.Dispatch();
                control.CursorControl.PlaceDeployable(_prefabToDeploy, _sprites[_index]);
                MMSoundManagerSoundPlayEvent.Trigger(_deploySound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
            }
        }

        public override void ExecuteSecondaryAction(FocusSlotControl control)
        {
            _index++;
            if (_index >= _sprites.Count)
                _index = 0;

            _pr.PlaceDownIndicator.ItemSprite = _sprites[_index];
        }
    }
}

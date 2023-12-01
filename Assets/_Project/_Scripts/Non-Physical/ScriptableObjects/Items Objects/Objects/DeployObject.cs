using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Deployable", menuName = "Create Item/New Deployable")]
    public class DeployObject : ItemObject
    {
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private GameObject _prefabToDeploy;
        [SerializeField] private AudioClip _deploySound;

        public override ToolType ToolType => _baseToolType;
        public override ArmorType ArmorType => _baseArmorType;

        public override void ExecutePrimaryAction(FocusSlotControl control)
        {

        }

        public override void ExecuteSecondaryAction(FocusSlotControl control)
        {
            bool wallTmHasTile = _tmr.WallTilemap.HasTile(Vector3Int.FloorToInt(control.CursorControl.gameObject.transform.position));
            bool groundTmHasTile = _tmr.GroundTilemap.HasTile(Vector3Int.FloorToInt(control.CursorControl.gameObject.transform.position));
            bool tilActionClear = control.CursorControl.IsClear();

            if (tilActionClear && !wallTmHasTile && groundTmHasTile)
            {
                control.FocusSlot.InventoryItem.Count--;
                control.CursorControl.PlaceDeployable(_prefabToDeploy);

                MMSoundManagerSoundPlayEvent.Trigger(_deploySound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
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

            return $"{Description}<br>• Right Click to place<br>• {clickDistance} build distance";
        }
    }
}

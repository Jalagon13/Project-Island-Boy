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
        public override AmmoType AmmoType => _baseAmmoType;
        public override ArmorType ArmorType => _baseArmorType;
        public override GameObject AmmoPrefab => null;
        public override int ConsumeValue => 0;

        public override void ExecutePrimaryAction(SelectedSlotControl control)
        {
            if (PointerHandler.IsOverLayer(5)) return;

            bool wallTmHasTile = _tmr.WallTilemap.HasTile(Vector3Int.FloorToInt(control.TileAction.gameObject.transform.position));
            bool groundTmHasTile = _tmr.GroundTilemap.HasTile(Vector3Int.FloorToInt(control.TileAction.gameObject.transform.position));
            bool tilActionClear = control.TileAction.IsClear();

            if (tilActionClear && !wallTmHasTile && groundTmHasTile)
            {
                control.SelectedSlot.InventoryItem.Count--;
                control.TileAction.PlaceDeployable(_prefabToDeploy);

                AudioManager.Instance.PlayClip(_deploySound, false, true);
            }
        }

        public override void ExecuteSecondaryAction(SelectedSlotControl control)
        {

        }

        public override string GetDescription()
        {
            return $"{Description}<br>• Left Click to place";
        }
    }
}

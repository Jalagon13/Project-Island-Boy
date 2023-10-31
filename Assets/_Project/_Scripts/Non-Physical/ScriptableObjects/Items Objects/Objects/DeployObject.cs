using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Deployable", menuName = "Create Item/New Deployable")]
    public class DeployObject : ItemObject
    {
        [SerializeField] private GameObject _prefabToDeploy;
        [SerializeField] private RuleTile _exclusiveDeployTile;
        [SerializeField] private AudioClip _deploySound;

        public override ToolType ToolType => _baseToolType;
        public override AmmoType AmmoType => _baseAmmoType;
        public override ArmorType ArmorType => _baseArmorType;
        public override GameObject AmmoPrefab => null;
        public override int ConsumeValue => 0;

        public GameObject PrefabToDeploy { get { return _prefabToDeploy; } }

        public override void ExecuteAction(SelectedSlotControl control)
        {
            if (PointerHandler.IsOverLayer(5)) return;

            if(_exclusiveDeployTile != null)
            {
                var taPos = Vector3Int.FloorToInt(control.TileAction.gameObject.transform.position);

                if (control.TMR.GroundTilemap.GetTile(taPos) != _exclusiveDeployTile) return;
            }

            bool wallTmHasTile = control.TMR.WallTilemap.HasTile(Vector3Int.FloorToInt(control.TileAction.gameObject.transform.position));
            bool groundTmHasTile = control.TMR.GroundTilemap.HasTile(Vector3Int.FloorToInt(control.TileAction.gameObject.transform.position));
            bool tilActionClear = control.TileAction.IsClear();

            if (tilActionClear && !wallTmHasTile && groundTmHasTile)
            {
                AudioManager.Instance.PlayClip(_deploySound, false, true);
                control.SelectedSlot.InventoryItem.Count--;
                control.TileAction.PlaceDeployable(_prefabToDeploy);
            }
        }

        public override string GetDescription()
        {
            return $"{Description}<br>• Can be placed";
        }
    }
}

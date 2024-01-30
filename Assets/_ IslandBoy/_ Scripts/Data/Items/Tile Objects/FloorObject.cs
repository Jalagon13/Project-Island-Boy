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
		public override AccessoryType AccessoryType => _baseAccessoryType;

		public override void ExecutePrimaryAction(FocusSlotControl control)
		{
			var pos = Vector3Int.FloorToInt(control.CursorControl.gameObject.transform.position);
			
			if(control.FloorTm.Tilemap == null) return;

			if (!control.WallTm.Tilemap.HasTile(pos) && !control.FloorTm.Tilemap.HasTile(pos))
			{
				control.FocusSlot.InventoryItem.Count--;
				GameSignals.ITEM_DEPLOYED.Dispatch();
				control.FloorTm.Tilemap.SetTile(pos, _floorTile);
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

			return $"* Left Click to place<br>* {clickDistance} build distance<br>{Description}";
		}
	}
}

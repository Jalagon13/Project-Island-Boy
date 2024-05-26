using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
	[CreateAssetMenu(fileName = "New Wall", menuName = "Create Item/New Wall")]
	public class WallObject : ItemObject
	{
		[SerializeField] private RuleTileExtended _wallTile;

		public override ToolType ToolType => _baseToolType;
		public override ToolTier ToolTier => _baseToolTier;
		public override ArmorType ArmorType => _baseArmorType;
		public override AccessoryType AccessoryType => _baseAccessoryType;

		public override void ExecutePrimaryAction(FocusSlotControl control)
		{
			var pos = Vector3Int.FloorToInt(control.CursorControl.gameObject.transform.position);
			bool groundTmHasTile = control.GroundTm.Tilemap.HasTile(Vector3Int.FloorToInt(control.CursorControl.gameObject.transform.position));
			bool wallTmHasTile = control.WallTm.Tilemap.HasTile(Vector3Int.FloorToInt(control.CursorControl.gameObject.transform.position));
			bool tilActionClear = control.CursorControl.IsClear();
			bool onSurface = SceneManager.GetActiveScene().buildIndex == 4;
			
			if (groundTmHasTile && tilActionClear && !wallTmHasTile && onSurface)
			{
				control.FocusSlot.InventoryItem.Count--;
				GameSignals.ITEM_DEPLOYED.Dispatch();
				control.WallTm.Tilemap.SetTile(pos, _wallTile);
				MMSoundManagerSoundPlayEvent.Trigger(_wallTile.PlaceSound, MMSoundManager.MMSoundManagerTracks.Sfx, default, pitch:UnityEngine.Random.Range(0.9f, 1.1f));
				AStarExtensions.Instance.UpdatePathfinding(pos, new(2, 2, 2));
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
			return $"{GetDescriptionBreak()}<color={textBlueColor}>* Material<br>* Left Click to place<br>* {clickDistance} build distance</color={textBlueColor}>";
		}
	}
}

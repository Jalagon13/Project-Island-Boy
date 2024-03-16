using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
	[CreateAssetMenu(fileName = "New Deployable", menuName = "Create Item/New Deployable")]
	public class DeployObject : ItemObject
	{
		[SerializeField] protected GameObject _prefabToDeploy;
		[SerializeField] protected AudioClip _deploySound;
		[SerializeField] private bool _canPlaceOnFloor = true;

		public override ToolType ToolType => _baseToolType;
		public override ToolTier ToolTier => _baseToolTier;
		public override ArmorType ArmorType => _baseArmorType;
		public override AccessoryType AccessoryType => _baseAccessoryType;

		public override void ExecutePrimaryAction(FocusSlotControl control)
		{
			bool wallTmHasTile = control.WallTm.Tilemap.HasTile(Vector3Int.FloorToInt(control.CursorControl.gameObject.transform.position));
			bool floorTmHasTile = control.FloorTm.Tilemap.HasTile(Vector3Int.FloorToInt(control.CursorControl.gameObject.transform.position));
			bool groundTmHasTile = control.GroundTm.Tilemap.HasTile(Vector3Int.FloorToInt(control.CursorControl.gameObject.transform.position));
			bool tilActionClear = control.CursorControl.IsClear();
			bool onSurface = SceneManager.GetActiveScene().buildIndex == 2;

			if (tilActionClear && !wallTmHasTile && groundTmHasTile && onSurface)
			{
				if(floorTmHasTile && !_canPlaceOnFloor)
				{
					return;
				}
				
				control.FocusSlot.InventoryItem.Count--;
				GameSignals.ITEM_DEPLOYED.Dispatch();
				control.CursorControl.PlaceDeployable(_prefabToDeploy);
				MMSoundManagerSoundPlayEvent.Trigger(_deploySound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
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

			return $"{Description}<br>* Left Click to place<br>* {clickDistance} build distance";
		}
	}
}

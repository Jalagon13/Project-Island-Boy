using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace IslandBoy
{
	[CreateAssetMenu(fileName = "New Throw Object", menuName = "Create Item/New Throw Object")]
	public class ThrowObject : ItemObject
	{
		[SerializeField] private PlayerObject _pr;
		[SerializeField] private Throwable _throwPrefab;
		[SerializeField] private AudioClip _launchSound;

		public override ToolType ToolType => _baseToolType;
		public override ArmorType ArmorType => _baseArmorType;
		public override AccessoryType AccessoryType => _baseAccessoryType;

		public override void ExecutePrimaryAction(FocusSlotControl control)
		{
			_pr.Inventory.RemoveItem(this, 1);

			Vector3 playerPosition = control.Player.transform.position;

			Throwable throwObject = Instantiate(_throwPrefab, playerPosition + new Vector3(0, 0.65f), Quaternion.identity);
			Vector3 direction = (control.CursorControl.transform.position - throwObject.transform.position).normalized;
			throwObject.Setup(direction);
			
			MMSoundManagerSoundPlayEvent.Trigger(_launchSound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
		}

		public override void ExecuteSecondaryAction(FocusSlotControl control)
		{
			
		}

		public override string GetDescription()
		{
			return "meow";
		}
	}
}

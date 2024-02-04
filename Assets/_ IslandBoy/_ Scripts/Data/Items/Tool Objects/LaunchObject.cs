using MoreMountains.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Launch Object", menuName = "Create Item/New Launch Object")]
    public class LaunchObject : ItemObject
    {
        [SerializeField] private PlayerObject _pr;
        [SerializeField] private Ammo _launchPrefab;
        [SerializeField] private ItemObject _ammo;
        [SerializeField] private AudioClip _launchSound;

        public override ToolType ToolType => _baseToolType;
        public override ArmorType ArmorType => _baseArmorType;
        public override AccessoryType AccessoryType => _baseAccessoryType;

        public override void ExecutePrimaryAction(FocusSlotControl control)
        {
            if(!_pr.Inventory.Contains(_ammo, 1))
            {
                PopupMessage.Create(control.Player.transform.position, $"Need {_ammo.Name} to shoot!", Color.yellow, Vector2.up, 1f);
                return;
            }

            _pr.Inventory.RemoveItem(_ammo, 1);

            Vector3 playerPosition = control.Player.transform.position;

            Ammo ammo = Instantiate(_launchPrefab, playerPosition + new Vector3(0, 0.65f), Quaternion.identity);
            Vector3 direction = (control.CursorControl.transform.position - ammo.transform.position).normalized;
            ammo.Setup(this, _ammo, direction);

            MMSoundManagerSoundPlayEvent.Trigger(_launchSound, MMSoundManager.MMSoundManagerTracks.Sfx, control.transform.position);
        }

        public override void ExecuteSecondaryAction(FocusSlotControl control)
        {

        }

        public override string GetDescription()
        {
            float clickDistance = 0;
            float damage = 0;

            foreach (var item in DefaultParameterList)
            {
                switch (item.Parameter.ParameterName)
                {
                    case "ClickDistance":
                        clickDistance = item.Value;
                        break;
                    case "Damage":
                        damage = item.Value;
                        break;
                }
            }

            return $"{Description}<br>? {damage} damage<br>? {clickDistance} click distance";
        }
    }
}

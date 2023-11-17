using System;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Launch Object", menuName = "Create Item/New Launch Object")]
    public class LaunchObject : ItemObject
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private Ammo _launchPrefab;
        [SerializeField] private ItemObject _ammo;
        [SerializeField] private AudioClip _launchSound;

        public override ToolType ToolType => _baseToolType;
        public override ArmorType ArmorType => _baseArmorType;
        public override int ConsumeValue => 0;

        public override void ExecutePrimaryAction(SelectedSlotControl control)
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

            AudioManager.Instance.PlayClip(_launchSound, false, true);
        }

        public override void ExecuteSecondaryAction(SelectedSlotControl control)
        {

        }

        public override string GetDescription()
        {
            float clickDistance = 0;
            float power = 0;

            foreach (var item in DefaultParameterList)
            {
                switch (item.Parameter.ParameterName)
                {
                    case "ClickDistance":
                        clickDistance = item.Value;
                        break;
                    case "Power":
                        power = item.Value;
                        break;
                }
            }

            return $"{Description}<br>• {power} hits to creatures<br>• {clickDistance} click distance";
        }
    }
}

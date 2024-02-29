using MoreMountains.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Cast Object", menuName = "Create Item/New Cast Object")]
    public class CastObject : ItemObject
    {
        [SerializeField] private PlayerObject _pr;
        [SerializeField] private FishingHook _launchPrefab;
        [SerializeField] private ItemObject _bait;
        [SerializeField] private AudioClip _launchSound;
        private GameObject _currentHook = null;

        public override ToolType ToolType => _baseToolType;
        public override ToolTier ToolTier => _baseToolTier;
        public override ArmorType ArmorType => _baseArmorType;
        public override AccessoryType AccessoryType => _baseAccessoryType;

        public override void ExecutePrimaryAction(FocusSlotControl control)
        {
            if(!_pr.Inventory.Contains(_bait, 1))
            {
                PopupMessage.Create(control.Player.transform.position, $"Need {_bait.Name} to cast!", Color.yellow, Vector2.up, 1f);
                return;
            }

            if (_currentHook != null)
            {
                if (!_currentHook.GetComponent<FishingHook>()._inMinigame)
                    CreateHook(control);
            }
            else
                CreateHook(control);
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

        private void CreateHook(FocusSlotControl control)
        {
            if (_currentHook != null)
            {
                _currentHook.GetComponent<FishingHook>().StopFishing();
                _currentHook = null;
            }
            
            Vector3 playerPosition = control.Player.transform.position;

            FishingHook hook = Instantiate(_launchPrefab, playerPosition + new Vector3(0, 0.65f), Quaternion.identity);
            _currentHook = hook.gameObject;
            Vector3 direction = (control.CursorControl.transform.position - hook.transform.position).normalized;
            hook.Setup(this, direction);

            MMSoundManagerSoundPlayEvent.Trigger(_launchSound, MMSoundManager.MMSoundManagerTracks.Sfx, control.transform.position);
        }
    }
}

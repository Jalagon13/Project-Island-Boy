using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Spell Object", menuName = "Create Item/New Spell Object")]
    public class SpellObject : ItemObject
    {
        [Space(10)]
        [SerializeField] private int _manaCostPerCast;
        [SerializeField] private Spell _spellPrefab;
        [SerializeField] private Throwable _throwPrefab; // for now, either Spell or Throwable should be null
        [SerializeField] private bool _isThrowable; // false for casting, true for throwing
        [SerializeField] private AudioClip _castSound;

        public override ToolType ToolType => _baseToolType;
        public override ToolTier ToolTier => _baseToolTier;
        public override ArmorType ArmorType => _baseArmorType;
        public override AccessoryType AccessoryType => _baseAccessoryType;

        public override void ExecutePrimaryAction(FocusSlotControl control)
        {
            if (!control.Player.HasEnoughManaToCast(_manaCostPerCast))
            {
                PopupMessage.Create(control.Player.transform.position, $"You have no mana!", Color.yellow, Vector2.up, 1f);
                return;
            }

            if (_isThrowable)
            {
                Vector3 playerPosition = control.Player.transform.position;

                Throwable throwObject = Instantiate(_throwPrefab, playerPosition + new Vector3(0, 0.65f), Quaternion.identity);
                Vector3 direction = (control.CursorControl.transform.position - throwObject.transform.position).normalized;
                throwObject.Setup(direction);
            }
            else
            {
                Spell spell = Instantiate(_spellPrefab, control.CursorControl.transform.position, Quaternion.identity);
                spell.Setup(this);
            }

            control.Player.AddToMp(-_manaCostPerCast);
            MMSoundManagerSoundPlayEvent.Trigger(_castSound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
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
                    case "Damage Max":
                        damage = item.Value;
                        break;
                }
            }
            return $"{GetDescriptionBreak()}<color={textBlueColor}>* {_manaCostPerCast} mana per click<br>* {damage} damage<br>* {clickDistance} cast distance</color={textBlueColor}>";
        }
    }
}

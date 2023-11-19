using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Spell Object", menuName = "Create Item/New Spell Object")]
    public class SpellObject : ItemObject
    {
        [SerializeField] private int _manaCostPerCast;
        [SerializeField] private Spell _spellPrefab;
        [SerializeField] private AudioClip _castSound;

        public override ToolType ToolType => _baseToolType;
        public override ArmorType ArmorType => _baseArmorType;

        public override void ExecutePrimaryAction(SelectedSlotControl control)
        {
            if (control.CursorControl.CurrentClickable == null) return;
            if (!control.Player.HasEnoughManaToCast(_manaCostPerCast))
            {
                PopupMessage.Create(control.Player.transform.position, $"You have no mana!", Color.yellow, Vector2.up, 1f);
                return;
            }

            if (control.CursorControl.CurrentClickable is Entity)
            {
                Entity targetEntity = (Entity)control.CursorControl.CurrentClickable;

                Spell spell = Instantiate(_spellPrefab);
                spell.Setup(targetEntity, this);

                control.Player.AddToMp(-_manaCostPerCast);
                //AudioManager.Instance.PlayClip(_castSound, false, true);
            }
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

            return $"• {_manaCostPerCast} mana per click<br>• {power} hits to creatures<br>• {clickDistance} click distance<br>{Description}";
        }
    }
}

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
        [SerializeField] private AudioClip _castSound;

        public override ToolType ToolType => _baseToolType;
        public override ArmorType ArmorType => _baseArmorType;

        public override void ExecutePrimaryAction(FocusSlotControl control)
        {
            if (!control.Player.HasEnoughManaToCast(_manaCostPerCast))
            {
                PopupMessage.Create(control.Player.transform.position, $"You have no mana!", Color.yellow, Vector2.up, 1f);
                return;
            }

            Spell spell = Instantiate(_spellPrefab, control.CursorControl.transform.position, Quaternion.identity);
            spell.Setup(this);

            control.Player.AddToMp(-_manaCostPerCast);
            //AudioManager.Instance.PlayClip(_castSound, false, true);
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

            return $"* {_manaCostPerCast} mana per click<br>* {damage} damage<br>* {clickDistance} cast distance<br>{Description}";
        }
    }
}

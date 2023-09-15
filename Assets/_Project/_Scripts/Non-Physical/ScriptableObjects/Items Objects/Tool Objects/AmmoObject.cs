using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public enum AmmoType
    {
        None,
        Arrow
    }

    [CreateAssetMenu(fileName = "New Ammo Object", menuName = "Create Item/New Ammo Object")]
    public class AmmoObject : ItemObject
    {
        [SerializeField] private AmmoType _ammoType;
        [SerializeField] private GameObject _ammoPrefab;

        public override ToolType ToolType => _baseToolType;
        public override AmmoType AmmoType => _ammoType;
        public override ArmorType ArmorType => _baseArmorType;
        public override GameObject AmmoPrefab => _ammoPrefab;
        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            
        }

        public override string GetDescription()
        {
            return $"• AmmoType: {_ammoType}<br>{Description}";
        }
    }
}
